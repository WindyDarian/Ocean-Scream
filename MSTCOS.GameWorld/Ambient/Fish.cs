using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MSTCOS.GameWorld.Ambient
{
    /// <summary>
    /// 鱼，和船有大量重复代码所以待提取接口
    /// </summary>
    public class Fish:AmbientObject
    {
        float acceleration = 25f;
        /// <summary>
        /// 加速度大小
        /// </summary>
        public float Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        float maxSpeed = 100f;
        /// <summary>
        /// 最大速率
        /// </summary>
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        float currentSpeed = 0f;
        /// <summary>
        /// 当前速率
        /// </summary>
        public float CurrentSpeed
        {
            get { return currentSpeed; }
        }

        public Vector2 Velocity
        {
            get { return currentSpeed * Direction; }
        }

        float angularRate = 0f;
        /// <summary>
        /// 每秒旋转角度
        /// </summary>
        public float AngularRate
        {
            get { return angularRate; }
            set { angularRate = MathHelper.Clamp(value, 0, float.MaxValue); }
        }

        bool isMoving = false;
        /// <summary>
        /// 是否正在执行向前移动的指令
        /// </summary>
        public bool IsMoving
        {
            get { return isMoving; }
        }

        bool isBlocked = false;
        /// <summary>
        /// 是否被挡住了
        /// </summary>
        public bool IsBlocked
        {
            get { return isBlocked; }
        }

        private bool isStoppingAtTarget = false;
        /// <summary>
        /// 是否在到达目标时停下来，任何额外的旋转指令都会取消该状态。
        /// </summary>
        public bool IsStoppingAtTarget
        {
            get { return isStoppingAtTarget; }
        }

        bool isRotating = false;
        /// <summary>
        /// 是否正在执行朝目标点旋转的指令，不能和朝目标角量旋转的指令共存
        /// </summary>
        public bool IsRotating
        {
            get { return isRotating; }
        }

        bool isRotatingToAngle = false;
        /// <summary>
        /// 是否正在执行朝目标角量旋转的指令，不能和朝目标点旋转的指令共存
        /// </summary>
        public bool IsRotatingToAngle
        {
            get { return isRotatingToAngle; }
        }

        Vector2 targetPoint = Vector2.Zero;
        /// <summary>
        /// 旋转目标点或移动停止点
        /// </summary>
        public Vector2 TargetPoint
        {
            get { return targetPoint; }
        }

        float targetAngle = 0;
        /// <summary>
        /// 旋转目标角量
        /// </summary>
        public float TargetAngle
        {
            get { return targetAngle; }
        }


        Vector2 positionl = Vector2.Zero;
        /// <summary>
        /// 上一帧的位置
        /// </summary>
        public Vector2 Positionl
        {
            get { return positionl; }
            set { positionl = value; }
        }

        public Fish(World world)
            : base(world, new Vector2(world.MapSize.X * ((float)Base.GameOperators.Random.NextDouble() - 0.5f), world.MapSize.Y * ((float)Base.GameOperators.Random.NextDouble() - 0.5f))
            , "fish", (1 + ((float)Base.GameOperators.Random.NextDouble() - 0.5f) * 0.4f) * 0.05f, Color.White, MathHelper.TwoPi * (float)Base.GameOperators.Random.NextDouble())
        {
      
            positionl = Position;

            maxSpeed = 36f;
            acceleration = 10f;
            angularRate = 5f;

        }

        /// <summary>
        /// 停止移动和旋转
        /// </summary>
        public void Stop()
        {
            StopMoving();
            StopRotating();
        }

        /// <summary>
        /// 停止移动
        /// </summary>
        public void StopMoving()
        {
            isStoppingAtTarget = false;
            isMoving = false;
        }

        /// <summary>
        /// 停止旋转
        /// </summary>
        public void StopRotating()
        {
            isRotating = false;
            isRotatingToAngle = false;
        }

        /// <summary>
        /// 开始向前移动，忽略MoveTo的到达目标附近时停止移动的命令
        /// </summary>
        public void StartMoving()
        {
            isMoving = true;
            isStoppingAtTarget = false;
        }

        /// <summary>
        /// 开始朝目标点旋转
        /// </summary>
        /// <param name="target">旋转的目标点</param>
        public void StartRotating(Vector2 target)
        {
            targetPoint = target;
            isRotating = true;
            isStoppingAtTarget = false;
            isRotatingToAngle = false;
        }

        /// <summary>
        /// 开始朝目标角量旋转
        /// </summary>
        /// <param name="target">旋转的目标角量</param>
        public void StartRotating(float target)
        {
            targetAngle = target;
            isRotating = false;
            isRotatingToAngle = true;
            isStoppingAtTarget = false;
        }

        /// <summary>
        /// 开始向目标点同时旋转和移动
        /// </summary>
        /// <param name="target">旋转和移动的目标点</param>
        public void MoveTo(Vector2 target)
        {
            targetPoint = target;
            isMoving = true;
            isRotating = true;
            isStoppingAtTarget = true;
            isRotatingToAngle = false;
        }

        public override void Update(GameTime gameTime)
        {


            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            positionl = AbsolutePosition;
            if (currentSpeed > 0)
            {

                //foreach (var item in World.Ships)
                //{

                //}
                positionl = AbsolutePosition;
                Position += currentSpeed * Direction * elapsedTime;
            }
            if (isStoppingAtTarget)
            {
                if (Vector2.Distance(Position, targetPoint) < GameSettings.StopRadius)
                {
                    StopMoving();
                }
            }

            if (isMoving)
            {
                currentSpeed = MathHelper.Clamp(currentSpeed + elapsedTime * acceleration, 0, maxSpeed);
            }
            else
            {
                currentSpeed = MathHelper.Clamp(currentSpeed - elapsedTime * acceleration, 0, maxSpeed);
            }

            if (isRotating)
            {
                Vector2 d = targetPoint - Position;
                if (d != Vector2.Zero)
                {
                    RotateToAngleFrame(MathHelper.ToDegrees(DirectionToAngle(d)), elapsedTime);
                }
                else isRotating = false;
            }

            if (isRotatingToAngle)
            {
                RotateToAngleFrame(targetAngle, elapsedTime);
            }

            base.Update(gameTime);

            if (!isMoving)
            {
                MoveTo(new Vector2(World.MapSize.X * ((float)Base.GameOperators.Random.NextDouble() - 0.5f), World.MapSize.Y * ((float)Base.GameOperators.Random.NextDouble() - 0.5f)));
            }
        }


        /// <summary>
        /// 1帧内旋转到角度
        /// </summary>
        void RotateToAngleFrame(float angle, float elapsedTime)
        {

            float p = MathHelper.WrapAngle(MathHelper.ToRadians(angle));
            if (RadianRotation == p)
            {
                return;
            }
            else
            {
                bool pbigger = false;
                if (Math.Abs(p - RadianRotation) >= MathHelper.Pi)
                {
                    if (p < 0)
                    {
                        p += MathHelper.TwoPi;
                        pbigger = true;
                    }
                    else p -= MathHelper.TwoPi;
                }
                else pbigger = p > RadianRotation;
                if (pbigger)
                {
                    RadianRotation = MathHelper.Clamp(RadianRotation + MathHelper.ToRadians(angularRate) * elapsedTime, RadianRotation, p);
                }
                else
                {
                    RadianRotation = MathHelper.Clamp(RadianRotation - MathHelper.ToRadians(angularRate) * elapsedTime, p, RadianRotation);
                }
                if (RadianRotation == p)
                {
                    StopRotating();
                    return;
                }
            }
        }

        float DirectionToAngle(Vector2 d)
        {
            return Math.Sign(d.X) * (float)Math.Acos(Vector2.Dot(new Vector2(0, -1), Vector2.Normalize(d)));
        }

    }
}
