using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MSTCOS.Base
{
    public static class Extensions
    {
        /// <summary>
        /// 使Color的Alpha值乘上一个浮点数
        /// </summary>
        /// <param name="c"></param>
        /// <param name="opancity"></param>
        /// <returns></returns>
        public static Color CrossAlpha(this Color c, float opancity)
        {
            Color t = c;
            t.A = (byte)(c.A * opancity);
            return t;
        }
    }
}
