using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MSTCOS.Base;
using Microsoft.Xna.Framework.Graphics;

namespace MSTCOS.GameWorld.Ambient
{
    public class Cloud:AmbientObject
    {
        Vector2 velocity;
        float worldWOver2;
        float worldHOver2;
        float opancity;
        Color staticColor;
        public Cloud(World world, Vector2 position, string texture, float scale, Color color, float radianrotation,Vector2 velocity)
            : base(world, position, texture, scale, color, radianrotation)
        {
            this.velocity = velocity;

            worldWOver2 = world.MapSize.X / 2 * 1.4f;
            worldHOver2 = world.MapSize.Y / 2 * 1.4f;
            staticColor = color;
            BlendState = BlendState.Additive;
           
        }
        public override void Update(GameTime gameTime)
        {
            Position += velocity*(float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X > worldWOver2) Position.X -= worldWOver2*2;
            else if (Position.X < -worldWOver2) Position.X += worldWOver2 * 2;
            else if (Position.Y > worldHOver2) Position.Y -= worldHOver2 * 2;
            else if (Position.Y < -worldHOver2) Position.Y += worldHOver2 * 2;
            
           
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float d = 1 - MathHelper.Clamp(World.CurrentCamera.Scale - 2f / 3f, 0, 1);
            opancity = d;
            Color = staticColor.CrossAlpha(opancity);
            
            base.Draw(gameTime);
        }
    }
}
