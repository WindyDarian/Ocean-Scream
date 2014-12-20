using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// 炮台，仅动画效果
    /// </summary>
    public class CannonBall:GameObject
    {
        Vector2 originPoint;
        Vector2 targetPoint;
        float rate = 0;//插值速度
        float currentvalue = 0;//当前插值
        
        public CannonBall(World world,Vector2 originPoint,Vector2 targetPoint,string texture,float scale,float speed)
            : base(world)
        {
            this.originPoint = originPoint;
            this.targetPoint = targetPoint;
            this.Texture = texture;
            this.Scale = scale;
            if (originPoint != targetPoint)
            {
                this.rate = speed / Vector2.Distance(targetPoint, originPoint);
            }
            else this.IsBeingRemoved = true;

        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentvalue += elapsedTime * rate;

            if (Position == targetPoint)
            {
                World.AddUpperParticle(new SpriteParticle.Particle(World, Base.GameOperators.Content.Load<Texture2D>("explosion"), 7, 0.9f, 0f, AbsolutePosition, AbsolutePosition, 0f, 0.4f, 1));
                //Base.SoundManager.Play3DSound(Base.GameOperators.Content.Load<SoundEffect>(@"Audio\Explosion"), AbsolutePosition, World.CurrentCamera.Center, World.CurrentCamera.Scale);
                this.IsBeingRemoved = true;
            }
            else
            {
                Position = Vector2.Lerp(originPoint, targetPoint, MathHelper.Clamp(currentvalue, 0, 1));
            }
            base.Update(gameTime);
        }
    }
}
