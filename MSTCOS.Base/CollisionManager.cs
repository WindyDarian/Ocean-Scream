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

namespace MSTCOS.Base
{
    /// <summary>
    /// 碰撞检测
    /// 黎健成2011/11/26
    /// </summary>
    public static class CollisionManager
    {
        public static bool IsCollided(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }
    }
}
