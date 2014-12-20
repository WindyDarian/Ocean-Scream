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
using MSTCOS.GameWorld.Ambient;
using MSTCOS.GameWorld.SpriteParticle;
using MSTCOS.Base;

namespace MSTCOS.GameWorld
{
   
    public class World:MSTCOS.Base.IUpdatable,MSTCOS.Base.IDrawable
    {
        string currentMap = "1";
        /// <summary>
        /// 当前地图
        /// </summary>
        public string CurrentMap
        {
            get { return currentMap; }
            set { currentMap = value; }
        }


        Vector2 mapSize = new Vector2(2048, 2048);
        public Vector2 MapSize
        {
            get { return mapSize; }
        }
        /// <summary>
        /// 超出范围的每秒伤害
        /// </summary>
        const float OutOfRangeDamage = 25f;

        /// <summary>
        /// 时间管理 by 李翔 刘欣
        /// </summary>
        public TimeManager timeManager = new TimeManager();
        /// <summary>
        /// 信息收集 by 李翔 刘欣
        /// </summary>
        InfoCollector collector;
        public InfoCollector Collector
        {
            get { return collector; }
        }
        int winner;
        public int Winner
        {
            get { return winner; }
        }


        bool isGameEnd = false;
        /// <summary>
        /// 游戏是否结束
        /// </summary>
        public bool IsGameEnd
        {
            get { return isGameEnd; }
        }


        Game game;

        /// <summary>
        /// 相机管理器
        /// </summary>
        ItemManager<Camera> cameras = new ItemManager<Camera>();

        /// <summary>
        /// 浮动文字管理器
        /// </summary>
        ItemManager<FloatText> texts = new ItemManager<FloatText>();

        private Camera currentCamera;
        /// <summary>
        /// 当前相机
        /// </summary>
        public Camera CurrentCamera
        {
            get { return currentCamera; }
            set { currentCamera = value; }
        }

        ItemManager<Faction> factions = new ItemManager<Faction>();
        /// <summary>
        /// 阵营
        /// </summary>
        public ItemManager<Faction> Factions
        {
            get { return factions; }
            set { factions = value; }
        }

        /// <summary>
        /// 炮弹
        /// </summary>
        ItemManager<CannonBall> cannonBalls = new ItemManager<CannonBall>();

        ItemManager<Ship> ships = new ItemManager<Ship>();
        /// <summary>
        /// 船只
        /// </summary>
        public ItemManager<Ship> Ships
        {
            get { return ships; }
        }

        ItemManager<ResourceArea> resources = new ItemManager<ResourceArea>();
        /// <summary>
        /// 资源
        /// </summary>
        public ItemManager<ResourceArea> Resources
        {
            get { return resources; }
        }

        ItemManager<Island> islands = new ItemManager<Island> ();
        ItemManager<Particle> lowerParticles = new ItemManager<Particle>();
        ItemManager<Particle> upperParticles = new ItemManager<Particle>();
        ItemManager<AmbientObject> lowerAmbients = new ItemManager<AmbientObject>();
        ItemManager<AmbientObject> upperAmbients = new ItemManager<AmbientObject>();

        Camera commonCamera;
        Camera player1Camera;
        Camera player2Camera;

        float cameraSpeed = 30.0f;//鼠标拖到边缘滚屏尚未实现



        Ship mouseOnShip;
        /// <summary>
        /// 鼠标悬停的船
        /// </summary>
        public Ship MouseOnShip
        {
            get { return mouseOnShip; }
        }



        Vector2[] corners=new Vector2[4];
        Faction f1;
        Faction f2;
        Sea sea;
        WaterWave waterWave;
        SmallMap map;
        bool ambientOn;

        List<Ship> humanSelectedShips = new List<Ship>(10);
    
        /// <summary>
        /// 人类玩家当前选择的船
        /// </summary>
        public List<Ship> HumanSelectedShips
        {
            get { return humanSelectedShips; }
            set { humanSelectedShips = value; }
        }


