using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MSTCOS.Base;

namespace MSTCOS.GameWorld.Ambient
{
    /// <summary>
    /// 环境物体
    /// </summary>
    public class AmbientObject:GameObject
    {
        public AmbientObject(World world, Vector2 position,string texture,float scale,Color color,float radianrotation):base(world)
        {
            Position = position;
            Texture = texture;
            this.Scale = scale;
            this.Color = color;
            RadianRotation = radianrotation; 
        }

        public static Cloud CreateCloud(World world,Vector2 position,Vector2 velocity)
        {
            return new Cloud(world, position, "cloud" + Base.GameOperators.Random.Next(1, 7),
                (float)Base.GameOperators.Random.NextDouble() + 0.5f, Color.White.CrossAlpha(0.9f), 0, new Vector2((((float)GameOperators.Random.NextDouble() - 0.5f) * 0.4f + 1) * velocity.X, (((float)GameOperators.Random.NextDouble() - 0.5f) * 0.4f + 1) * velocity.Y));//MathHelper.TwoPi * (float)Base.GameOperators.Random.NextDouble());
        }
    }
}
