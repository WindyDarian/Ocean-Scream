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
        /// ������Ϸ��ʹ�õ�minute
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
        /// ������Ϸ��ʹ�õ�second
        /// </summary>
        public int Second
        {
            get { return second; }
            //set { second = value; }
        }

        /// <summary>
        /// ������Ϸ��ʹ�õ�milisecond
        /// </summary>
        public int Milisecond
        {
            get { return milisecond; }
            //set { milisecond = value; }
        }

        /// <summary>
        /// ������Ϸ��ʹ�õ�fullSecond
        /// </summary>
        public int FullSecond
        {
            get { return fullSecond; }
            //set { fullSecond = value; }
        }

        /// <summary>
        /// ������Ϸ��ʹ�õ�fullMilisecond
        /// </summary>
        public int FullMilisecond
        {
            get { return fullMilisecond; }
            //set { fullMilisecond = value; }
        }

        /// <summary>
        /// ��ȡ�Ƿ����ʱ��Ĳ���ֵ
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
        /// ������λ����ʱ���ַ���
        /// </summary>
        /// <param name="timeint">ʱ��</param>
        /// <returns>�ַ���</returns>
        public string TimetoString(int timeint)
        {
            if (timeint < 10)
                return "0" + System.Convert.ToString(timeint);//Ϊ��λ��ǰ��0
            else
                return System.Convert.ToString(timeint);
        }
        
        /// <summary>
        /// ����ʱ����ʾ�ı�
        /// </summary>
        /// <returns>���ؼ�¼ʱ����ַ���</returns>
        public string getTimeStringforDisplay()
        {
            return System.Convert.ToString(minute) + ":" + TimetoString(second);
        }

        /// <summary>
        /// ������Ϸ��Ƶ��¼ʱ��
        /// </summary>
        /// <returns>���ؼ�¼ʱ����ַ���</returns>
        public string getTimeStringforRecord()
        {
            return System.Convert.ToString(fullMilisecond);
        }
    }
}