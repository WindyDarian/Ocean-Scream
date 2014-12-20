using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// 帆
    /// </summary>
    public class Sail:GameObject
    {
        public Sail(World world, Ship ship, Vector2 relatedPosition, Faction faction,float scale):base(world)
        {
            base.ParentObject = ship;
            base.Position = relatedPosition;
            //base.Rotation = 30.0f;
            base.Color = faction.FactionColor;
            base.Scale = scale;
            this.Texture = @"Sail";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