        public World(int InitFactionNumber,Game game,bool ambientOn,string mapName)
        {
            this.game = game;
            commonCamera = new Camera(Vector2.Zero, 1.5f);
            player1Camera = new Camera(Vector2.Zero, 1.5f);
            player2Camera = new Camera(Vector2.Zero, 1.5f);
            cameras.Add(commonCamera);
            cameras.Add(player1Camera);
            cameras.Add(player2Camera);

            currentCamera = commonCamera;

            //by 刘欣 李翔
            collector = new InfoCollector(ships, resources);

            if (InitFactionNumber == 1) InitShip(2, Color.Blue, "Player", FactionControllerType.Player);

            if (mapName == null)
            {
                mapName = GameOperators.Random.Next(1, 7).ToString();
            }

            currentMap = mapName;
            LoadMap(currentMap);

            //添加资源点
            //resources.Add(new ResourceArea(1, 196, new Vector2(-384, -384), null, this));
            //islands.Add(new Island(this, "island1", 0.3f, new Vector2(-384, -384), 32f));
            //resources.Add(new ResourceArea(2, 196, new Vector2(-384, 384), null, this));
            //islands.Add(new Island(this, "island2", 0.3f, new Vector2(-384, 384), 32f));
            //resources.Add(new ResourceArea(3, 196, new Vector2(0, 0), null, this));
            //islands.Add(new Island(this, "island0", 0.3f, new Vector2(0, 0), 32f));
            //resources.Add(new ResourceArea(4, 196, new Vector2(384, -384), null, this));
            //islands.Add(new Island(this, "island3", 0.3f, new Vector2(384, -384), 32f));
            //resources.Add(new ResourceArea(5, 196, new Vector2(384, 384), null, this));
            //islands.Add(new Island(this, "island4", 0.3f, new Vector2(384, 384), 32f));

            sea = new Sea();
            waterWave = new WaterWave();
            for (int i = 0; i < 20; i++)//添加云
            {

                // upperAmbients.Add(AmbientObject.CreateCloud(this,new Vector2(GameOperators.Random.Next(-768,768),GameOperators.Random.Next(-1024,1024)),new Vector2 (10f,2f)));
                upperAmbients.Add(AmbientObject.CreateCloud(this, new Vector2(GameOperators.Random.Next((int)(-MapSize.X / 2 * 1.4f), (int)(MapSize.X / 2 * 1.4f)), GameOperators.Random.Next((int)(-MapSize.Y / 2 * 1.4f), (int)(MapSize.Y / 2 * 1.4f))), new Vector2(10f, 2f)));

            }

            lowerAmbients.Add(new AmbientObject(this, new Vector2(0, 0), "OceanBackground", 2, Color.White, 0));
            lowerAmbients.Add(new AmbientObject(this, new Vector2(0, 0), "OceanBackgroundWord", 0.75f, Color.White, 0));
            for (int i = 0; i < 20; i++)//添加鱼
            {

                // upperAmbients.Add(AmbientObject.CreateCloud(this,new Vector2(GameOperators.Random.Next(-768,768),GameOperators.Random.Next(-1024,1024)),new Vector2 (10f,2f)));
                lowerAmbients.Add(new Fish(this));

            }
            
            map = new SmallMap(ships, resources, currentCamera);

            corners[0] = new Vector2(-mapSize.X / 2,-mapSize.Y / 2);
            corners[1] = new Vector2(mapSize.X / 2, -mapSize.Y / 2);
            corners[2] = new Vector2(mapSize.X / 2, mapSize.Y / 2);
            corners[3] = new Vector2(-mapSize.X / 2, mapSize.Y / 2);
            this.ambientOn = ambientOn;
            
        }

