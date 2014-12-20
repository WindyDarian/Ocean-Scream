using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MSTCOS.GameWorld;
using Microsoft.Xna.Framework;

namespace MSTCOS.Network
{
    public class AISocket
    {
        public int faction;
        string AIName = "";
        byte r = 0, g = 0, b = 0;

        TcpClient curClient;
        public MessageManager messageManager;
        StreamReader reader;
        StreamWriter writer;
        bool isActive;
        int messageCooldown;
        Queue<string> messageQueue;
        RequestManager requestManager;
        World world;
        NetworkStream netStream;
        Thread messageListener;

        public AISocket(TcpClient client, int faction, World world, RequestManager requestManager,ReplayController repController)
        {
            this.requestManager = requestManager;
            curClient = client;

            netStream = client.GetStream();
            reader = new StreamReader(netStream);
            writer = new StreamWriter(netStream);
            this.faction = faction;
            this.world = world;
            getAIinfo();
            if (AIName == "") AIName = "Faction" + faction.ToString();
            world.InitShip(faction, new Color(r, g, b), AIName, FactionControllerType.AI);

            isActive = true;
            messageManager = new MessageManager(this, faction, world, repController);
            messageCooldown = 0;
            messageQueue = new Queue<string>();
        }

        public void start()
        {
            messageListener = new Thread(new ThreadStart(startListening));
            messageListener.Start();
        }
        public void stop()
        {
            try
            {
                messageListener.Abort();
            }
            catch 
            {
            }
        }

        public void startListening()
        {
            while (true)
            {
                if (!isActive) break;
                if (curClient != null && netStream!=null)
                {
                    try
                    {
                        String message = "";
                        if (reader.BaseStream.CanRead)
                            message = reader.ReadLine();
                        if (message != "")
                        {
                            String[] messages = message.Split('\n');
                            foreach (String m in messages)
                            {
                                if (m != null && m != "")
                                {
                                    //Console.WriteLine("request:" + m);
                                    messageQueue.Enqueue(m);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    Thread.Sleep(50);
                }
            }
        }

        public void send(string s)
        {
            try
            {
                writer.WriteLine(s);
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void sendResource()
        {
            try
            {
                string s = "";
                List<ResourceArea> resources = world.Resources.Items;

                foreach (ResourceArea resource in resources)
                {
                    s += resource.ID + "," + resource.Position.X + "," + resource.Position.Y + ";";
                }
                send(s);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        

        public void setWinner(int winner)
        {
            if (winner == 0) send("draw");
            else if (winner == faction) send("win");
            else send("lose");
            isActive = false;
            curClient.Close();
            curClient = null;
        }


        public void getAIinfo()
        {
            try
            {
                String message = reader.ReadLine();
                if (message != "")
                {
                    int pos = message.IndexOf(";");
                    AIName = message.Substring(0, pos);
                    message = message.Substring(pos + 1);

                    pos = message.IndexOf(";");
                    message = message.Substring(pos + 1);
                    pos = message.IndexOf(";");
                    r = byte.Parse(message.Substring(0, pos));
                    message = message.Substring(pos + 1);
                    pos = message.IndexOf(";");
                    g = byte.Parse(message.Substring(0, pos));
                    message = message.Substring(pos + 1);
                    pos = message.IndexOf(";");
                    b = byte.Parse(message.Substring(0, pos));
                }
                Thread.Sleep(50);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void refresh()
        {
            messageCooldown--;
            if (messageCooldown < 0)
                messageCooldown = 0;
            if ((messageCooldown == 0) && (messageQueue.Count != 0))
            {
                requestManager.addRequest(new AIRequest(this, messageQueue.Dequeue()));
                messageCooldown = 6;
            }
            //if ((messageQueue.Count != 0))
            //{
            //    requestManager.addRequest(new AIRequest(this, messageQueue.Dequeue()));
            //}
        }
    }
}
