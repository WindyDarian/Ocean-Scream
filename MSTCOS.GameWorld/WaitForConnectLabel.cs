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

namespace MSTCOS.GameWorld
{
    public class WaitForConnectLabel: MSTCOS.Base.IDrawable
    {
        SpriteFont scorefont = GameOperators.Content.Load<SpriteFont>("scoreFont1");
        private int AIConnected = 0;
        public int GetAIConncted
        {
            get { return AIConnected; }
            set { AIConnected = value; }
        }

        public void Draw(GameTime gameTime)
        {
            //自适应分辨率 范若余
            string t = "AIConnected: " + AIConnected.ToString();
            GameOperators.SpriteBatch.Begin();
            GameOperators.SpriteBatch.DrawString(scorefont, t, new Vector2(GameOperators.GraphicsDevice.Viewport.Width / 2, GameOperators.GraphicsDevice.Viewport.Height / 2), new Color(Color.Black.R, Color.Black.G, Color.Black.B, 60), 0, scorefont.MeasureString(t) / 2, 1.0f, SpriteEffects.None, 0);
            GameOperators.SpriteBatch.End();
        }
    }
}
