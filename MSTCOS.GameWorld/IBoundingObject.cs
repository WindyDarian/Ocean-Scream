using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// 碰撞物件的接口
    /// </summary>
    public interface IBoundingObject
    {
        /// <summary>
        /// 获得其绝对位置
        /// </summary>
        Vector2 AbsolutePosition
        {
            get;
        }
        /// <summary>
        /// 碰撞半径
        /// </summary>
        float BoundingRadius
        {
            get;
        }

    }
}
