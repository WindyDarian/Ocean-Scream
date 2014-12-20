using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MSTCOS.Base
{
    /// <summary>
    /// 可绘出物件的接口
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// 绘出
        /// </summary>
        /// <param name="gameTime">获取Game传递的GameTime</param>
        void Draw(GameTime gameTime);
    }
}
