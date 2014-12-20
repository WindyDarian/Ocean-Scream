using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MSTCOS.GameWorld;

namespace MSTCOS.Network
{
    public class AIMessageServer : GameComponent 
    {
        List<AISocket> AISockets = new List<AISocket>(5);
        TcpListener listener;
        int RunNumber;
        public int connectNumber;
        public bool isStart = false ;
        public bool canGameStart = false;
        bool listenStatu;
        bool endStatu;
        byte[] locker = new byte[0];
        string ip;
        int port;
        World world;
        RequestManager requestManager;
        ReplayController repController;

        Thread listeningThread;

        public AIMessageServer(Game mainGame, World world, RequestManager requestManager, int RunNumber, ReplayController repController)
            : base(mainGame)
        {
            this.world = world;
            this.requestManager = requestManager;
            this.RunNumber = RunNumber;
            this.repController = repController;
        }

        public void start( string ip , int port ) {
            this.ip = ip;
            this.port = port;
            isStart = true ;
            connectNumber = 0;
            listenStatu = true;
            endStatu = true;
            listeningThread = new Thread(new ThreadStart(waitForConnect));
            listeningThread.Start();
            canGameStart = false;
        }

        void waitForConnect()
        {
            IPAddress myIP = IPAddress.Parse(ip);
            IPEndPoint myEnd = new IPEndPoint(myIP, port);
            if (listener == null)
            {
                listener = new TcpListener(myEnd);
                listener.Start();
            }
            isStart = true ;
            try
            {
                while (true)
                {
                    if (connectNumber >= RunNumber)
                    {
                        canGameStart = true;
                        world.timeManager.Start();

                        foreach (var ai in AISockets){
                            ai.start();
                            ai.send(ai.faction.ToString());
                            ai.sendResource();
                        }
                        Console.WriteLine("ai connect number {0}", connectNumber);
                        break;
                    }

                    listener.BeginAcceptSocket( new AsyncCallback( this.socketCallBack ) , null ) ;

                    while (true) {
                        Thread.Sleep(50);
                        lock (locker)
                        {
                            if (!listenStatu)
                            {
                                listenStatu = true;
                                break;
                            }
                            if (!endStatu)
                            {
                                //listener.Stop();
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void socketCallBack( IAsyncResult result ) {
            if (listener == null) return;
            else
            {
                TcpClient connection = listener.EndAcceptTcpClient(result);

                connectNumber++;

                AISocket socket = new AISocket(connection, connectNumber, world, requestManager,repController);

                AISockets.Add(socket);
                //socket.start();
                lock (locker)
                {
                    listenStatu = false;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (AISocket socket in AISockets)
                socket.refresh();
        }

        public void setWinner(int winner)
        {
            foreach (AISocket socket in AISockets)
                socket.setWinner(winner);
            AISockets.Clear();
            isStart = false;
        }

        public void stop()
        {
            if (AISockets.Count == 2)
            {
                AISockets[0].setWinner(world.Winner);
                AISockets[0].stop();
                AISockets[1].setWinner(world.Winner);
                AISockets[1].stop();
            }
            else if (AISockets.Count == 1)
            {
                AISockets[0].setWinner(world.Winner);
            }
            AISockets.Clear();
            lock (locker)
            {
                endStatu = false;
            }
            isStart = false;
        }

        public void finalStop()
        {
            try
            {
                listeningThread.Abort();
            }
            catch
            {
            }
            if (listener != null)
            {
                
                listener.Stop();
                listener = null;
            }
        }

    }
}
