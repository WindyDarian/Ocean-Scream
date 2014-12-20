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
using MSTCOS.Base;
using MSTCOS.GameWorld;

namespace MSTCOS.Network
{
    public class MessageManager
    {
        Faction myfaction;
        int faction;
        AISocket socket;
        World world;
        ReplayController repController;

        public MessageManager(AISocket socket, int faction, World world, ReplayController repController)
        {
            this.socket = socket;
            this.faction = faction;
            this.world = world;
            this.repController = repController;
        }

        public void dealWith(string message)
        {
            if (message.Substring(0, 4) == "Data")
            {
                SendData();
            }
            else if (message.Substring(0, 4) == "Stop")
            {
                Stop(message);
            }
            else if (message.Substring(0, 6) == "MoveTo")
            {
                MoveTo(message);
            }
            else if (message.Substring(0, 6) == "Attack")
            {
                Attack(message);
            }
            else if (message.Substring(0, 11) == "StartMoving")
            {
                StartMoving(message);
            }
            else if (message.Substring(0, 13) == "StartRotating")
            {
                StartRotating(message);
            }

        }

        void SendData()
        {
            string response = "";
            if (myfaction == null)
            {
                foreach (var item in world.Factions.Items)
                    if (item.FactionID == faction) myfaction = item;
            }
            response = world.timeManager.FullMilisecond.ToString() + ";";

            List<ResourceArea> resources = world.Resources.Items;
            foreach (ResourceArea resource in resources)
            {
                if (resource.ControllingFaction != null) response += resource.ControllingFaction.FactionID.ToString();
                response += ",";
            }
            response += ";";

            List<Ship> ships = myfaction.ShipsInSight();
            foreach (Ship ship in ships)

            {
                response += ship.ID + "," + ship.Faction.FactionID + "," + ship.Armor + "," + ship.Position.X + "," + ship.Position.Y + ","
                    + ship.Velocity.X + "," + ship.Velocity.Y + "," + ship.CurrentSpeed + "," + ship.Direction.X + "," + ship.Direction.Y + ","
                    + ship.Rotation + "," + ship.IsMoving + "," + ship.IsBlocked + "," + (ship.IsRotating || ship.IsRotatingToAngle) + ","
                    + MathHelper.Clamp(ship.Cannons[0].CooldownRemain, 0, float.MaxValue) + "," + MathHelper.Clamp(ship.Cannons[1].CooldownRemain, 0, float.MaxValue) + ";";
            }
            socket.send(response);
        }

