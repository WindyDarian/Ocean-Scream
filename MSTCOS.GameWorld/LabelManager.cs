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
    public class LabelManager : MSTCOS.Base.IUpdatable, MSTCOS.Base.IDrawable
    {
        World world;
        Texture2D tex = GameOperators.Content.Load<Texture2D>(@"WhiteCloth");

        ShowWinnerLabel showWinnerLabel;
        WaitForConnectLabel waitForConnectLabel;
        public WaitForConnectLabel WaitForConnectLabel
        {
            get { return waitForConnectLabel; }
        }

        SideInfoLabel sideInfoLabel;
        private  bool isEnd;
        public bool IsEnd
        {
            set { isEnd = value; }
        }

        public enum LabelState { Menu, WaitForConnect, InGame, GameOver };
        public static LabelState currentLabelState = LabelState.Menu;

        public LabelManager(World world)
        {
            this.world = world;
            sideInfoLabel = new SideInfoLabel(world.timeManager, world.Collector);
            showWinnerLabel = new ShowWinnerLabel(world);
            waitForConnectLabel = new WaitForConnectLabel();
            isEnd = false;
        }

        public void Update(GameTime gameTime)
        {
            switch (currentLabelState)
            {
                case LabelState.Menu:
                    break;
                case LabelState.WaitForConnect:
                    break;
                case LabelState.InGame:
                    isEnd = world.IsGameEnd;
                    if (isEnd)
                    {
                        currentLabelState = LabelState.GameOver;
                    }
                    break;
                case LabelState.GameOver:
                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            switch (currentLabelState)
            {
                case LabelState.Menu:
                    break;
                case LabelState.WaitForConnect:
                    sideInfoLabel.Draw(gameTime);
                    DrawBackground();
                    waitForConnectLabel.Draw(gameTime);
                    break;
                case LabelState.InGame:
                    sideInfoLabel.Draw(gameTime);
                    break;
                case LabelState.GameOver:
                    sideInfoLabel.Draw(gameTime);
                    DrawBackground();
                    showWinnerLabel.Draw(gameTime);
                    break;
            }
        }

        private void DrawBackground()
        {
            GameOperators.SpriteBatch.Begin();
            GameOperators.SpriteBatch.Draw(tex, new Rectangle(0, 0, GameOperators.GraphicsDevice.Viewport.Width, GameOperators.GraphicsDevice.Viewport.Height), Color.White);
            GameOperators.SpriteBatch.End();
        }
    }
}