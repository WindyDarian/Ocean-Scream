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
    /// 波纹
    /// </summary>
    public class WaterWave
    {
        Vector2 translation1 = new Vector2 (0,0);
        Texture2D texture;
        float size = 1024;
        float height = 1536;
        Vector2 t = new Vector2(0, 0);

        Vector2 flowSpeed = new Vector2(0.0f, 0.003f);


        public WaterWave()
        {
            texture = Base.GameOperators.Content.Load<Texture2D>(@"waterwave");
        }
        
        public void Update(float elapsedTime)
        {
            t += flowSpeed * elapsedTime;
            while (t.X > 1 || t.X < 0)
            {
                t.X -= Math.Sign(t.X) * 1;
            }
            while (t.Y > 1 || t.Y < 0)
            {
                t.Y -= Math.Sign(t.Y) * 1;
            }
        }
        public void Draw(Camera camera)
        {

            translation1 = (camera.Center - Vector2.Zero)*camera.Scale;
            float realsize = size * camera.Scale;
            float realHeight = height * camera.Scale;
            translation1 = new Vector2((int)(translation1.X % realsize), (int)(translation1.Y % realHeight));
            Vector2 origin = new Vector2(-realsize,-realHeight);
            Vector2 end = new Vector2 (Base.GameOperators.GraphicsDevice.Viewport.Width+realsize,Base.GameOperators.GraphicsDevice.Viewport.Height+realHeight);
            int im = (int)((end.X - origin.X ) / (size * camera.Scale));
            int jm = (int)((end.Y - origin.Y ) / (height * camera.Scale));
            Base.GameOperators.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.NonPremultiplied); 
            for (int i = -2; i <= im; i++)
            {
                for (int j = -2; j <= jm; j++)
                {
                    Base.GameOperators.SpriteBatch.Draw(texture,new Vector2 (realsize*i,realHeight*j)-translation1+t*(realHeight),null, Color.White.CrossAlpha(0.75f),0,Vector2.Zero,realsize/texture.Width,SpriteEffects.None,1f);
                    //Base.GameOperators.SpriteBatch.Draw(texture, new Rectangle((int)(realsize * i), (int)(realsize * j), (int)realsize, (int)realsize), Color.White);
                }
            }
            Base.GameOperators.SpriteBatch.End(); 
        }
    }
}