        void MoveTo(string message)
        {
            try
            {
                int pos = message.IndexOf(";");
                message = message.Substring(pos + 1);

                pos = message.IndexOf(";");
                int sourceNum = int.Parse(message.Substring(0, pos));
                message = message.Substring(pos + 1);

                pos = message.IndexOf(";");
                float x = float.Parse(message.Substring(0, pos));
                message = message.Substring(pos + 1);

                pos = message.IndexOf(";");
                float y = float.Parse(message.Substring(0, pos));

                Ship sourceShip = null;

                ItemManager<Ship> ships = world.Ships;
                foreach (Ship ship in ships)
                    if (ship.ID == sourceNum)
                    {
                        sourceShip = ship;
                        break;
                    }

                if ((sourceShip != null) && (sourceShip.Faction.FactionID == faction))
                {
                    sourceShip.MoveTo(new Vector2(x, y));
                    repController.getMoveTo(sourceNum, x, y, world.timeManager.getTimeStringforRecord());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void Attack(string message)
        {
            try
            {
                int pos = message.IndexOf(";");
                message = message.Substring(pos + 1);

                pos = message.IndexOf(";");
                int sourceNum = int.Parse(message.Substring(0, pos));
                message = message.Substring(pos + 1);

                pos = message.IndexOf(";");
                int targetNum = int.Parse(message.Substring(0, pos));

                Ship sourceShip = null;
                Ship targetShip = null;

                ItemManager<Ship> ships = world.Ships;
                foreach (Ship ship in ships)
                {
                    if (ship.ID == sourceNum) sourceShip = ship;
                    if (ship.ID == targetNum) targetShip = ship;
                }

                if ((sourceShip != null) && (sourceShip != targetShip) && (sourceShip.Faction.FactionID == faction) && (targetShip == null || (targetShip != null && (targetShip.Faction.FactionID != faction))))
                {
                    sourceShip.Attack(targetShip);
                    repController.getAttack(sourceNum, targetNum, world.timeManager.getTimeStringforRecord());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void Stop(string message)
        {
            try
            {
                int state = 0;

               if (message.Substring(0, 10) == "StopMoving") state = 1;
                else if (message.Substring(0, 12) == "StopRotating") state = 2;

                int pos = message.IndexOf(";");
                message = message.Substring(pos + 1);

                pos = message.IndexOf(";");
                int sourceNum = int.Parse(message.Substring(0, pos));

                Ship sourceShip = null;
                ItemManager<Ship> ships = world.Ships;
                foreach (Ship ship in ships)
                    if (ship.ID == sourceNum)
                    {
                        sourceShip = ship;
                        break;
                    }
                if ((sourceShip != null) && (sourceShip.Faction.FactionID == faction)){
                    switch (state)
                    {
                        case 1:
                            {
                                sourceShip.StopMoving();
                                break;
                            }
                        case 2:
                            {
                                sourceShip.StopRotating();
                                break;
                            }
                        default:
                            {
                                sourceShip.Stop();
                                break;
                            }
                    }
                    repController.getStop(sourceNum, state, world.timeManager.getTimeStringforRecord());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void StartMoving(string message)
        {
            try
            {
                int pos = message.IndexOf(";");
                message = message.Substring(pos + 1);

                pos = message.IndexOf(";");
                int sourceNum = int.Parse(message.Substring(0, pos));

                Ship sourceShip = null;
                ItemManager<Ship> ships = world.Ships;
                foreach (Ship ship in ships)
                    if (ship.ID == sourceNum)
                    {
                        sourceShip = ship;
                        break;
                    }
                if ((sourceShip != null) && (sourceShip.Faction.FactionID == faction))
                {
                    sourceShip.StartMoving();
                    repController.getStartMoving(sourceNum, world.timeManager.getTimeStringforRecord());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void StartRotating(string message)
        {
            try
            {
                bool flag = false;
                if (message.Substring(0, 15) == "StartRotatingTo") flag = true;

                int pos = message.IndexOf(";");
                message = message.Substring(pos + 1);

                pos = message.IndexOf(";");
                int sourceNum = int.Parse(message.Substring(0, pos));
                message = message.Substring(pos + 1);

                float degree = 0;
                float x = 0;
                float y = 0;

                if (flag)
                {
                    pos = message.IndexOf(";");
                    x = float.Parse(message.Substring(0, pos));
                    message = message.Substring(pos + 1);

                    pos = message.IndexOf(";");
                    y = float.Parse(message.Substring(0, pos));
                }
                else
                {
                    pos = message.IndexOf(";");
                    degree = float.Parse(message.Substring(0, pos));
                }

                Ship sourceShip = null;
                ItemManager<Ship> ships = world.Ships;
                foreach (Ship ship in ships)
                    if (ship.ID == sourceNum)
                    {
                        sourceShip = ship;
                        break;
                    }

                if ((sourceShip != null) && (sourceShip.Faction.FactionID == faction))
                {
                    if (flag)
                    {
                        sourceShip.StartRotating(new Vector2(x, y));
                        repController.getStartRotating(sourceNum, 1, x, y, world.timeManager.getTimeStringforRecord());
                    }

                    else
                    {
                        sourceShip.StartRotating(degree);
                        repController.getStartRotating(sourceNum, 2, degree, world.timeManager.getTimeStringforRecord());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}