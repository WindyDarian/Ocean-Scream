using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
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
    /// 物件管理器
    /// 范若余 2011/11/25
    /// </summary>
    /// <typeparam name="T">要管理的物件类型</typeparam>
    public class ItemManager<T> :IDrawable, IUpdatable, IRemovable,IEnumerable<T>
    {
        /// <summary>
        /// 最后加入的物件
        /// </summary>
        public T LatestAddedItem
        {
            get
            {
                return latestAddedItem;
            }
        }

        /// <summary>
        /// 表示该管理器的物体们的List的拷贝——注意在这个List里增加或移除东西不能被影响到Manager的列表
        /// </summary>
        public List<T> Items
        {
            get
            {
                return new List<T>(items);
            }
        }

        public List<T> MemberList
        {
            get
            {
                return items;
            }
        }

        public bool IsBeingRemoved
        {
            get 
            {
                return isBeingRemoved;
            }
        }
        /// <summary>
        /// 更新所有物件，并添加待添加的物件和移除待移除的物件
        /// </summary>
        /// <param name="gameTime">给予的GameTime参数</param>
        public virtual void Update(GameTime gameTime)
        {
            foreach (var o in addingList)
            {
                items.Add(o);
            }
            addingList.Clear();
            foreach (var o in items)
            {
                if (IsItemRemoving(o))
                {
                    removingList.Add(o);
                    continue;
                }
                if (o is IUpdatable)
                {
                    ((IUpdatable)o).Update(gameTime);//执行物件的更新
                }
            }
            foreach (var o in removingList)
            {
                items.Remove(o);
            }
            removingList.Clear();
        }
        /// <summary>
        /// 绘出其中的所有物件
        /// </summary>
        /// <param name="gameTime">给予的GameTime参数</param>
        public virtual void Draw(GameTime gameTime)
        {
            foreach (T o in items)
            {

                if (!IsItemRemoving(o))
                {
                    if (o is IDrawable)
                    {

                        ((IDrawable)o).Draw(gameTime);
                    }
                }
            }

        }


        /// <summary>
        /// 在下一次Update时安全地加入一个物件
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            addingList.Add(item);
            latestAddedItem = item;
        }


        /// <summary>
        /// 在下一次Update时强制移除一个物件，注意如果是IRemovable请不要用该方法移除，因为其如果标记为IsBeingRemoved会自动移除
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            if (items.Contains(item))
            {
                
                removingList.Add(item);
            }
        }
       

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
      
        #region 私有实现
        private bool isBeingRemoved = false;
        private List<T> items = new List<T>(10);
        private List<T> removingList = new List<T>(10);
        private List<T> addingList = new List<T>(10);
        T latestAddedItem;

        /// <summary>
        /// 得到一个物件是否正在被移除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool IsItemRemoving(T item)
        {
            if (item is IRemovable)
            {
                return ((IRemovable)item).IsBeingRemoved;
            }
            else return false;
        }
        #endregion
    }
}
