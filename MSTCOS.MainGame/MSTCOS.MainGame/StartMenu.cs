using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MSTCOS.GameWorld;
using MSTCOS.Base;
using System.Diagnostics;

namespace MSTCOS.MainGame
{

    public class StartMenu : MSTCOS.Base.IUpdatable, MSTCOS.Base.IDrawable
    {
        Game1 game;

        float scaleStartButton;
        Vector2 position;
        ItemManager<MenuButton> buttons = new ItemManager<MenuButton>();

        public event EventHandler StartPVA;
        public event EventHandler StartAVA;
        public event EventHandler StartVideo;

        MenuButton playerVsAI;
        MenuButton AIVsAI;
        MenuButton Video;
        MenuButton Exit;
        MenuButton Credits;
        bool cdShowing = false;
        float opancity=0;

        Texture2D cd;

        public StartMenu(Game1 game)
        {
            this.game = game;
            Texture2D t;
            t = GameOperators.Content.Load<Texture2D>(@"button1");
            playerVsAI = new MenuButton(new Vector2(GameOperators.GraphicsDevice.Viewport.Width - 256, 100), t);
            playerVsAI.OnClick += new EventHandler(playerVsAI_OnClick);
            t = GameOperators.Content.Load<Texture2D>(@"button2");
            AIVsAI = new MenuButton(new Vector2(GameOperators.GraphicsDevice.Viewport.Width - 256, 220), t);
            AIVsAI.OnClick += new EventHandler(AIVsAI_OnClick);
            t = GameOperators.Content.Load<Texture2D>(@"button3");
            Video = new MenuButton(new Vector2(GameOperators.GraphicsDevice.Viewport.Width - 256, 340), t);
            Video.OnClick += new EventHandler(Video_OnClick);
            t = GameOperators.Content.Load<Texture2D>(@"button5");
            Credits = new MenuButton(new Vector2(GameOperators.GraphicsDevice.Viewport.Width - 256, 460), t);
            Credits.OnClick += new EventHandler(Credits_OnClick);
            t = GameOperators.Content.Load<Texture2D>(@"button4");
            Exit = new MenuButton(new Vector2(GameOperators.GraphicsDevice.Viewport.Width - 256, 580), t);
            Exit.OnClick += new EventHandler(Exit_OnClick);

            cd = GameOperators.Content.Load<Texture2D>(@"Credits");
            buttons.Add(playerVsAI);
            buttons.Add(AIVsAI);
            buttons.Add(Video);
            buttons.Add(Credits);
            buttons.Add(Exit);

        }

        void Credits_OnClick(object sender, EventArgs e)
        {
            cdShowing = true;
            opancity = 0;
        }
        void playerVsAI_OnClick(object sender, EventArgs e)
        {
            //GameState.currentGameState = GameState.State.PlayerVsAI;
            StartPVA(this, EventArgs.Empty);
        }
        void AIVsAI_OnClick(object sender, EventArgs e)
        {
            StartAVA(this, EventArgs.Empty);
        }
        void Video_OnClick(object sender, EventArgs e)
        {
            StartVideo(this, EventArgs.Empty);
        }


        void Exit_OnClick(object sender, EventArgs e)
        {
            Process p = new Process();

            // 设定程序名
            p.StartInfo.FileName = "cmd.exe";
            // 关闭Shell的使用
            p.StartInfo.UseShellExecute = false;
            // 重定向标准输入
            p.StartInfo.RedirectStandardInput = true;
            // 重定向标准输出
            p.StartInfo.RedirectStandardOutput = true;
            //重定向错误输出
            p.StartInfo.RedirectStandardError = true;
            // 设置不显示窗口
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.StandardInput.WriteLine("taskkill /IM AI.exe /F\ntaskkill /IM AI.exe /F\ntaskkill /IM java.exe /F\ntaskkill /IM java.exe /F\ntaskkill /IM MSTCOS.MainGame.exe /F\n");
            p.StandardInput.WriteLine("exit");
            p.Close();

            game.Exit();
        }


        public void Update(GameTime gameTime)
        {
            if (!cdShowing)
            {
                buttons.Update(gameTime);
            }
            else if (InputState.IsMouseButtonPressed(MouseButton.LeftButton))
            {
                cdShowing = false;
            }
        

            //if (nowMouseState.X >= rectStartMenu.X  && nowMouseState.X <= rectStartMenu.X+rectStartMenu.Width
            //    && nowMouseState.Y >= rectStartMenu.Y  && nowMouseState.Y <= rectStartMenu.Y+rectStartMenu.Height )
            //{
            //    scaleStartButton = 1.2f;
            //    if (nowMouseState.LeftButton == ButtonState.Pressed) GameState.currentGameState = GameState.State.PlayerVsAI;
            //}
            //else
            //{
            //    scaleStartButton = 1f;
            //}
        }

        public void Draw(GameTime gameTime)
        {
            float elapsedTime=(float)gameTime.ElapsedGameTime.TotalSeconds;
            GameOperators.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.NonPremultiplied);
            GameOperators.SpriteBatch.Draw(GameOperators.Content.Load<Texture2D>(@"background3"),
                new Rectangle(0, 0, Base.GameOperators.GraphicsDevice.Viewport.Width, Base.GameOperators.GraphicsDevice.Viewport.Height), Color.White);
        
                GameOperators.SpriteBatch.Draw(cd,
                new Rectangle(game.GraphicsDevice.Viewport.Width/2-cd.Width/2, 0,cd.Width, cd.Height), Color.White.CrossAlpha(opancity));
            GameOperators.SpriteBatch.End();
            if (!cdShowing)
            {
                buttons.Draw(gameTime);
                opancity = MathHelper.Clamp(opancity - 0.7f * elapsedTime, 0, 1);
            }
            else opancity = MathHelper.Clamp(opancity + 0.5f * elapsedTime, 0, 1);
            
        }


    }

    class MenuButton : Base.IUpdatable, Base.IDrawable
    {
        public Vector2 Position;
        public Texture2D Texture;
        public event EventHandler OnClick;
        bool isMouseOn;
        public MenuButton(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            isMouseOn = IsMouseOn();
            if (isMouseOn && InputState.IsMouseButtonPressed(MouseButton.LeftButton))
            {
                OnClick(this, EventArgs.Empty);
            }

        }

        public void Draw(GameTime gameTime)
        {
            float scale;
            if (isMouseOn)
            {
                InputState.CursorState = CursorState.Select;
                scale = 1.1f;
            }
            else scale = 1f;
            GameOperators.SpriteBatch.Begin();

            GameOperators.SpriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height),
                Color.White, 0, new Vector2(Texture.Width / 2, Texture.Height / 2), scale, SpriteEffects.None, 0);
            GameOperators.SpriteBatch.End();
        }

        bool IsMouseOn()
        {
            Rectangle rectStartMenu = new Rectangle((int)(Position.X - 0.5 * Texture.Width), (int)(Position.Y - 0.5 * Texture.Height),
                (int)(1 * Texture.Width), (int)(0.8 * Texture.Height));

            if (InputState.CurrentMousePosition.X >= rectStartMenu.X && InputState.CurrentMousePosition.X <= rectStartMenu.X + rectStartMenu.Width
            && InputState.CurrentMousePosition.Y >= rectStartMenu.Y && InputState.CurrentMousePosition.Y <= rectStartMenu.Y + rectStartMenu.Height)
            {
                return true;
            }
            else return false;
        }
    }





}
