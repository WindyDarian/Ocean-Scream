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
    /// <summary>
    /// by 刘欣
    /// 显示时间
    /// </summary>
    public class SideInfoLabel: MSTCOS.Base.IDrawable
    {

        SpriteFont font = GameOperators.Content.Load<SpriteFont>("defaultFont");
        SpriteFont scorefont = GameOperators.Content.Load<SpriteFont>("scoreFont1");

        Vector2 messagePosition ;
        InfoCollector collector;
        TimeManager timeManager;

        public SideInfoLabel(TimeManager timeManager, InfoCollector collector)
        {
            this.timeManager = timeManager;
            this.collector = collector;
        }

        public void Draw(GameTime gameTime)
        {
            //自适应分辨率 范若余
            string t = timeManager.getTimeStringforDisplay();
            GameOperators.SpriteBatch.Begin();
            /*
            GameOperators.SpriteBatch.DrawString(font, "Name    :" + collector.Right.PlayerName, new Vector2(1050,40), Color.Red);
            GameOperators.SpriteBatch.DrawString(font, "Ship    :" + collector.Right.ShipSum, new Vector2(1050, 80), Color.Red);
            GameOperators.SpriteBatch.DrawString(font, "Resource:" + collector.Right.ResSum, new Vector2(1050, 120), Color.Red);
            //蓝方
            GameOperators.SpriteBatch.DrawString(font, "Name    :" + collector.Left.PlayerName, new Vector2(50, 560), Color.Blue);
            GameOperators.SpriteBatch.DrawString(font, "Ship    :" + collector.Left.ShipSum, new Vector2(50, 600), Color.Blue);
            GameOperators.SpriteBatch.DrawString(font, "Resource:" + collector.Left.ResSum, new Vector2(50, 640), Color.Blue);
            */
            //时间
            GameOperators.SpriteBatch.DrawString(scorefont,t , new Vector2(GameOperators.GraphicsDevice.Viewport.Width / 2, GameOperators.GraphicsDevice.Viewport.Height / 2), new Color(Color.Black.R, Color.Black.G, Color.Black.B, 60), 0, scorefont.MeasureString(t) / 2, 3.0f, SpriteEffects.None, 0);
            GameOperators.SpriteBatch.End();
        }
    }
}
