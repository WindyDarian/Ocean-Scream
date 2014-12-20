using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MSTCOS.Base;
using Microsoft.Xna.Framework.Input;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// 阵营
    /// </summary>
    public class Faction:MSTCOS.Base.IUpdatable
    {
        int factionID = 0;
        /// <summary>
        /// 阵营号
        /// </summary>
        public int FactionID
        {
            get { return factionID; }
            set { factionID = value; }
        }
        string name;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        int resourceNum = 0;
        /// <summary>
        /// 资源点数
        /// </summary>
        public int ResourceNum
        {
            get { return resourceNum; }
            set { resourceNum = value; }
        }
        
        Color factionColor = Color.Ivory;
        /// <summary>
        /// 阵营色
        /// </summary>
        public Color FactionColor
        {
            get { return factionColor; }
            set { factionColor = value; }
        }

        FactionControllerType controllerType = FactionControllerType.None;
        /// <summary>
        /// 控制者类型
        /// </summary>
        public FactionControllerType ControllerType
        {
            get { return controllerType; }
            set { controllerType = value; }
        }

        World world;
        /// <summary>
        /// 所属游戏世界
        /// </summary>
        public World World
        {
            get { return world; }
            set { world = value; }
        }


        List<Ship> ships = new List<Ship>(10);
        /// <summary>
        /// 属于该阵营的船
        /// </summary>
        public List<Ship> Ships
        {
            get { return ships; }
        }

        int restoreRate;
        /// <summary>
        /// 护甲回复率
        /// </summary>
        public int RestoreRate
        {
            get { return restoreRate; }
        }

        float totalDamage = 0;
        /// <summary>
        /// 总伤害
        /// </summary>
        public float TotalDamage
        {
            get { return totalDamage; }
            set { totalDamage = value; }
        }

        //private Ship selectedShip;

        //public Ship SelectedShip
        //{
        //    get { return selectedShip; }
        //    set { selectedShip = value; }
        //}

        public Faction(World world, FactionControllerType controllerType, int factionID, Color factionColor, string name)
        {
            this.world = world;
            this.controllerType = controllerType;
            this.factionColor = factionColor;
            this.factionID = factionID;
            this.name = name;
        }

        /// <summary>
        /// 得到己方的船和视野中的敌方船
        /// </summary>
        /// <returns></returns>
        public List<Ship> ShipsInSight()
        {
            List<Ship> ret = new List<Ship>(10);
            List<Ship> ally = new List<Ship>(5);
            foreach (var item in world.Ships)
            {
                if (item.Faction==this)
                {
                    ret.Add(item);
                    ally.Add(item);
                }
            }
            foreach (var item in world.Ships)
            {
                if (item.Faction!=this)
                {
                    foreach (var al in ally)
                    {
                        if (Vector2.Distance(al.AbsolutePosition, item.AbsolutePosition) < al.RangeOfView)
                        {
                            if (!ret.Contains(item))
                            {
                                ret.Add(item);
                            }
                        }
                    }
                }   
            }
            return ret;
        }

        /// <summary>
        /// 获取并执行指令
        /// </summary>
        /// <param name="command">指令</param>
        public void GetCommand(string command)
        {
            throw new NotImplementedException();//未加入
        }


        public virtual void Update(GameTime gameTime)
        {
            CalculateRestoreRate();
            InnerInput(gameTime);
        }

        /// <summary>
        /// 内置输入
        /// </summary>
        /// <param name="gameTime"></param>
        void InnerInput(GameTime gameTime)
        {
            #region 玩家控制的情况
            if (controllerType == FactionControllerType.Player)
            {
                if (world.Factions.MemberList.Count > 1)
                {

                    bool controllingAlly = true;
                    if (World.HumanSelectedShips.Count <= 0)
                    {
                        controllingAlly = false;
                    }
                    else foreach (var item in world.HumanSelectedShips)
                        {
                            if (item != null && item.Faction != this)
                            {
                                controllingAlly = false;
                            }
                        }

                    if (world.MouseOnShip != null && world.MouseOnShip.Faction != this && controllingAlly)
                    {
                        InputState.CursorState = CursorState.Attack;
                    }
                    if (InputState.IsMouseButtonPressed(MouseButton.RightButton))
                    {
                        if (controllingAlly)
                        {
                            if (world.MouseOnShip == null || world.MouseOnShip.Faction == this)
                            {
                                foreach (var item in world.HumanSelectedShips)
                                {
                                    if (!item.IsBeingRemoved)
                                    {
                                        item.MoveTo(world.CurrentCamera.ReversePoint(InputState.CurrentMousePosition));
                                    }
                                }
                            }
                            else if (world.MouseOnShip != null && world.MouseOnShip.Faction != this)
                            {
                                foreach (var item in world.HumanSelectedShips)
                                {
                                    if (!item.IsBeingRemoved)
                                    {
                                        item.Attack(world.MouseOnShip);
                                    }
                                }
                            }
                        }

                    }
                    foreach (var item in ships)
                    {
                        if (item.Target != null)
                        {
                            item.Attack(item.Target);
                        }
                    }
                    if (InputState.IsKeyPressed(Keys.D1) && ships.Count > 0)
                    {
                        world.HumanSelectedShips.Clear();
                        world.HumanSelectedShips.Add(ships[0]);
                    }
                    else if (InputState.IsKeyPressed(Keys.D2) && ships.Count > 1)
                    {
                        world.HumanSelectedShips.Clear();
                        world.HumanSelectedShips.Add(ships[1]);
                    }
                    else if (InputState.IsKeyPressed(Keys.D3) && ships.Count > 2)
                    {
                        world.HumanSelectedShips.Clear();
                        world.HumanSelectedShips.Add(ships[2]);
                    }
                    else if (InputState.IsKeyPressed(Keys.D4) && ships.Count > 3)
                    {
                        world.HumanSelectedShips.Clear();
                        world.HumanSelectedShips.Add(ships[3]);
                    }
                    else if (InputState.IsKeyPressed(Keys.D5) && ships.Count > 4)
                    {
                        world.HumanSelectedShips.Clear();
                        world.HumanSelectedShips.Add(ships[4]);
                    }
                    else if (InputState.IsKeyPressed(Keys.A) && (InputState.IsKeyDown(Keys.LeftControl) || InputState.IsKeyDown(Keys.RightControl)))
                    {
                        world.HumanSelectedShips.Clear();
                        world.HumanSelectedShips.AddRange(ships);
                    }
                }
            }
            #endregion

            #region 蛋疼测试AI控制的情况
            else if (controllerType == FactionControllerType.EggPainInnerAI)
            {
                foreach (var item in ships)
                {
               
                    if (item.IsBlocked)
                    {
                        item.StopMoving();
                        if (!item.IsRotating)
                        {

                            item.StartRotating(new Vector2((float)(GameOperators.Random.NextDouble() - 0.5) * 1000f, (float)(GameOperators.Random.NextDouble() - 0.5) * 1000f));
                        }
                    }
                    else if (!item.IsMoving&&!item.IsRotating)
                    {
                        item.MoveTo(new Vector2((float)(GameOperators.Random.NextDouble() - 0.5) * 1000f, (float)(GameOperators.Random.NextDouble() - 0.5) * 1000f));
                    }
                    foreach (var ta in world.Ships)
                    {
                        if (ta.Faction != this)
                        {
                            item.Attack(ta);
                        }
                    }
                }

            }
            #endregion
        }

        /// <summary>
        /// 计算回复
        /// </summary>
        void CalculateRestoreRate()
        {
            switch (resourceNum)
            {
                case 0:
                    restoreRate = 0;
                    break;
                case 1:
                    restoreRate = 5;
                    break;
                case 2:
                    restoreRate = 10;
                    break;
                case 3:
                    restoreRate = 15;
                    break;
                case 4:
                    restoreRate = 25;
                    break;
                case 5:
                    restoreRate = 50;
                    break;
            }
        }

    }

    /// <summary>
    /// 阵营控制者
    /// </summary>
    public enum FactionControllerType
    {
        Player,
        AI,
        InnerAI,
        EggPainInnerAI,
        None,
    }
}
