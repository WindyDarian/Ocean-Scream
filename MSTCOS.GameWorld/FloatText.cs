using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MSTCOS.Base;

namespace MSTCOS.GameWorld
{
    public class FloatText:Base.IUpdatable,Base.IDrawable,Base.IRemovable
    {
        World world;

        string text;
        /// <summary>
        /// 文字
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
       
       
        float rate = 0.25f;
        /// <summary>
        /// 插值速率
        /// </summary>
        public float Rate
        {
            get { return rate; }
            set { rate = value; }
        }

     
        float appearValue = 0.25f;
        /// <summary>
        /// 到完全出现所用的插值
        /// </summary>
        public float AppearValue
        {
            get { return appearValue; }
            set { appearValue = value; }
        }


        float disappearValue = 0.75f;
        /// <summary>
        /// 开始消失的插值
        /// </summary>
        public float DisappearValue
        {
            get { return disappearValue; }
            set { disappearValue = value; }
        }

        Color color;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        
        Vector2 position0;
        Vector2 position1;

        float scale;
        
        float currentValue = 0.0f;// 当前插值
        bool isBeingRemoved = false;

        SpriteFont sf;


        public FloatText(string text,Color color,float rate, float appearValue, float disppearValue,Vector2 position0,Vector2 position1,float scale,World world)
        {
            this.text = text;
            this.color = color;
            this.Rate = rate;
            this.AppearValue = appearValue;
            this.DisappearValue = disppearValue;
            this.position0 = position0;
            this.position1 = position1;
            this.scale = scale;
            this.world = world;
        }

        public FloatText(string text, Color color, Vector2 position0, Vector2 position1, float scale,World world)
        {
            this.text = text;
            this.color = color;
            this.Rate = 0.8f;
            this.AppearValue = 0.5f;
            this.DisappearValue = 0.8f;
            this.position0 = position0;
            this.position1 = position1;
            this.scale = scale;
            this.world = world;
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentValue = MathHelper.Clamp(currentValue + elapsedTime * Rate, 0, 1);



            if (currentValue >= 1)
            {

                isBeingRemoved = true;

            }

        }

        public virtual void Draw(GameTime gameTime)
        {
           
                Draw(currentValue, AppearValue, DisappearValue);
            

        }

        void Draw(float currentValue, float appearValue, float disappearValue)
        {
            Vector2 position = world.CurrentCamera.TransformPoint( Vector2.Lerp(position0, position1, currentValue));
            if (sf == null)
            {
                sf = GameOperators.Content.Load<SpriteFont>("defaultFont");
            }

            Vector2 center = sf.MeasureString(Text) / 2;


            string currentString = Text;

            float opancity = 1;

            if (currentValue < appearValue && appearValue > 0) opancity = currentValue / appearValue;
            else if (currentValue > disappearValue && disappearValue < 1) opancity = (1 - currentValue) / (1 - disappearValue);


            GameOperators.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            //GameOperators.SpriteBatch.DrawString(sf, currentString, position + new Vector2(2f, 2f), Color.Gray.CrossAlpha(opancity * 0.75f), 0, center, scale, SpriteEffects.None, 0.6f);

            GameOperators.SpriteBatch.DrawString(sf, currentString, position, Color.CrossAlpha(opancity), 0, center, scale, SpriteEffects.None, 0.5f);
            GameOperators.SpriteBatch.End();

        }


        public void Remove()
        {
            isBeingRemoved = true;
        }

        public bool IsBeingRemoved
        {
            get { return isBeingRemoved; }
        }

    }
}