        void LoadMap(string map)
        {
            Map p = GameOperators.Content.Load<Map>(@"Maps\" + map);
            int i = 0;
            foreach (var resp in p.Resources)
            {
                resources.Add(new ResourceArea(i+1, 196, resp, null, this));
                islands.Add(new Island(this, "island"+i.ToString(), 0.3f, resp, 32f));
                i++;
                i %= 5;
            }
        }


        bool oneFactioned = false;
        public void InitShip(int faction, Color color, string name, FactionControllerType type)
        {
            if (faction == 1)
            {
                Color c = color;
                if (f2 != null)
                {
                    Vector3 c1 = c.ToVector3();
                    Vector3 c2 = f2.FactionColor.ToVector3();
                    if (Vector3.Distance(c1, c2) < 0.1f )
                    {
                        if(Vector3.Distance(c2, Color.Red.ToVector3()) >= 0.1f)
                        c = Color.Red;
                        else c = Color.Green;
                    }

                }
                //test
                f1 = new Faction(this, type, 1, color, name);
                factions.Add(f1);

                ships.Add(new Ship(this, new Vector2(-768, -102.4f), 90f, f1, 1));
                f1.Ships.Add(ships.LatestAddedItem);
                ships.Add(new Ship(this, new Vector2(-768, -51.2f), 90f, f1, 2));
                f1.Ships.Add(ships.LatestAddedItem);
                ships.Add(new Ship(this, new Vector2(-768, 0), 90f, f1, 3));
                f1.Ships.Add(ships.LatestAddedItem);
                ships.Add(new Ship(this, new Vector2(-768, 51.2f), 90f, f1, 4));
                f1.Ships.Add(ships.LatestAddedItem);
                ships.Add(new Ship(this, new Vector2(-768, 102.4f), 90f, f1, 5));
                f1.Ships.Add(ships.LatestAddedItem);
               
            }
            else if (faction == 2)
            {
                Color c = color;
                if (f1 != null)
                {
                    Vector3 c1 = c.ToVector3();
                    Vector3 c2 = f1.FactionColor.ToVector3();
                    if (Vector3.Distance(c1, c2) < 0.1f)
                    {
                        if(Vector3.Distance(c2, Color.Blue.ToVector3()) >= 0.1f)
                        c = Color.Blue;
                        else c = Color.Yellow;
                    }

                }
                //test
                f2 = new Faction(this, type, 2, c, name);
                factions.Add(f2);

                ships.Add(new Ship(this, new Vector2(768, -102.4f), -90f, f2, 6));
                f2.Ships.Add(ships.LatestAddedItem);
                ships.Add(new Ship(this, new Vector2(768, -51.2f), -90f, f2, 7));
                f2.Ships.Add(ships.LatestAddedItem);
                ships.Add(new Ship(this, new Vector2(768, 0f), -90f, f2, 8));
                f2.Ships.Add(ships.LatestAddedItem);
                ships.Add(new Ship(this, new Vector2(768, 51.2f), -90f, f2, 9));
                f2.Ships.Add(ships.LatestAddedItem);
                ships.Add(new Ship(this, new Vector2(768, 102.4f), -90f, f2, 10));
                f2.Ships.Add(ships.LatestAddedItem);
               
            }
            //by 刘欣 李翔
            collector.GetPlayerInfo(faction, color, name);
        }


        //bool selectingRec = false;//是否正在拖矩形选框
        Vector2 rec0 = new Vector2(0, 0);
        Vector2 rec1 = new Vector2(0, 0);
       // float rectSelectionTime = 0.1f;
        bool mouseDowned = false;

        private List<Ship> BlockedShips = new List<Ship>();

        private void MoreBlocked()
        {
            for (int i = 0; i < BlockedShips.Count; i++)
            {
                for (int j = 0; j < ships.Items.Count; j++)
                {
                    if (!BlockedShips.Contains(ships.ElementAt(j)))
                    {
                        float d = Vector2.DistanceSquared(BlockedShips.ElementAt(i).Positionl, ships.ElementAt(j).AbsolutePosition);
                        if (d < (float)Math.Pow(BlockedShips.ElementAt(i).BoundingRadius + ships.ElementAt(j).BoundingRadius, 2))
                        {
                            BlockedShips.Add(ships.ElementAt(j));
                        }
                    }
                }
            }
           /*         foreach (Ship s1 in BlockedShips)
                    {
                        foreach (Ship s2 in ships)
                        {
                            if (!BlockedShips.Contains(s2))
                            {
                                float d = Vector2.DistanceSquared(s1.Positionl, s2.AbsolutePosition);
                                if (d < (float)Math.Pow(s1.BoundingRadius + s2.BoundingRadius, 2))
                                {
                                    BlockedShips.Add(s2);
                                }
                            }
                        }
                    }*/
        }

        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            factions.Update(gameTime);
            resources.Update(gameTime);
            ships.Update(gameTime);
            cameras.Update(gameTime);
            
            sea.Update(elapsedTime);
            if (ambientOn) waterWave.Update(elapsedTime);
            
            cannonBalls.Update(gameTime);
            islands.Update(gameTime);
            if (ambientOn) lowerParticles.Update(gameTime);
            if (ambientOn) upperParticles.Update(gameTime);
            if (ambientOn) lowerAmbients.Update(gameTime);
            if (ambientOn) upperAmbients.Update(gameTime);
            map.Update(gameTime);

            //by 刘欣 李翔
            timeManager.Update(gameTime);
            collector.Update(gameTime);

            foreach (var item in ships)
            {
                item.PerformCommand();
                Vector2 p = item.AbsolutePosition;
                if (p.X > mapSize.X / 2 || p.X < -mapSize.X / 2 || p.Y > mapSize.Y / 2 || p.Y < -mapSize.Y / 2)
                {
                    item.Armor -= OutOfRangeDamage * elapsedTime;
                }
            }

            float t = - (InputState.previousMouseState.ScrollWheelValue - InputState.currentMouseState.ScrollWheelValue) / 1000f;
            if (t != 0)
            {
                commonCamera.Scale += t;
            }
            #region 判断胜负
            if(!isGameEnd)
            {
            if(f1!=null &&f2!= null)
            {
            //by 刘欣 李翔 剩余时间为零
            if (timeManager.FullMilisecond <= 0)
            {
                isGameEnd = true;
                //首先判断船的数量
                int s1 = ships.Count(p => p.Faction == f1);
                int s2 = ships.Count(p => p.Faction == f2);
                if (s1 > s2)
                {
                    winner = 1;
                }
                else if (s1 < s2)
                {
                    winner = 2;
                }
                else
                {
                    //然后判断岛的数量
                    if (f1.ResourceNum > f2.ResourceNum)
                    {
                        winner = 1;
                    }
                    else if (f1.ResourceNum < f2.ResourceNum)
                    {
                        winner = 2;
                    }
                    else
                    {
                        //然后判断总血量
                        float a1 = 0;
                        float a2 = 0;
                        foreach (var s in ships)
                        {
                            if (s.Faction == f1)
                            {
                                a1 += s.Armor;
                            }
                            else if (s.Faction == f2)
                            {
                                a2 += s.Armor;
                            }
                        }
                        if (a1 > a2)
                        {
                            winner = 1;
                        }
                        else if (a1 < a2)
                        {
                            winner = 2;
                        }
                        else
                        {
                            //最后判断造成的总伤害
                            if (f1.TotalDamage > f2.TotalDamage) { winner = 1; }
                            else if (f1.TotalDamage < f2.TotalDamage) { winner = 2; }
                            else { winner = 0; }
                        }
                    }
                }

            }

            if ( ships.Count(p => p.Faction == f1) == 0)//如果f1全挂了
            {
                //其它操作有待添加
                isGameEnd = true;

                winner = 2;
            }

            if ( ships.Count(p => p.Faction == f2) == 0)//如果f2全挂了
            {
                //其它操作有待添加
                isGameEnd = true;
                
                winner = 1;
            }
            }
            }
#endregion

            if (game.IsActive)
            {
                #region 调整视野
                float screenWInGame = game.GraphicsDevice.Viewport.Width / currentCamera.Scale;
                float screenHInGame = game.GraphicsDevice.Viewport.Height / currentCamera.Scale;

                if (InputState.CurrentMousePosition.X <= 2 || InputState.currentMouseState.Y <= 2
                     || InputState.CurrentMousePosition.X >= GameOperators.GraphicsDevice.Viewport.Width - 2
                     || InputState.CurrentMousePosition.Y >= GameOperators.GraphicsDevice.Viewport.Height - 2)
                {
                    Vector2 screenCenter = new Vector2(GameOperators.GraphicsDevice.Viewport.Width / 2, GameOperators.GraphicsDevice.Viewport.Height / 2);
                    if (screenCenter != InputState.CurrentMousePosition)
                    {
                        Vector2 translator = Vector2.Normalize(InputState.CurrentMousePosition - screenCenter) * 1000f * (float)gameTime.ElapsedGameTime.TotalSeconds /currentCamera.Scale;
                        float borderX = Math.Abs(MapSize.X / 2 - screenWInGame / 2);
                        float borderY = Math.Abs(MapSize.Y / 2-screenHInGame/2);
                        //currentCamera.Center += translator;
                        if(translator.X>0&& currentCamera.Center.X<=borderX)
                            currentCamera.Center.X+=translator.X;
                        else if (translator.X<0&& currentCamera.Center.X>=-borderX)
                            currentCamera.Center.X+=translator.X;
                        if(translator.Y>0&& currentCamera.Center.Y<=borderY)
                            currentCamera.Center.Y+=translator.Y;
                        else if (translator.Y<0&& currentCamera.Center.Y>=-borderY)
                            currentCamera.Center.Y+=translator.Y;


                    }
                }

                #endregion
                if (InputState.IsKeyPressed(Keys.Space))
                {
                    currentCamera.Center = Vector2.Zero;
                }
                if (InputState.IsKeyPressed(Keys.Escape))
                {
                    isGameEnd = true;
                }
                #region 鼠标选择
                mouseOnShip = null;
                if (InputState.currentMouseState.LeftButton ==ButtonState.Pressed)
                {
                    if (!mouseDowned)
                    {
                        rec0 = InputState.CurrentMousePosition;
                        rec1 = rec0;
                        mouseDowned = true;
                    }
                    else
                    {
                        rec1 = InputState.CurrentMousePosition;

                    }
                }
                else if (InputState.currentMouseState.LeftButton == ButtonState.Released)
                {
                    
                    if (mouseDowned)
                    {
                        bool playerControled = ships.Count(p => p.Faction.ControllerType == FactionControllerType.Player)>0;
                        humanSelectedShips.Clear();

                        float ax, bx, ay, by;
                        ax = currentCamera.ReversePoint(rec0).X;
                        ay = currentCamera.ReversePoint(rec0).Y;
                        bx = currentCamera.ReversePoint(rec1).X;
                        by = currentCamera.ReversePoint(rec1).Y;
                        BoundingBox bb = new BoundingBox(new Vector3(MathHelper.Min(ax,bx),MathHelper.Min(ay,by), 0),new Vector3(MathHelper.Max(ax,bx),MathHelper.Max(ay,by), 0));
                
                        foreach (var item in ships)
                        {
                            BoundingSphere bs = new BoundingSphere(new Vector3(item.AbsolutePosition, 0), item.SelectionRadius);
                            if (bb.Intersects(bs))
                            {
                                if (!playerControled || (playerControled && item.Faction.ControllerType == FactionControllerType.Player))
                                {

                                    humanSelectedShips.Add(item);
                                }
                            }
                        }
                        mouseDowned = false;
                    }
                }

                foreach (var item in ships)
                {
                    if (item.IsMouseOn())
                    {
                        mouseOnShip = item;

                        if (InputState.CursorState == Base.CursorState.Normal)
                        {
                            InputState.CursorState = Base.CursorState.Select;
                        }
                       
                        if (InputState.IsMouseButtonPressed(MouseButton.LeftButton)&&(rec0==rec1))
                        {
                            humanSelectedShips.Clear();
                            humanSelectedShips.Add(item);//选中船只
                        }
                        break;
                    }

                }
                #endregion
            }
            texts.Update(gameTime);

            foreach (var item in ships)
            {
                foreach (var item2 in ships)
                {
                    if (item!= item2)
                    {
                        float d= 0;
                        if (IsShipCollided(item, item2,ref d))
                        {

                            if (Vector2.DistanceSquared(item.Positionl, item2.AbsolutePosition) > d)//如果碰撞且距离拉近（卡住）
                            {
                                float dmg = item.GetKnockDamage(item2);
                                if (dmg>0)
                                {
                                    item2.ReceiveDamage(dmg, 0.1f, true);
                                    item.ReceiveDamage(dmg/2, 0.1f, true);
                                }
                                item.Block(Vector2.Normalize(item.AbsolutePosition - item2.AbsolutePosition) * (item.BoundingRadius + item2.BoundingRadius + 0.2f - Vector2.Distance(item.AbsolutePosition, item2.AbsolutePosition)));
                                //item.Block();//那么就触发碰撞事件
                                /*if (!BlockedShips.Contains(item) && !BlockedShips.Contains(item2))
                                {
                                    BlockedShips.Add(item);
                                    BlockedShips.Add(item2);
                                }*/
                            }
                            
                        }

                        //d < (float)Math.Pow(object1.BoundingRadius + object2.BoundingRadius,2)
                        if (item.IsMoving && Vector2.DistanceSquared(item.AbsolutePosition, item2.AbsolutePosition) <= (float)Math.Pow(item.BoundingRadius + item2.BoundingRadius+0.5, 2) )
                            item.IsBlocked = true;
                    }
                }
                foreach (var island in islands)
                {
                        float d= 0;
                        if (IsShipCollided(item, island, ref d))
                        {

                            if (Vector2.DistanceSquared(item.Positionl, island.AbsolutePosition) > d)//如果碰撞且距离拉近（卡住）
                            {
                                item.Block(Vector2.Normalize(item.AbsolutePosition - island.AbsolutePosition) * (item.BoundingRadius + island.BoundingRadius+0.2f - Vector2.Distance(item.AbsolutePosition, island.AbsolutePosition)));
                                /*if (!BlockedShips.Contains(item))
                                {
                                    BlockedShips.Add(item);
                                }*/
                            }
                        }
                }
            }

            /*if (BlockedShips.Count != 0)
            {
                MoreBlocked();
                foreach (Ship s in BlockedShips)
                {
                    s.Block();
                }
                BlockedShips.Clear();
            }*/
         
        }

