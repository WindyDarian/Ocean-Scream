using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MSTCOS.Base;
using Microsoft.Xna.Framework.Audio;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// 船只
    /// </summary>
    public class Ship:GameObject,IBoundingObject
    {
        int id;
        /// <summary>
        /// 标号
        /// </summary>
        public int ID
        {
            get { return id; }
        }

        float maxArmor = 1000f;
        /// <summary>
        /// 最大护甲值
        /// </summary>
        public float MaxArmor
        {
            get { return maxArmor; }
            set
            {
                if (value > 0)
                {
                    maxArmor = value;
                    armor = MathHelper.Clamp(armor, 0, value);
                }
            }

        }

        float armor = 1000f;
        /// <summary>
        /// 护甲值
        /// </summary>
        public float Armor
        {
            get { return armor; }
            set { armor = MathHelper.Clamp(value, 0, maxArmor); }
        }
     
        float acceleration = 25f;
        /// <summary>
        /// 加速度大小
        /// </summary>
        public float Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }
     
        float maxSpeed = 25f;
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
            set { currentSpeed = value; }
        }

        Vector2 velocity = Vector2.Zero;
        public Vector2 Velocity
        {
            get { return velocity; }
        }

        private Faction faction;
        /// <summary>
        /// 所属阵营
        /// </summary>
        public Faction Faction
        {
            get { return faction; }
        }

        float angularRate = 0f;
        /// <summary>
        /// 每秒旋转角度
        /// </summary>
        public float AngularRate
        {
            get { return angularRate; }
            set { angularRate = MathHelper.Clamp(value,0,float.MaxValue); }
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
            set { isBlocked = value; }
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
            get 
            {
                return positionl;
            }
            set { positionl = value; }
        }
       
        List<Cannon> cannons = new List<Cannon>(5);
        /// <summary>
        /// 炮台
        /// </summary>
        public List<Cannon> Cannons
        {
            get { return cannons; }
        }

        float boundingRadius = 20f;
        /// <summary>
        /// 碰撞半径
        /// </summary>
        public float BoundingRadius
        {
            get { return boundingRadius; }
            set { boundingRadius = MathHelper.Clamp(value, 0, float.MaxValue); }
        }

        float addWaveSpan;//添加波纹的间隔
        float addWaveSpanRemaining = 0;

        float rangeOfView = 400f;

        public float RangeOfView
        {
            get { return rangeOfView; }
            set { rangeOfView = value; }
        }

        float selectionRadius = 0.1f ;

        /// <summary>
        /// 选择框半径
        /// </summary>
        public float SelectionRadius
        {
            get { return selectionRadius; }
            set { selectionRadius = value; }
        }

        Texture2D selectionTexture;
        /// <summary>
        /// 选择纹理
        /// </summary>
        public Texture2D SelectionTexture
        {
            get { return selectionTexture; }
        }
        Vector2 selectionTextureCenter = Vector2.Zero;
        float selectionTextureScale = 1;


        Ship target;
        /// <summary>
        /// 最后一次攻击的目标，或者玩家指定的攻击目标
        /// </summary>
        public Ship Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// 碰撞移动缓冲
        /// </summary>
        Vector2 tempCharge = Vector2.Zero;
       

        public Ship(World world, Vector2 position, float rotation, Faction faction, int id)
            : base(world)
        {
            //初始化数据
            base.Position = position;
            positionl = position;
            base.Rotation = rotation;
            this.faction = faction;
            this.id = id;

            //如果要将Ship类作为抽象类并创建多个种类的船则删除以下信息
            maxSpeed = 25f;
            acceleration = 10f;
            ChildItems.Add(new Sail(world, this, new Vector2(0, 0), faction, 1f));
            //ChildItems.Add(new Sail(world, this, new Vector2(0,-109),faction,0.5f));
            //ChildItems.Add(new Sail(world, this, new Vector2(0, 34), faction, 0.75f));
            this.Texture = @"Ship";
            Scale = 0.15f;
            boundingRadius = 15f;
            angularRate = 45f;
            rangeOfView = 400f;
            addWaveSpan = 1f;

            ChildItems.Add(new Cannon(world, this, 90));
            cannons.Add((Cannon)ChildItems.LatestAddedItem);
            ChildItems.Add(new Cannon(world, this, -90));
            cannons.Add((Cannon)ChildItems.LatestAddedItem);

            selectionRadius = 50f;
            selectionTexture = GameOperators.Content.Load<Texture2D>(@"Circle1");
            selectionTextureCenter = new Vector2(selectionTexture.Width / 2, selectionTexture.Height / 2);
            if (selectionTexture.Width > 0)
            {
                selectionTextureScale = selectionRadius / selectionTexture.Width *1.3f;
            }
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

        /// <summary>
        /// 设定目标
        /// </summary>
        /// <param name="target"></param>
        public void Attack(Ship target)
        {
            if (!IsBeingRemoved)
            {
                Target = target;
            }
         
        }

        /// <summary>
        /// 尝试向目标开火
        /// </summary>
        /// <param name="target">目标</param>
        public void FireAt(Ship target)
        {
            if (!IsBeingRemoved)
            {
                foreach (var cannon in cannons)
                {
                    cannon.FireAt(target);
                }
            }
        }

        /// <summary>
        /// 返回目标是否在火力范围中
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>目标是否在火力范围中</returns>
        public bool IsTargetInFireRange(Ship target)
        {
            foreach (Cannon item in cannons)
            {
                if (item.IsTargetInRange(target))
                {
                    return true;
                }
            }
            return false;
        }

        bool tempblock= false;

        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="damage">伤害</param>
        /// <param name="delay">延迟</param>
        public void ReceiveDamage(float damage, float delay,bool isCritical)
        {
            if (damage > 0)
            {

                incomingDamages.Add(new IncomingDamage(damage, delay,isCritical));
            }
        }
        List<IncomingDamage> incomingDamages = new List<IncomingDamage>(10);
        List<IncomingDamage> removingDamages = new List<IncomingDamage>(5);


        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += tempCharge;
            tempCharge = Vector2.Zero;

            if (tempblock)
            {
                currentSpeed = 0;
                tempblock = false;
            }

            positionl = AbsolutePosition;
            if (currentSpeed > 0)
            {

                //foreach (var item in World.Ships)
                //{
                    
                //}
                positionl = AbsolutePosition;
                Position += currentSpeed * Direction * elapsedTime;
                //positionn = Position + currentSpeed * Direction * elapsedTime;
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
            velocity = currentSpeed* Direction; 

            if (isRotating)
            {
                Vector2 d = targetPoint - Position;
               // try
                //{
                    if (d != Vector2.Zero)
                    {
                        RotateToAngleFrame(MathHelper.ToDegrees(DirectionToAngle(d)), elapsedTime);
                    }
                    else isRotating = false;
                //}
               // catch
                //{
                   // isRotating = false;
                //}
            }

            if (isRotatingToAngle)
            {
                RotateToAngleFrame(targetAngle, elapsedTime);
            }

            //计算伤害
            foreach (var dmg in incomingDamages)
            {
                dmg.DelayRemain -= elapsedTime;
                if (dmg.DelayRemain<=0)
                {
                    OnDamage(dmg.Damage,dmg.Critical);
                    removingDamages.Add(dmg);
                }
            }
            foreach (var dmg in removingDamages)
            {
                incomingDamages.Remove(dmg);
            }
            removingDamages.Clear();

            if (Armor<=0)
            {
                BeginToDie();
            }

            //回复护甲
            Armor += faction.RestoreRate * elapsedTime;

            base.Update(gameTime);
            isBlocked = false;

            if (target!=null&&target.IsBeingRemoved)
            {
                target = null;
            }

        }

        /// <summary>
        /// 执行攻击命令啥的
        /// </summary>
        public void PerformCommand()
        {
            if (!IsBeingRemoved&&target!= null)
            {
                FireAt(target);
            }
        }


        /// <summary>
        /// 受到伤害并扣血
        /// </summary>
        /// <param name="dmg"></param>
        void OnDamage(float dmg,bool isCritical)
        {
            Armor -= dmg;
            World.AddDamageText(dmg, isCritical,AbsolutePosition);
        }

        public bool IsMouseOn()
        {
            return Vector2.DistanceSquared(World.CurrentCamera.ReversePoint(MSTCOS.Base.InputState.CurrentMousePosition), Position) <= selectionRadius * selectionRadius;
        }

        /// <summary>
        /// 1帧内旋转到角度
        /// </summary>
        void RotateToAngleFrame(float angle,float elapsedTime)
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
                    RadianRotation = MathHelper.Clamp(RadianRotation + MathHelper.ToRadians(angularRate)*elapsedTime, RadianRotation, p);
                }
                else
                {
                    RadianRotation = MathHelper.Clamp(RadianRotation - MathHelper.ToRadians(angularRate) * elapsedTime, p, RadianRotation);
                }
                if (RadianRotation==p)
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

        public override void Draw(GameTime gameTime)
        {
            addWaveSpanRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (addWaveSpanRemaining<0)
            {
                addWaveSpanRemaining = addWaveSpan;
                World.AddLowerParticle(new SpriteParticle.Particle(World, GameOperators.Content.Load<Texture2D>("water"), 1, 1, 0, AbsolutePosition, AbsolutePosition, 0.1f, 0.5f, 10));
            }
            base.Draw(gameTime);
            bool ct = World.HumanSelectedShips.Contains(this);
            isDrawingArmorBar = GameSettings.IsArmorBarOn || this == World.MouseOnShip || ct;
            isDrawingSelection =  this == World.MouseOnShip || ct;
            isDrawingTargetInf = this == World.MouseOnShip || ct;

        }

        bool isDrawingArmorBar;
        /// <summary>
        /// 是否绘制血条
        /// </summary>
        public bool IsDrawingArmorBar
        {
            get { return isDrawingArmorBar; }
        }

        bool isDrawingSelection;
        /// <summary>
        /// 是否绘制选择圈
        /// </summary>
        public bool IsDrawingSelection
        {
            get { return isDrawingSelection; }
        }

        bool isDrawingTargetInf;
        /// <summary>
        /// 是否绘制目标信息
        /// </summary>
        public bool IsDrawingTargetInf
        {
            get { return isDrawingTargetInf; }
            set { isDrawingTargetInf = value; }
        }
        
        


        /// <summary>
        /// 计算对另一条船的撞击伤害，由World调用
        /// </summary>
        /// <param name="t"></param>
        public float GetKnockDamage(Ship t)
        {
            if (faction != t.faction)
            {
                if (AbsolutePosition == t.AbsolutePosition || Vector2.Dot(Vector2.Normalize(t.AbsolutePosition-AbsolutePosition),AbsoluteDirection)>0.866f)
                {
                    if (Vector2.Distance(Velocity, t.Velocity) > 7.5f)
                    {

                        return 100+Vector2.Dot(AbsoluteDirection, (Velocity - t.Velocity)) * 14f;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 表示被挡住，由World调用
        /// </summary>
        public void Block(Vector2 direct)
        {

            isBlocked = true;
            tempblock = true;
            tempCharge += direct;
           
        }

        public override void BeginToDie()
        {
            isDrawingTargetInf = false;
            isDrawingSelection = false;
            isDrawingArmorBar = false;
            Base.SoundManager.Play3DSound(Base.GameOperators.Content.Load<SoundEffect>(@"Audio\Explosion"), AbsolutePosition, World.CurrentCamera.Center, World.CurrentCamera.Scale,0.5f);
            for (int i = 0; i < 10; i++)
            {
                World.AddUpperParticle(new SpriteParticle.Particle(World, Base.GameOperators.Content.Load<Texture2D>("explosion"), 7, 0.7f, 0f, AbsolutePosition, AbsolutePosition + new Vector2(GameOperators.Random.Next(-20, 20), GameOperators.Random.Next(-50, 50)), 0f, 2f*(float)GameOperators.Random.NextDouble(), 2));

            }
            base.BeginToDie();
        }

        public void DrawCircle()
        {
            Color c;
            if (Faction == null)
            {
                c = Color.Gray.CrossAlpha(0.25f);
            }
            else c =Faction.FactionColor.CrossAlpha(0.25f);

            GameOperators.SpriteBatch.Begin();
            GameOperators.SpriteBatch.Draw(selectionTexture, World.CurrentCamera.TransformPoint(AbsolutePosition), null, c, 0, selectionTextureCenter, selectionTextureScale * World.CurrentCamera.Scale, SpriteEffects.None, 0.9f);
            GameOperators.SpriteBatch.End();
        }

        public void DrawTargetInf()
        {
            GameOperators.PrimitiveBatch.Begin(PrimitiveType.LineList);
            if (isMoving&&isStoppingAtTarget)
            {
                GameOperators.PrimitiveBatch.AddVertex(World.CurrentCamera.TransformPoint(this.AbsolutePosition), Color.Yellow);

                GameOperators.PrimitiveBatch.AddVertex(World.CurrentCamera.TransformPoint(targetPoint), Color.Yellow);
                
            }
            if (Target != null)
            {
                GameOperators.PrimitiveBatch.AddVertex(World.CurrentCamera.TransformPoint(this.AbsolutePosition+Direction*10f), Color.White);

                GameOperators.PrimitiveBatch.AddVertex(World.CurrentCamera.TransformPoint(Target.AbsolutePosition), Color.Red);
            }


            GameOperators.PrimitiveBatch.End();
        }

        

        
        /// <summary>
        /// 将要受到的伤害
        /// </summary>
        class IncomingDamage
        {
            /// <summary>
            /// 伤害值
            /// </summary>
            public float Damage;
            /// <summary>
            /// 剩余延迟
            /// </summary>
            public float DelayRemain;
            /// <summary>
            /// 是否暴击
            /// </summary>
            public bool Critical;
            public IncomingDamage(float damage, float delay,bool isCritical)
            {
                this.Damage = damage;
                this.DelayRemain = delay;
                this.Critical = isCritical;
            }
        }

    }
    
}
