using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace MSTCOS.Base
{
    /// <summary>
    /// FPS显示
    /// </summary>
    public class FpsShower : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Game game;
        SpriteFont font;
        float i;
        float t;
        int fps = 1;
        int ufps = 1;
        SpriteBatch spriteBatch;
        bool IsFpsShow = false;
        public FpsShower(Game game)
            : base(game)
        {
            this.game = game;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            font = game.Content.Load<SpriteFont>("defaultFont");
            i = 0;
            fps = 2;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            t += elapsedTime;
            if (t > 0.1)
            {

                if (IsFpsShow)
                {

                    if (elapsedTime != 0)
                    {
                        ufps = (int)(1 / elapsedTime);
                    }
                    else ufps = 0;



                }
                t = 0;
            }

  
            
            if (InputState.IsKeyPressed(Keys.F9))
            {

                IsFpsShow = (IsFpsShow == false);
            }

           


            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            i += elapsedTime;
            if (i > 0.1)
            {

                if (IsFpsShow)
                {

                    if (elapsedTime != 0)
                    {
                        fps = (int)(1 / elapsedTime);
                    }
                    else fps = 0;



                }
                i = 0;
            }
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            if (IsFpsShow)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Update FPS:"+ufps.ToString()+" Draw FPS:" + fps.ToString(), new Vector2(500, 0), Color.White);
                spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}