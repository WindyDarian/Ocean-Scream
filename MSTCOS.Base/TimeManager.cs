using System;
using System.Collections.Generic;
using System.Linq;
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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TimeManager : IUpdatable
    {
        private int minute;
        private int second;
        private int milisecond;

        private int fullSecond;
        private int fullMilisecond = 5 * 60 * 1000;

        private bool timeState = false;

        /// <summary>
        /// 返回游戏所使用的minute
        /// </summary>
        public int Minute
        {
            get { return minute; }
            //set { minute = value; }
        }
        public void Start()
        {
            timeState = true;
        }

        public void Stop()
        {
            timeState = false;
        }
        /// <summary>
        /// 返回游戏所使用的second
        /// </summary>
        public int Second
        {
            get { return second; }
            //set { second = value; }
        }

        /// <summary>
        /// 返回游戏所使用的milisecond
        /// </summary>
        public int Milisecond
        {
            get { return milisecond; }
            //set { milisecond = value; }
        }

        /// <summary>
        /// 返回游戏所使用的fullSecond
        /// </summary>
        public int FullSecond
        {
            get { return fullSecond; }
            //set { fullSecond = value; }
        }

        /// <summary>
        /// 返回游戏所使用的fullMilisecond
        /// </summary>
        public int FullMilisecond
        {
            get { return fullMilisecond; }
            //set { fullMilisecond = value; }
        }

        /// <summary>
        /// 获取是否更新时间的布尔值
        /// </summary>
        public bool TimeState
        {
            get { return timeState; }
            set { timeState = value; }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (timeState)
            {
                fullMilisecond -= gameTime.ElapsedGameTime.Milliseconds;
            }
            if (fullMilisecond < 0)
            {
                fullMilisecond = 0;
                milisecond = 0;
                fullSecond = 0;
                second = 0;
                minute = 0;
            }
            else
            {
                milisecond = fullMilisecond % 1000;
                fullSecond = fullMilisecond / 1000;
                second = fullSecond % 60;
                minute = fullSecond / 60;
            }
        }

        /// <summary>
        /// 生成两位数的时间字符串
        /// </summary>
        /// <param name="timeint">时间</param>
        /// <returns>字符串</returns>
        public string TimetoString(int timeint)
        {
            if (timeint < 10)
                return "0" + System.Convert.ToString(timeint);//为个位数前加0
            else
                return System.Convert.ToString(timeint);
        }
        
        /// <summary>
        /// 生成时间显示文本
        /// </summary>
        /// <returns>返回记录时间的字符串</returns>
        public string getTimeStringforDisplay()
        {
            return System.Convert.ToString(minute) + ":" + TimetoString(second);
        }

        /// <summary>
        /// 生成游戏视频记录时间
        /// </summary>
        /// <returns>返回记录时间的字符串</returns>
        public string getTimeStringforRecord()
        {
            return System.Convert.ToString(fullMilisecond);
        }
    }
}