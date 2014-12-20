using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MSTCOS.Base;

namespace MSTCOS.GameWorld
{
    public class GameReplay
    {
        List<string> operates = new List<string>();
        int curOperateNum = 0;

        public GameReplay()
        {
        }

        public void addOperate(string opr)
        {
            operates.Add(opr);
        }

        public void saveReplay(InfoCollector collector,string currentMap)
        {
            if (!Directory.Exists(@".\rep\"))
                Directory.CreateDirectory(@".\rep\");
            StreamWriter writer = new StreamWriter(@".\rep\" + collector.left.PlayerName + "VS" + collector.right.PlayerName + ".grep", false);
            writer.WriteLine(currentMap);
            writer.WriteLine(AddPlayerInfo(collector.left));
            writer.WriteLine(AddPlayerInfo(collector.right));
            for (int i = 0; i < operates.Count; i++)
                writer.WriteLine(operates[i]);
            writer.Close();
        }

        private string AddPlayerInfo(playerInfo pInfo)
        {
            return pInfo.FactionNum.ToString() + ";" + pInfo.ShipColor.R.ToString() + ";" + pInfo.ShipColor.G.ToString() + ";" + pInfo.ShipColor.B.ToString() + ";" + pInfo.PlayerName;
        }

        public void loadReplay(string repPath, World world)
        {
            StreamReader reader = new StreamReader(repPath);
            reader.ReadLine();
            string left = reader.ReadLine();
            addFaction(left, world);
            string right = reader.ReadLine();
            addFaction(right, world);
            while (!reader.EndOfStream)
            {
                operates.Add(reader.ReadLine());
            }
            curOperateNum = 0;
            reader.Close();
        }
        /// <summary>
        /// 添加船
        /// </summary>
        /// <param name="info"></param>
        /// <param name="world"></param>
        void addFaction(string info, World world)
        {
            string[] args = info.Split(';');
            world.InitShip(int.Parse(args[0]), new Color(int.Parse(args[1]), int.Parse(args[2]), int.Parse(args[3])), args[4], FactionControllerType.None);
        }


        public void checkAction(int milisec, ItemManager<Ship> ship)
        {
            if (operates.Count == 0)
                return;
            ActionInfo curInfo = new ActionInfo(operates[curOperateNum]);
            while (curInfo.time >= milisec && curOperateNum + 1 < operates.Count)
            {
                bool isBreak = false;
                if (curInfo.OpeType == "ST")
                {
                    foreach (Ship s in ship)
                    {
                        s.Armor = 0;
                    }
                    while (curInfo.OpeType == "ST")
                    {
                        foreach (Ship s in ship)
                        {
                            if (s.ID == int.Parse(curInfo.AcInfoshipID1))
                            {
                                s.Position = curInfo.vec2;
                                s.CurrentSpeed = curInfo.currentSpeed;
                                s.RadianRotation = curInfo.radianRotation;
                                s.Armor = curInfo.armor;
                            }
                        }
                        if (curOperateNum + 1 < operates.Count)
                            curOperateNum++;
                        else
                        {
                            isBreak = true;
                            break;
                        }
                        curInfo.setInfo(operates[curOperateNum]);
                    }
                }
                else
                {
                    foreach (Ship s in ship)
                    {
                        if (s.ID == int.Parse(curInfo.AcInfoshipID1))
                        {
                            switch (curInfo.OpeType)
                            {
                                case "MT":
                                    s.MoveTo(curInfo.vec2);
                                    break;
                                case "SM":
                                    s.StartMoving();
                                    break;
                                case "AT":
                                    foreach (Ship v in ship)
                                        if (v.ID == int.Parse(curInfo.AcInfoshipID2))
                                            s.Attack(v);
                                    break;
                                case "Stop":
                                    switch (curInfo.state)
                                    {
                                        case 1:
                                            s.StopMoving();
                                            break;
                                        case 2:
                                            s.StopRotating();
                                            break;
                                        default:
                                            s.Stop();
                                            break;
                                    }
                                    break;
                                case "SR":
                                    if (curInfo.state == 1)
                                        s.StartRotating(curInfo.vec2);
                                    else
                                        s.StartRotating(curInfo.degree);
                                    break;
                            }
                        }
                    }
                    if (curOperateNum + 1 < operates.Count)
                        curOperateNum++;
                    else
                    {
                        isBreak = true;
                        break;
                    }
                    curInfo.setInfo(operates[curOperateNum]);
                }
                if (isBreak)
                    break;
            }

        }
    }
    class ActionInfo
    {

        public string AcInfoshipID1;
        public string AcInfoshipID2;
        public string OpeType;
        public int time;
        public Vector2 vec2;
        public float currentSpeed;
        public float radianRotation;
        public float armor;
        public int state;
        public float degree;

        public ActionInfo(string opr)
        {
            setInfo(opr);
        }

        public void setInfo(string opr)
        {
            if (opr != "")
            {
                string[] args = opr.Split(';');
                OpeType = args[1];
                switch (OpeType)
                {
                    case "ST":
                        AcInfoshipID1 = args[0];
                        vec2 = new Vector2(float.Parse(args[2]), float.Parse(args[3]));
                        currentSpeed = float.Parse(args[4]);
                        radianRotation = float.Parse(args[5]);
                        armor = float.Parse(args[6]);
                        time = int.Parse(args[7]);
                        break;
                    case "MT":
                        AcInfoshipID1 = args[0];
                        vec2 = new Vector2(float.Parse(args[2]), float.Parse(args[3]));
                        time = int.Parse(args[4]);
                        break;
                    case "SM":
                        AcInfoshipID1 = args[0];
                        time = int.Parse(args[2]);
                        break;
                    case "AT":
                        AcInfoshipID1 = args[0];
                        AcInfoshipID2 = args[2];
                        time = int.Parse(args[3]);
                        break;
                    case "Stop":
                        AcInfoshipID1 = args[0];
                        state = int.Parse(args[2]);
                        time = int.Parse(args[3]);
                        break;
                    case "SR":
                        AcInfoshipID1 = args[0];
                        state = int.Parse(args[2]);
                        if (state == 1)
                        {
                            vec2 = new Vector2(float.Parse(args[3]), float.Parse(args[4]));
                            time = int.Parse(args[5]);
                        }
                        else
                        {
                            degree = float.Parse(args[3]);
                            time = int.Parse(args[4]);
                        }
                        break;
                }
            }
        }
    }
}