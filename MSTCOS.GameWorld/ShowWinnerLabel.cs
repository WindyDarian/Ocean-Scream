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
    public class ShowWinnerLabel : MSTCOS.Base.IDrawable
    {
        SpriteFont scorefont = GameOperators.Content.Load<SpriteFont>(@"scoreFont1");
        int factionNum = -1;
        public int FactionNum
        {
            set { factionNum = value; }
        }
        World world;

        public ShowWinnerLabel(World world)
        {
            this.world = world;
        }
        public void Draw(GameTime gameTime)
        {
            //自适应分辨率 范若余
            string t = GetWinner(0);
            GameOperators.SpriteBatch.Begin();
            GameOperators.SpriteBatch.DrawString(scorefont, t, new Vector2(GameOperators.GraphicsDevice.Viewport.Width / 2, GameOperators.GraphicsDevice.Viewport.Height / 2), Color.Black, 0, scorefont.MeasureString(t)/2, 0.8f, SpriteEffects.None, 0);
            GameOperators.SpriteBatch.End();
        }
        public string GetWinner(int factionNum)
        {
            factionNum = world.Winner;
            string winner;
            if (factionNum == 0)
            {
                return " It's a draw " + "\n" + "Press Enter to Continue" ;
            }
            else
            {
                if (factionNum == 1)
                    winner = world.Collector.left.PlayerName;
                else
                    winner = world.Collector.right.PlayerName;
                return " The winner is " + winner + "\n" + "Press Enter to Continue";
            }
        }
    }
}
