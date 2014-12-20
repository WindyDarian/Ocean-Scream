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
    /// 游戏的操作器们！
    /// 郭哲聪2011/11/26创建
    /// 范若余2011/11/26润色
    /// </summary>
    public static class GameOperators
    {
        private static ContentManager content;
        private static SoundManager soundManager;
        private static SpriteBatch spriteBatch;
        private static GraphicsDevice graphicsDevice;
        private static Random random = new Random();
        private static PrimitiveBatch primitiveBatch;



        /// <summary>
        /// 返回游戏所使用的ContentManager，或在游戏初始化时为其赋值
        /// 若已经赋过值则不能再次给其赋值
        /// </summary>
        public static ContentManager Content
        {
            get 
            {
                if (content != null)
                {
                    return content;
                }
                else throw new ApplicationException("未定义Content");
            }
            set
            {
                if (content == null)
                {
                    content = value;
                }
                else throw new ApplicationException("已定义Content，不能重复赋值！");
            }
        }



        /// <summary>
        /// 返回游戏所使用的SoundManager，或在游戏初始化时为其赋值
        /// 若已经赋过值则不能再次给其赋值
        /// </summary>
        public static SoundManager SoundManager
        {
            get
            {
                if (soundManager != null)
                {
                    return soundManager;
                }
                else throw new ApplicationException("未定义SoundManager");
            }
            set
            {
                if (soundManager == null)
                {
                    soundManager = value;
                }
                else throw new ApplicationException("已定义SoundManager，不能重复赋值！");
            }
        }

        /// <summary>
        /// 返回游戏所使用的SpriteBatch，或在游戏初始化时为其赋值
        /// 若已经赋过值则不能再次给其赋值
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get
            {
                if (spriteBatch != null)
                {
                    return spriteBatch;
                }
                else throw new ApplicationException("未定义SpriteBatch");
            }
            set
            {
                if (spriteBatch == null)
                {
                    spriteBatch = value;
                }
                else throw new ApplicationException("已定义SpriteBatch，不能重复赋值！");
            }
        }

        /// <summary>
        /// 返回游戏所使用的PrimitiveBatch，或在游戏初始化时为其赋值
        /// 若已经赋过值则不能再次给其赋值
        /// </summary>
        public static PrimitiveBatch PrimitiveBatch
        {
            get
            {
                if (primitiveBatch != null)
                {
                    return primitiveBatch;
                }
                else throw new ApplicationException("未定义SpriteBatch");
            }
            set
            {
                if (primitiveBatch == null)
                {
                    primitiveBatch = value;
                }
                else throw new ApplicationException("已定义PrimitiveBatch，不能重复赋值！");
            }
        }

        /// <summary>
        /// 返回游戏所使用的GraphicsDevice，或在游戏初始化时为其赋值
        /// 若已经赋过值则不能再次给其赋值
        /// </summary>
        public static GraphicsDevice GraphicsDevice
        {
            get
            {
                if (graphicsDevice != null)
                {

                    return GameOperators.graphicsDevice;
                }
                else throw new ApplicationException("未定义GraphicsDevice");
            }
            set
            {
                if (graphicsDevice == null)
                {
                    GameOperators.graphicsDevice = value;
                }
                else throw new ApplicationException("已定义GraphicsDevice，不能重复赋值！");
            }
        }

        /// <summary>
        /// 随机数生成器
        /// </summary>
        public static Random Random
        {
            get { return GameOperators.random; }
        }
    }
}
