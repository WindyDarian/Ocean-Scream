using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MSTCOS.Base
{
    /// <summary>
    /// 可更新物件的接口
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">获取Game传递的GameTime</param>
        void Update(GameTime gameTime);

    }
}
