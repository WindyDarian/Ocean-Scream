using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTCOS.Settings
{
    /// <summary>
    /// 设置选项
    /// </summary>
    [Serializable]
    public class SettingData
    {
        /// <summary>
        /// 分辨率X
        /// </summary>
        public int ResolutionX = 1280;

        /// <summary>
        /// 分辨率Y
        /// </summary>
        public int ResolutionY = 720;

        /// <summary>
        /// 开启环境效果
        /// </summary>
        public bool AmbientOn = true;

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP = "127.0.0.1";

        /// <summary>
        /// 端口
        /// </summary>
        public string Port = "21943";
    }
}
