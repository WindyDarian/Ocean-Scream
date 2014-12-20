using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSTCOS.Base
{
    /// <summary>
    /// 可移除物件的接口，管理其的父物件检测到IsBeingRemoved能将其移除
    /// </summary>
    public interface IRemovable
    {
        /// <summary>
        /// 由父物件得到是否生命期已结束需要移除
        /// </summary>
        bool IsBeingRemoved { get; }
    }
}
