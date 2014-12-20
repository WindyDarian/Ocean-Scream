using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTCOS.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// 2D的游戏物件
    /// </summary>
    public class GameObject : MSTCOS.Base.IUpdatable, MSTCOS.Base.IDrawable , MSTCOS.Base.IRemovable
    {
        /// <summary>
        /// 位置（若有父物件则是相对位置）
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// 获得其绝对位置
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get
            {
                if (parentObject == null)//如果没有父物件则是其Position
                {
                    return Position;
                }
                else return ParentObject.AbsolutePosition + ParentObject.Scale * Vector2.TransformNormal(Position, Matrix.CreateFromAxisAngle(new Vector3(0,0,1), ParentObject.RadianRotation));
            }
        }

        private float radianRotation = 0f;
        /// <summary>
        /// 角度制的旋转角（角度），顺时针为正
        /// </summary>
        public float Rotation
        {
            get { return MathHelper.ToDegrees(radianRotation); }
            set { radianRotation = MathHelper.WrapAngle(MathHelper.ToRadians(value)); }
        }


        /// <summary>
        /// 角度制的旋转角（弧度），顺时针为正
        /// </summary>
        public float RadianRotation
        {
            get { return radianRotation; }
            set { radianRotation = MathHelper.WrapAngle(value); }
        }

        public float AbsoluteRadianRotation
        {
            get
            {
                if (parentObject == null)
                {
                    return radianRotation;
                }
                else return parentObject.AbsoluteRadianRotation + radianRotation;
            }
        }


        private ItemManager<GameObject> childItems = new ItemManager<GameObject>();
        /// <summary>
        /// 子物件，如船的桅杆之类的
        /// </summary>
        public ItemManager<GameObject> ChildItems
        {
            get { return childItems; }
        }

        private GameObject parentObject;
        /// <summary>
        /// 父物件
        /// </summary>
        public GameObject ParentObject
        {
            get { return parentObject; }
            set { parentObject = value; }
        }

        private World world;
        /// <summary>
        /// 所属游戏世界
        /// </summary>
        protected World World
        {
            get { return world; }
            set { world = value; }
        }

        /// <summary>
        /// 绘出时颜色
        /// </summary>
        public Color Color = Color.White;

        private float scale = 1.0f;
        /// <summary>
        /// 缩放
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set { scale = MathHelper.Clamp(value, 0.0001f, 100f); }
        }
        
        private string texture = "";
        /// <summary>
        /// 纹理地址
        /// </summary>
        public string Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                textureLoaded = false;
            }
        }

        BlendState blendState;

        public BlendState BlendState
        {
            get { return blendState; }
            set { blendState = value; }
        }


        /// <summary>
        /// 朝向
        /// </summary>
        public Vector2 Direction
        {
            get { return (Vector2.Transform(new Vector2(0, -1), Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), RadianRotation))); }
        }

        /// <summary>
        /// 绝对朝向
        /// </summary>
        public Vector2 AbsoluteDirection
        {
            get
            {
                if (ParentObject == null)
                {
                    return Direction;
                }
                else return (Vector2.Transform(new Vector2(0, -1), Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), ParentObject.RadianRotation + RadianRotation)));
            }
        }



        public GameObject(World world)
        {
            this.world = world;
        }


        public virtual void Update(GameTime gameTime)
        {
            childItems.Update(gameTime);
        }


        public virtual void Draw(GameTime gameTime)
        {
            //载入纹理

            if (texture != "")
            {
                if (!textureLoaded || tex == null)
                {
                    tex = GameOperators.Content.Load<Texture2D>(texture);
                    textureLoaded = true;
                }
            }

            //绘制

            if (tex != null)
            {
                float s = scale * world.CurrentCamera.Scale;//缩放
                if (parentObject != null) s *= parentObject.scale;
                if (blendState != null) GameOperators.SpriteBatch.Begin(SpriteSortMode.BackToFront, blendState);
                else GameOperators.SpriteBatch.Begin();
                GameOperators.SpriteBatch.Draw(tex
                    , world.CurrentCamera.TransformPoint(AbsolutePosition)
                    , null
                    , Color
                    , AbsoluteRadianRotation
                    , new Vector2(tex.Width / 2, tex.Height / 2)
                    , s
                    , SpriteEffects.None
                    , 0.5f);
                GameOperators.SpriteBatch.End();
            }


            childItems.Draw(gameTime);

        }

        // 载入之后的纹理
        Texture2D tex;
        /// <summary>
        /// 是否已载入纹理
        /// </summary>
        bool textureLoaded = false;


        bool isBeingRemoved;
        /// <summary>
        /// 该物件是否会被ItemManager移除
        /// </summary>
        public bool IsBeingRemoved
        {
            get
            {
                return isBeingRemoved;
            }
            set
            {
                if (isBeingRemoved && value == false)
                {
                    return;
                    //无法将标记为需移除的物件取消
                }
                else
                {
                    isBeingRemoved = value;
                }

            }
        }

        public virtual void BeginToDie()
        {
            isBeingRemoved = true;
            
        }
    }
}
