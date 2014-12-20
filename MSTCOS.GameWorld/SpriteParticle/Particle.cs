using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MSTCOS.Base;

namespace MSTCOS.GameWorld.SpriteParticle
{
    /// <summary>
    /// 粒子
    /// </summary>
    public class Particle : Base.IDrawable, Base.IRemovable
    {
        Texture2D texture;
        int textureSectionNum = 1;
        float opancity0 = 0.8f;
        float opancity1 = 0;
        Vector2 position0 = Vector2.Zero;
        Vector2 position1 = Vector2.Zero;
        float scale0 = 0.8f;
        float scale1 = 1.3f;
        float duration = 1f;
        float currentTime = 0;
        bool isBeingRemoved = false;
        World world;
        int spriteWidth;
        Vector2 center;


        public Particle(World world,Texture2D texture, int textureSectionNum, float opancity0, float opancity1, Vector2 position0, Vector2 position1, float scale0
            , float scale1, float duration)
        {
            this.world = world;
            this.texture = texture;
            this.textureSectionNum = textureSectionNum;
            this.opancity0 = opancity0;
            this.opancity1 = opancity1;
            this.scale0 = scale0;
            this.scale1 = scale1;
            this.position0 = position0;
            this.position1 = position1;
            this.duration = duration;
            if (textureSectionNum > 0)
            {
                spriteWidth = texture.Width / textureSectionNum;
            }
            else spriteWidth = texture.Width;
            center = new Vector2(spriteWidth / 2, texture.Height/2);
           
        }

        public void Draw(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime > duration)
            {
                isBeingRemoved = true;
            }
            else
            {
                float value = MathHelper.Clamp( currentTime/duration,0,1);
                 Rectangle r;
                if (textureSectionNum>0)
	            {
		            int currentSection =(int)value*textureSectionNum;
                    r = new Rectangle (spriteWidth*currentSection,0,spriteWidth,texture.Height);
                    center = new Vector2(spriteWidth / 2+ spriteWidth*currentSection, texture.Height / 2);
                }
                else r= new Rectangle (0,0,spriteWidth,texture.Height);
               
               
                GameOperators.SpriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.Additive);

                GameOperators.SpriteBatch.Draw(texture, world.CurrentCamera.TransformPoint(Vector2.SmoothStep(position0, position1, value))
                    , r, Color.White.CrossAlpha(MathHelper.Lerp(opancity0, opancity1, value)), 0
                    , center,MathHelper.SmoothStep(scale0,scale1,value)*world.CurrentCamera.Scale, SpriteEffects.None, 0.2f);
                GameOperators.SpriteBatch.End();
                
            }
        }

        public bool IsBeingRemoved
        {
            get { return isBeingRemoved; }
        }
    }
}
