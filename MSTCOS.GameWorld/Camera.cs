using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MSTCOS.Base;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// 相机类
    /// </summary>
    public class Camera : MSTCOS.Base.IUpdatable
    {
        /// <summary>
        /// 中心坐标
        /// </summary>
        public Vector2 Center = Vector2.Zero;
        
        
        float scale = 2f;
        /// <summary>
        /// 缩放
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set { scale = MathHelper.Clamp(value,0.5f,100f); }
        }

        /// <summary>
        /// 向量的转换
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector2 TransformNormal(Vector2 v)
        {
            return v * scale;
        }

        /// <summary>
        /// 将游戏坐标的点转换为屏幕坐标的点
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Vector2 TransformPoint(Vector2 p)
        {
            Vector2  screenCenter= new Vector2(GameOperators.GraphicsDevice.Viewport.Width / 2f,
                GameOperators.GraphicsDevice.Viewport.Height / 2f);

            //平移向量
            Vector2 translation = screenCenter; //- Center * scale;

            return (p - Center) * scale + translation;
        }

        /// <summary>
        /// 将屏幕坐标转化为游戏坐标
        /// </summary>
        /// <param name="screenPoint">屏幕坐标</param>
        /// <returns></returns>
        public Vector2 ReversePoint(Vector2 screenPoint)
        {
            Vector2 screenCenter = new Vector2(GameOperators.GraphicsDevice.Viewport.Width / 2f,
    GameOperators.GraphicsDevice.Viewport.Height / 2f);

            return (screenPoint - screenCenter) / scale + Center;
        }

        public Camera(Vector2 position, float scale)
        {
            this.Center = position;
            this.scale = scale;
        }

        public void Update(GameTime gameTime)
        {
          
        }
    }
}
