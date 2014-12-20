using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTCOS.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MSTCOS.GameWorld
{
    class SmallMap :MSTCOS.Base.IDrawable
    {
        /// <summary>
        /// 船只
        /// </summary>
        ItemManager<Ship> Ships;
        /// <summary>
        /// 小地图大小
        /// </summary>
        Vector2 mapSize;
        /// <summary>
        /// 绘制小地图的位置
        /// </summary>
        Vector2 position = new Vector2(-10,-10);
        /// <summary>
        /// 小地图背景图片
        /// </summary>
        Texture2D Texture;
        /// <summary>
        /// 小地图当前视野
        /// </summary>
        Texture2D border;
        /// <summary>
        /// 当前摄像机
        /// </summary>
        Camera currentCamera;
        /// <summary>
        /// 小地图缩放比例
        /// </summary>
        float scale = 0.1f;
        /// <summary>
        /// 资源点
        /// </summary>
        ItemManager<ResourceArea> resources;

        public SmallMap(ItemManager<Ship> Ships,ItemManager<ResourceArea> resources , Camera currentCamera)
        {
            this.Ships = Ships;
            mapSize = new Vector2(205, 205);
            Texture = GameOperators.Content.Load<Texture2D>(@"smallMapFrame");
            border = GameOperators.Content.Load<Texture2D>(@"border");
            this.currentCamera = currentCamera;
            this.resources = resources;
        }
        /// <summary>
        /// 从真实的位置转换到小地图的位置
        /// </summary>
        /// <param name="realMapPosition"></param>
        /// <returns></returns>
        private Vector2 TransformRtoS(Vector2 realMapPosition)
        {
            Vector2 s = new Vector2(scale * realMapPosition.X + mapSize.X / 2, scale * realMapPosition.Y  + mapSize.Y / 2);
            return s;
        }
        /// <summary>
        /// 从小地图位置转换到真实位置
        /// </summary>
        /// <param name="smallMapPosition"></param>
        /// <returns></returns>
        private Vector2 TransformStoR(Vector2 smallMapPosition)
        {
            return new Vector2((smallMapPosition.X - mapSize.X / 2) / scale, (smallMapPosition.Y - mapSize.Y / 2) / scale);
        }
        /// <summary>
        /// 获得视野框的绘制位置
        /// </summary>
        /// <returns></returns>
        private Vector2 GetPosition()
        {
            Vector2 temp = TransformRtoS(currentCamera.Center);
            Vector2 temp2 = new Vector2(0, 0);
            if (temp.X - border.Width / 2 < 0)
            {
                temp2.X = 0;
            }
            else if (temp.X > mapSize.X - border.Width/2)
            {
                temp2.X = mapSize.X - border.Width;
            }
            else
            {
                temp2.X = temp.X - border.Width / 2;
            }
            if (temp.Y - border.Height / 2 < 0)
            {
                temp2.Y = 0; 
            }
            else if (temp.Y > mapSize.Y - border.Height/2)
            {
                temp2.Y = mapSize.Y - border.Height;
            }
            else
            {
                temp2.Y = temp.Y - border.Height / 2;
            }
            return temp2;
        }

        public void Update(GameTime gameTime)
        {
            if (MSTCOS.Base.InputState.CurrentMousePosition.X > 0 && MSTCOS.Base.InputState.CurrentMousePosition.X < mapSize.X &&
                    MSTCOS.Base.InputState.CurrentMousePosition.Y > 0 && MSTCOS.Base.InputState.CurrentMousePosition.Y < mapSize.Y)
            {
                if (InputState.IsMouseButtonPressed(MouseButton.LeftButton))
                {
                    currentCamera.Center = TransformStoR(MSTCOS.Base.InputState.CurrentMousePosition);
                }
            }
        }

        public  void Draw(GameTime gameTime)
        {
            GameOperators.SpriteBatch.Begin();
            GameOperators.SpriteBatch.Draw(Texture, this.position, null, Color.White, 0, Vector2.Zero, 0.45f, SpriteEffects.None, 0);
            GameOperators.SpriteBatch.End();

            foreach (ResourceArea re in resources)
            {
                if (re.ControllingFaction == null)
                {
                    GameOperators.SpriteBatch.Begin();
                    GameOperators.SpriteBatch.Draw(GameOperators.Content.Load<Texture2D>(@"Resouse"), TransformRtoS(re.Position), null, Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.FlipHorizontally, 0);
                    GameOperators.SpriteBatch.End();
                }
                else
                {
                    GameOperators.SpriteBatch.Begin();
                    GameOperators.SpriteBatch.Draw(GameOperators.Content.Load<Texture2D>(@"Resouse"), TransformRtoS(re.Position), null, re.ControllingFaction.FactionColor, 0, Vector2.Zero, 0.2f, SpriteEffects.FlipHorizontally, 0);
                    GameOperators.SpriteBatch.End();
                }
            }

            foreach (Ship smallShip in Ships)
            {

                GameOperators.SpriteBatch.Begin();
                GameOperators.SpriteBatch.Draw(GameOperators.Content.Load<Texture2D>(@"Resouse"), TransformRtoS(smallShip.Position), null, smallShip.Faction.FactionColor.CrossAlpha(0.9f), 0, Vector2.Zero, 0.1f, SpriteEffects.FlipHorizontally, 0);
                GameOperators.SpriteBatch.End();
            }
            GameOperators.SpriteBatch.Begin();
            GameOperators.SpriteBatch.Draw(border, GetPosition(), null, Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0);
            GameOperators.SpriteBatch.End();
        }
    }
}