        public void Draw(GameTime gameTime)
        {
            sea.Draw(currentCamera);
            if (ambientOn) waterWave.Draw(currentCamera);
            if (ambientOn) lowerAmbients.Draw(gameTime);
            foreach (var item in ships)
            {
                if (item.IsDrawingSelection)
                {
                    item.DrawCircle();
                }
            }
            if (ambientOn) lowerParticles.Draw(gameTime);
            resources.Draw(gameTime);
            islands.Draw(gameTime);
            ships.Draw(gameTime);
            if (ambientOn) upperParticles.Draw(gameTime);
            if (ambientOn) upperAmbients.Draw(gameTime);

            GameOperators.PrimitiveBatch.Begin(PrimitiveType.LineList);
            #region 绘制选择框
            if (mouseDowned)
            {
             
                GameOperators.PrimitiveBatch.AddVertex(rec0, Color.Yellow);
                GameOperators.PrimitiveBatch.AddVertex(new Vector2(rec1.X, rec0.Y), Color.Yellow);
                GameOperators.PrimitiveBatch.AddVertex(new Vector2(rec1.X, rec0.Y), Color.Yellow);
                GameOperators.PrimitiveBatch.AddVertex(rec1, Color.Yellow);
                GameOperators.PrimitiveBatch.AddVertex(rec1, Color.Yellow);
                GameOperators.PrimitiveBatch.AddVertex(new Vector2(rec0.X, rec1.Y), Color.Yellow);
                GameOperators.PrimitiveBatch.AddVertex(new Vector2(rec0.X, rec1.Y), Color.Yellow);
                GameOperators.PrimitiveBatch.AddVertex(rec0, Color.Yellow);

            }
            #endregion
            #region 绘制边界
            GameOperators.PrimitiveBatch.AddVertex(currentCamera.TransformPoint(corners[0]), Color.Green);
            GameOperators.PrimitiveBatch.AddVertex(currentCamera.TransformPoint(corners[1]), Color.White);
            GameOperators.PrimitiveBatch.AddVertex(currentCamera.TransformPoint(corners[1]), Color.White);
            GameOperators.PrimitiveBatch.AddVertex(currentCamera.TransformPoint(corners[2]), Color.Green);
            GameOperators.PrimitiveBatch.AddVertex(currentCamera.TransformPoint(corners[2]), Color.Green);
            GameOperators.PrimitiveBatch.AddVertex(currentCamera.TransformPoint(corners[3]), Color.White);
            GameOperators.PrimitiveBatch.AddVertex(currentCamera.TransformPoint(corners[3]), Color.White);
            GameOperators.PrimitiveBatch.AddVertex(currentCamera.TransformPoint(corners[0]), Color.Green);
            #endregion
            GameOperators.PrimitiveBatch.End();

            map.Draw(gameTime);
            
       
            foreach (var item in ships)
            {
                if (item.IsDrawingArmorBar)
                {
                    Vector2 screenPoint = CurrentCamera.TransformPoint(item.AbsolutePosition);
                    DrawArmorBar(item, new Vector2((int)(screenPoint.X - GameSettings.ArmorBarLength / 2), (int)(screenPoint.Y - GameSettings.ArmorBarWidth / 2 - GameSettings.ArmorBarHeight * CurrentCamera.Scale)));
                }
                if (item.IsDrawingTargetInf)
                {
                    item.DrawTargetInf();
                }
     
            }

            DrawResourcesOwners();

            cameras.Draw(gameTime);
            factions.Draw(gameTime);
            cannonBalls.Draw(gameTime);
            texts.Draw(gameTime);
            Vector2 v1 = new Vector2(20, GameOperators.GraphicsDevice.Viewport.Height - 250);
            Vector2 v2 = new Vector2(GameOperators.GraphicsDevice.Viewport.Width - 210, GameOperators.GraphicsDevice.Viewport.Height - 250);

            if (f1 != null) DrawFactionInf(f1, v1);
            if (f2 != null) DrawFactionInf(f2, v2);
            
        }

