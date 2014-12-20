using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace MSTCOS.GameWorld
{
    public class ReplayController
    {

        GameReplay curReplay;
        static bool enabled = false;
        public static bool Enalble
        {
            get { return enabled; }
        }
        World world;
        bool isSave = false;

        public ReplayController(World world)
        {
            enabled = false;
            isSave = false;
            this.world = world;
            curReplay = new GameReplay();
        }

        public void getMoveTo(int sourceNum, float x, float y, string time)
        {
            string temp = sourceNum.ToString() + ";" + "MT;" + x.ToString() + ";" + y.ToString() + ";" + time;
            curReplay.addOperate(temp);
        }
        public void getAttack(int sourceNum, int targetNum, string time)
        {
            string temp = sourceNum.ToString() + ";" + "AT;" + targetNum.ToString() + ";" + time;
            curReplay.addOperate(temp);
        }

        public void getStop(int sourceNum, int state, string time)
        {
            string temp = sourceNum.ToString() + ";" + "Stop;" + state.ToString() + ";" + time;
            curReplay.addOperate(temp);
        }
        public void getStartRotating(int sourceNum, int state, float x, float y, string time)
        {
            string temp = sourceNum.ToString() + ";" + "SR;" + state.ToString() + ";" + x.ToString() + ";" + y.ToString() + ";" + time;
            curReplay.addOperate(temp);
        }

        public void getStartRotating(int sourceNum, int state, float degree, string time)
        {
            string temp = sourceNum.ToString() + ";" + "SR;" + state.ToString() + ";" + degree.ToString() + ";" + time;
            curReplay.addOperate(temp);
        }
        public void getStartMoving(int sourceNum, string time)
        {
            string temp=sourceNum.ToString() + ";" + "SM;" + time;
            curReplay.addOperate(temp);
        }

        public void recordState()
        {
            foreach (Ship s in world.Ships)
            {
                string temp = s.ID.ToString() + ";" + "ST;" + s.Position.X.ToString() + ";" + s.Position.Y.ToString() + ";" + s.CurrentSpeed.ToString() + ";" + s.RadianRotation.ToString() + ";" + s.Armor.ToString() + ";" + world.timeManager.getTimeStringforRecord();
                curReplay.addOperate(temp);
            }
        }

        public void saveReplay()
        {
            if (!isSave)
            {
                curReplay.saveReplay(world.Collector,world.CurrentMap);
                isSave = true;
            }
        }

        public void setReplay(string replayPath)
        {
            enabled = true;
            curReplay.loadReplay(replayPath, world);
            world.timeManager.Start();
        }

        public void Update(GameTime gameTime)
        {
            curReplay.checkAction(world.timeManager.FullMilisecond, world.Ships);
        }
    }
}
