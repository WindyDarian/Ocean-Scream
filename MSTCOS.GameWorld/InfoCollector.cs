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
using MSTCOS.Base;
using MSTCOS.GameWorld;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InfoCollector : MSTCOS.Base.IUpdatable
    {
        public playerInfo left, right;
        ItemManager<Ship> shipManager;
        ItemManager<ResourceArea> resourceArea;

        public playerInfo Left
        {
            get { return left; }
        }
        public playerInfo Right
        {
            get { return right; }
        }

        public InfoCollector(ItemManager<Ship> shipsManager, ItemManager<ResourceArea> resourceArea)
        {
            left = new playerInfo();
            right = new playerInfo();

            left.FactionNum = 1;
            right.FactionNum = 2;
            
            this.shipManager = shipsManager;
            this.resourceArea = resourceArea;
        }

        public void Update(GameTime gameTime)
        {
            getAllInfo();
        }

        void getAllInfo()
        {
            left.ResSum=0;
            right.ResSum=0;
            right.ShipSum = 0;
            left.ShipSum = 0;
            foreach (ResourceArea s in resourceArea)
            {
                if (s.ControllingFaction == null)
                { }
                else if (s.ControllingFaction.FactionColor == left.ShipColor)
                {
                    left.ResSum++;
                }
                else
                {
                    right.ResSum++;
                }
            }
        }

        public void GetPlayerInfo(int FacNum, Color col, string str)
        {
            if (FacNum == 1)
            {
                left.PlayerName = str;
                left.ShipColor = col;
            }
            else
            {
                right.PlayerName = str;
                right.ShipColor = col;
            }
        }

        
    }
    public struct playerInfo
    {
        public string PlayerName;
        public Color ShipColor;
        public int ResSum;
        public int ShipSum;
        public int FactionNum;
    }
}