        void DrawResourcesOwners()
        {
            Texture2D t = GameOperators.Content.Load<Texture2D>(@"resourceicon");
            float width= t.Width*0.1f;
            Vector2 startPoint = new Vector2(game.GraphicsDevice.Viewport.Width / 2 - width*2.5f, 10);

            GameOperators.SpriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.NonPremultiplied);
            for (int i = 0; i < resources.MemberList.Count; i++)
            {
                Color c = Color.White;
                float opancity;
                if (resources.MemberList[i].ControllingFaction == null)
                {
                    opancity = 0.4f;
                }
                else
                {
                    c = resources.MemberList[i].ControllingFaction.FactionColor;
                    opancity = 1f;
                }
                GameOperators.SpriteBatch.Draw(t, new Rectangle((int)startPoint.X + (int)width * i, (int)startPoint.Y, (int)width, (int)width),c.CrossAlpha(opancity));
            }
            GameOperators.SpriteBatch.End();
            foreach (var item in resources)
            {
                item.DrawUI();
            }
        }
        /// <summary>
        /// 绘制阵营血条
        /// </summary>
        /// <param name="f"></param>
        /// <param name="position"></param>
        void DrawFactionInf(Faction f, Vector2 position)
        {
            SpriteFont sf =GameOperators.Content.Load<SpriteFont>("defaultFont");
            GameOperators.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            GameOperators.SpriteBatch.DrawString(sf, f.Name, position+new Vector2(95,25), f.FactionColor,0,sf.MeasureString(f.Name)/2,1,SpriteEffects.None,0.5f);
         
            GameOperators.SpriteBatch.Draw(GameOperators.Content.Load<Texture2D>(@"bar"), position, null, Color.White,0,new Vector2 (0,0),0.5f,SpriteEffects.None,0.9f);

            GameOperators.SpriteBatch.End();
            for (int i = 0; i < f.Ships.Count; i++)
            {
                DrawArmorBar(f.Ships[i], position + new Vector2(40, 16 + 33 * (i + 1)));
                GameOperators.SpriteBatch.Begin();
                if (f.Ships[i] == mouseOnShip||humanSelectedShips.Contains( f.Ships[i]))
                {
                    GameOperators.SpriteBatch.DrawString(sf, ((int)f.Ships[i].Armor).ToString(), position + new Vector2(50, 22 + 33 * (i + 1)), Color.White);
                }
                else GameOperators.SpriteBatch.DrawString(sf, ((int)f.Ships[i].Armor).ToString(), position + new Vector2(50, 22 + 33 * (i + 1)), f.FactionColor);
                GameOperators.SpriteBatch.End();
            }

        }

