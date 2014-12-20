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
    /// 炮台
    /// </summary>
    public class Cannon:GameObject
    {
        
        float basicDamage = 200f;
        /// <summary>
        /// 基础伤害
        /// </summary>
        public float BasicDamage
        {
            get { return basicDamage; }
            set { basicDamage = MathHelper.Clamp(value, 0, float.MaxValue); }
        }

        float basicBulletSpeed = 150f;
        /// <summary>
        /// 弹药速度
        /// </summary>
        public float BasicBulletSpeed
        {
            get { return basicBulletSpeed; }
            set { basicBulletSpeed = MathHelper.Clamp(value, 0.01f, float.MaxValue); }
        }

        float rangeSquared = 122500f;
        /// <summary>
        /// 射程的平方
        /// </summary>
        public float RangeSquared
        {
            get { return rangeSquared; }
            set { rangeSquared = (float)Math.Pow(MathHelper.Clamp(value,0,float.MaxValue),2); }
        }

        float criticalAngle = 60f;
        /// <summary>
        /// 暴击扇形夹角
        /// </summary>
        public float CriticalAngle
        {
            get { return criticalAngle; }
            set { criticalAngle = value; }
        }


        /// <summary>
        /// 射程
        /// </summary>
        public float Range
        {
            get { return (float)Math.Sqrt(rangeSquared); }
            set { rangeSquared = (float)Math.Pow(MathHelper.Clamp(value,0,float.MaxValue), 2); }
        }

        float maxCooldown = 4f;

        float cooldownRemain = 0;
        public float CooldownRemain
        {
            get { return cooldownRemain; }
        }

        float maxRadianAngle = MathHelper.PiOver4;

        List<Vector2> firePoints = new List<Vector2>(4);

        public float MaxAngle
        {
            get { return MathHelper.ToDegrees(maxRadianAngle); }
            set { maxRadianAngle = MathHelper.Clamp(MathHelper.ToRadians(value), 0, MathHelper.TwoPi); }
        }

        /// <summary>
        /// 定义一个炮台，因为炮台已经画在船身上所以没有纹理。
        /// </summary>
        /// <param name="world"></param>
        /// <param name="ship"></param>
        public Cannon(World world, Ship owner,float rotation)
            : base(world)
        {
            ParentObject = owner;
            MaxAngle = 90f;
            basicDamage = 200f;
            rangeSquared =  122500f;
;
            base.Rotation = rotation;
            firePoints.Add(new Vector2(-96,-60));
            firePoints.Add(new Vector2(-7,-52));
            firePoints.Add(new Vector2(49,-52));
            firePoints.Add(new Vector2(137,-60));
        }

        public override void Update(GameTime gameTime)
        {
            if (cooldownRemain>0)
            {
                cooldownRemain -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gameTime);
        }

        public void FireAt(Ship target)
        {
            if (IsTargetInRange(target) && cooldownRemain <= 0)
            {

                bool isCritical = false;

                float p1 = (float)Math.Sqrt(MathHelper.Clamp(1 - Vector2.DistanceSquared(target.AbsolutePosition, AbsolutePosition) / rangeSquared, 0, 1));
                float p2 = 1;
                if (ParentObject is Ship)
                {
                    p2 = 1 - MathHelper.Clamp((float)Math.Abs(Vector2.Dot(ParentObject.AbsoluteDirection, ((Ship)ParentObject).Velocity - target.Velocity)) / 100, 0, 1);
                }
                float dmg = basicDamage * p1 * p2;
                Vector2 pd = target.AbsolutePosition - AbsolutePosition;
                if (pd != Vector2.Zero) pd.Normalize();
                if (Math.Abs(Vector2.Dot(target.AbsoluteDirection, pd)) > Math.Cos(MathHelper.ToRadians(criticalAngle / 2)))
                {
                    isCritical = true;
                    dmg *= 2f;
                }

                target.ReceiveDamage(dmg, Vector2.Distance(AbsolutePosition, ((GameObject)target).AbsolutePosition) / basicBulletSpeed, isCritical);
                if (ParentObject is Ship)
                {
                    ((Ship)ParentObject).Faction.TotalDamage += dmg;
                    ((Ship)ParentObject).Target = target;
                }
                cooldownRemain += maxCooldown;

                foreach (var p in firePoints)
                {
                    Vector2 tp;
                    if (target is Ship)
                    {
                        tp = new Vector2(((Ship)target).AbsolutePosition.X - (float)(MSTCOS.Base.GameOperators.Random.NextDouble() - 0.5) * ((Ship)target).BoundingRadius * 2
                            , ((Ship)target).AbsolutePosition.Y - (float)(MSTCOS.Base.GameOperators.Random.NextDouble() - 0.5) * ((Ship)target).BoundingRadius * 2);
                    }
                    else tp = ((GameObject)target).AbsolutePosition;
                    Vector2 op = Vector2.TransformNormal(p, Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), AbsoluteRadianRotation)) * ParentObject.Scale + AbsolutePosition;
                    World.AddCannonBall(new CannonBall(World,op , tp, @"CannonBall", 0.02f, basicBulletSpeed));
                    World.AddUpperParticle(new SpriteParticle.Particle(World, Base.GameOperators.Content.Load<Texture2D>("explosion"), 7, 0.7f, 0f, op, Vector2.Lerp(op, tp, 0.1f), 0f, 0.1f, 1));
                }
                Base.SoundManager.Play3DSound(Base.GameOperators.Content.Load<SoundEffect>(@"Audio\Cannon"), AbsolutePosition, World.CurrentCamera.Center, World.CurrentCamera.Scale);
            }
        }

        public bool IsTargetInRange(GameObject target)
        {

            Vector2 t;
            if ((target).AbsolutePosition != AbsolutePosition)
            {
                t = Vector2.Normalize((target).AbsolutePosition - AbsolutePosition);
            }
            else t = Vector2.Zero;
            bool p = Vector2.DistanceSquared((target).AbsolutePosition, AbsolutePosition) < rangeSquared; //在射程范围内
            bool q = Math.Acos(Vector2.Dot(t, AbsoluteDirection)) < maxRadianAngle / 2;//在射程角度内
            return (p && q);

        }
    }
}
