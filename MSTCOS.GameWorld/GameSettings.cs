using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTCOS.GameWorld
{
    public static class GameSettings
    {
        /// <summary>
        /// 船只在MoveTo命令时停止的半径
        /// </summary>
        public static float StopRadius = 25f;
        /// <summary>
        /// 护甲条时刻存在
        /// </summary>
        public static bool IsArmorBarOn = false;
        /// <summary>
        /// 血条长度
        /// </summary>
        public static float ArmorBarLength = 120f;
        /// <summary>
        /// 血条宽度
        /// </summary>
        public static float ArmorBarWidth = 16f;
        /// <summary>
        /// 血条高度
        /// </summary>
        public static float ArmorBarHeight = 36f;
        

    }
}