        /// <summary>
        /// 绘制护甲条
        /// </summary>
        void DrawArmorBar(Ship t,Vector2 position)
        {
            if (t.MaxArmor>0)
            {
                Vector2 screenPoint = CurrentCamera.TransformPoint(t.AbsolutePosition);
                Texture2D tx;
                if (t.Armor / t.MaxArmor > 0.3f)
                {
                    tx = GameOperators.Content.Load<Texture2D>(@"blue");
                }
                else tx = GameOperators.Content.Load<Texture2D>(@"red");
                Color c = Color.White;
                GameOperators.SpriteBatch.Begin();
                GameOperators.SpriteBatch.Draw(GameOperators.Content.Load<Texture2D>(@"empty")
                    , new Rectangle((int)position.X, (int)position.Y, (int)(GameSettings.ArmorBarLength), (int)GameSettings.ArmorBarWidth)
                    , new Rectangle(0, 0, (int)tx.Width, (int)tx.Height)
                    , c
                    , 0
                    , Vector2.Zero
                    , SpriteEffects.None
                    , 0.5f);

                GameOperators.SpriteBatch.Draw(tx
                    , new Rectangle((int)position.X, (int)position.Y, (int)(GameSettings.ArmorBarLength * t.Armor / t.MaxArmor), (int)GameSettings.ArmorBarWidth)
                    , new Rectangle(0,0, (int)(tx.Width * t.Armor / t.MaxArmor), (int)tx.Height)
                    , c
                    , 0
                    , Vector2.Zero
                    , SpriteEffects.None
                    , 0.4f);
                GameOperators.SpriteBatch.End();
            }

        }

