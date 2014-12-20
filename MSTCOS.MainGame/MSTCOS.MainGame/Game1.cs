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
using MSTCOS.Network;
using System.Diagnostics;
using System.IO;

namespace MSTCOS.MainGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public string IP = null;
        public int Port;

        public static AIMessageServer currentServer;
        public static RequestManager requestManager;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PrimitiveBatch primitiveBatch;
        World world;
        FpsShower fps;
        StartMenu startMenu;

        //by 刘欣 李翔
        public  ReplayController repController;
        private int lastTime;
        LabelManager lableManager;
        bool ambientOn;

        bool SaveReplayFlag = true;

        public Game1(string[] args,int resolutionX,int resolutionY,bool ambientOn)
        {
            SoundEffect.DistanceScale = 180;
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            this.ambientOn = ambientOn;
            this.IsMouseVisible = false;
            if (args.Length == 2)
            {
                IP = args[0];
                Port = int.Parse(args[1]);
            }
        }
        
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);
            fps = new FpsShower(this);
            GameOperators.GraphicsDevice = GraphicsDevice;
            GameOperators.SoundManager = new SoundManager();
            GameOperators.SpriteBatch = spriteBatch;
            GameOperators.PrimitiveBatch = primitiveBatch;
            GameOperators.Content = Content;
            Components.Add(fps);

            startMenu = new StartMenu(this);
            GameState.currentGameState = GameState.State.Menu;
            startMenu.StartVideo += new EventHandler(startMenu_StartVideo);
            startMenu.StartAVA += new EventHandler(startMenu_StartAVA);
            startMenu.StartPVA += new EventHandler(startMenu_StartPVA);

            base.Exiting += new EventHandler<EventArgs>(Game1_Exiting);
            base.Initialize();
            Services.AddService(typeof(SpriteBatch), spriteBatch);
        }

        void Game1_Exiting(object sender, EventArgs e)
        {
            if (currentServer != null)
            {
                currentServer.stop();
                currentServer.finalStop();
            }
        }

        void startMenu_StartPVA(object sender, EventArgs e)
        {
            world = new World(1, this,ambientOn,null);
            SaveReplayFlag = false;
            //by 李翔
            lableManager = new LabelManager(world);
            LabelManager.currentLabelState = LabelManager.LabelState.WaitForConnect;
            repController = new ReplayController(world);

            if (requestManager != null) requestManager.clearAll();
            requestManager = new RequestManager(this);
            Components.Add(requestManager);

            if (currentServer != null) currentServer.finalStop();
            currentServer = new AIMessageServer(this, world, requestManager, 1, repController);
            Components.Add(currentServer);
            currentServer.start(IP, Port);
            GameState.currentGameState = GameState.State.PlayerVsAI;
        }

        void startMenu_StartAVA(object sender, EventArgs e)
        {
            world = new World(0,this,ambientOn,null);
            SaveReplayFlag = true;
            //by 李翔
            lableManager = new LabelManager(world);
            LabelManager.currentLabelState = LabelManager.LabelState.WaitForConnect;
            //by 李翔
            lastTime = world.timeManager.FullMilisecond;
            repController = new ReplayController(world);

            if (requestManager != null) requestManager.clearAll();
            requestManager = new RequestManager(this);
            Components.Add(requestManager);

            if (currentServer != null) currentServer.finalStop();
            currentServer = new AIMessageServer(this, world, requestManager, 2, repController);
            Components.Add(currentServer);
            currentServer.start(IP, Port);
            GameState.currentGameState = GameState.State.AIVsAI;
        }

        void startMenu_StartVideo(object sender, EventArgs e)
        {
            if (Directory.Exists(@".\rep"))
            {
                System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
                op.InitialDirectory = @".\rep\";
                op.Title = "请选择一个录像文件";
                op.FileName = "";
                op.Filter = "录像文件(*.grep)|*.grep";
                op.Multiselect = false;
                if (op.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                {
                    StreamReader reader = new StreamReader(op.FileName);
                    world = new World(0, this, ambientOn,reader.ReadLine());//视频组注意，把World构造函数中的null换为相应记录下的CurrentMap
                    reader.Close();
                    SaveReplayFlag = false;
                    repController = new ReplayController(world);
                    lableManager = new LabelManager(world);
                    repController.setReplay(op.FileName);
                    GameState.currentGameState = GameState.State.Video;
                }
                else
                    System.Windows.Forms.MessageBox.Show("提示： 没有选择录像文件！");
            }
            else
                System.Windows.Forms.MessageBox.Show("提示： 没有可播放录像文件！");
        }

        protected override void LoadContent()
        {
            InputState.MouseTexture = Content.Load<Texture2D>(@"Mouse2");
            InputState.SelectMouseTexture = Content.Load<Texture2D>(@"Mouse");
            InputState.AttackMouseTexture = Content.Load<Texture2D>(@"Mouse3");
            Content.Load<SoundEffect>(@"Audio\Cannon");
            Content.Load<SoundEffect>(@"Audio\Explosion");
            Content.Load<Texture2D>(@"explosion");
            Content.Load<Texture2D>(@"CannonBall");
        }

        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            InputState.UpdateInput(this);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (currentServer != null) currentServer.finalStop();
                if (requestManager != null) requestManager.clearAll();

                this.Exit();

            }
            switch (GameState.currentGameState)
            {
                case GameState.State.PlayerVsAI:
                    world.Update(gameTime);
                    //by 李翔
                    if (currentServer.canGameStart)
                        LabelManager.currentLabelState = LabelManager.LabelState.InGame;
                    if (LabelManager.currentLabelState == LabelManager.LabelState.WaitForConnect)
                        lableManager.WaitForConnectLabel.GetAIConncted = currentServer.connectNumber;
                    lableManager.Update(gameTime);
                    break;
                case GameState.State.AIVsAI:
                    world.Update(gameTime);
                    //by 李翔
                    if (lastTime - world.timeManager.FullMilisecond >= 500)
                    {
                        repController.recordState();
                        lastTime = world.timeManager.FullMilisecond;
                    }
                    if (currentServer.canGameStart)
                        LabelManager.currentLabelState = LabelManager.LabelState.InGame;
                    if (LabelManager.currentLabelState == LabelManager.LabelState.WaitForConnect)
                        lableManager.WaitForConnectLabel.GetAIConncted = currentServer.connectNumber;
                    lableManager.Update(gameTime);
                    break;
                case GameState.State.Video:
                    repController.Update(gameTime);
                    world.Update(gameTime);
                    LabelManager.currentLabelState = LabelManager.LabelState.InGame;
                    lableManager.Update(gameTime);
                    break;
                case GameState.State.Menu:
                    startMenu.Update(gameTime);
                    break;
                case GameState.State.Exit:
                    this.Exit();
                    break;
            }
            if (world!= null && world.IsGameEnd)
            {
                if (currentServer != null)
                {
                    currentServer.stop();
                    currentServer.finalStop();
                }
                world.timeManager.Stop();
                if (repController!=null)
                {
                    if (!ReplayController.Enalble && SaveReplayFlag)
                    {
                        repController.recordState();
                        repController.saveReplay();
                    }
                }
                
                if (InputState.IsKeyPressed(Keys.Enter))
                {
                    LabelManager.currentLabelState = LabelManager.LabelState.Menu;
                    GameState.currentGameState = GameState.State.Menu;
                }
                    
            }

            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (GameState.currentGameState)
            {
                case GameState.State.PlayerVsAI:
                    world.Draw(gameTime);
                    lableManager.Draw(gameTime);
                    break;
                case GameState.State.AIVsAI:
                    world.Draw(gameTime);
                    lableManager.Draw(gameTime);
                    break;
                case GameState.State.Video:
                    world.Draw(gameTime);
                    lableManager.Draw(gameTime);
                    break;
                case GameState.State.Menu:
                    startMenu.Draw(gameTime);
                    break;
                case GameState.State.Exit:
                    if (currentServer != null) currentServer.finalStop();
                    if (requestManager != null) requestManager.clearAll();
                    this.Exit();
                    break;
            }
  
            base.Draw(gameTime);
            InputState.DrawMouse(spriteBatch);
        }
    }
}