        public void AddCannonBall(CannonBall ball)
        {
            cannonBalls.Add(ball);
        }

        public void AddLowerParticle(Particle p)
        {
            lowerParticles.Add(p);
        }

        public void AddUpperParticle(Particle p)
        {
            upperParticles.Add(p);
        }

        public void AddDamageText(float damage, bool critical, Vector2 gamePosition)
        {
            if (critical)
            {
                texts.Add(new FloatText(((int)damage).ToString() + "!", Color.OrangeRed, gamePosition, gamePosition + new Vector2(0, -12), 1.2f, this));
            }
            else texts.Add(new FloatText(((int)damage).ToString(), Color.Orange, gamePosition, gamePosition + new Vector2(0, -10), 1f, this));
        }


        /// <summary>
        /// 检测两个碰撞物件是否碰撞，并得到距离的平方
        /// </summary>
        /// <param name="object1">碰撞物件1</param>
        /// <param name="object2">碰撞物件2</param>
        /// <param name="centerDistanceSquared">距离的平方</param>
        /// <returns></returns>
        public static bool IsShipCollided(IBoundingObject object1, IBoundingObject object2, ref float centerDistanceSquared)
        {
            float d = Vector2.DistanceSquared(object1.AbsolutePosition, object2.AbsolutePosition);
            centerDistanceSquared = d;
            if (d < (float)Math.Pow(object1.BoundingRadius + object2.BoundingRadius,2))
            {
                return true;
            }
            else return false;

        }

        /// <summary>
        /// 检测两个碰撞物件是否碰撞
        /// </summary>
        /// <param name="object1">碰撞物件1</param>
        /// <param name="object2">碰撞物件2</param>
        /// <returns></returns>
        public static bool IsShipCollided(IBoundingObject object1, IBoundingObject object2)
        {
            if (object1 == object2)
            {
                return false;
            }
            else
            {
                float d = Vector2.DistanceSquared(object1.AbsolutePosition, object2.AbsolutePosition);
                if (d < (float)Math.Pow(object1.BoundingRadius + object2.BoundingRadius, 2))
                {
                    return true;
                }
                else return false;

            }

        }
    }
}
