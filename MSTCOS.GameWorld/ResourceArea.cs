using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTCOS.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MSTCOS.GameWorld
{
    /// <summary>
    /// 资源点
    /// 2012年3月21日 修正了蓝方在占领机制上占优的BUG，现在若双方有等量船在区域内则占领值不变
    /// 2012.4.2 add the ID param
    /// </summary>
    public class ResourceArea:Base.IDrawable,Base.IUpdatable
    {
        int id;
        /// <summary>
        /// 标号
        /// </summary>
        public int ID
        {
            get { return id; }
        }

        Vector2 position = Vector2.Zero;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }
        
        float radius = 64f;
        /// <summary>
        /// 半径
        /// </summary>
        public float Radius
        {
            get { return radius; }
        }

        Texture2D texture;
        Texture2D blank;

        Faction controllingFaction;
        /// <summary>
        /// 控制者
        /// </summary>
        public Faction ControllingFaction
        {
            get { return controllingFaction; }
        }

        float scale;
        Vector2 center;
        World world;
        int t = 0;
        Faction tempController = null;
        float tempControlledTime;


        float tempControlRequiredTime = 1f;
        //占领所需领先时间
        public float TempControlRequiredTime
        {
            get { return tempControlRequiredTime; }
            set { tempControlRequiredTime = value; }
        }

        public ResourceArea(int id, float radius, Vector2 position, Faction faction, World world)
        {
            this.id = id;
            this.radius = radius;
            this.controllingFaction = faction;
            this.position = position;
            texture = GameOperators.Content.Load<Texture2D>("Circle2");
            blank  = GameOperators.Content.Load<Texture2D>("blank");
            scale = radius * 2 / texture.Width;
            center = new Vector2(texture.Width / 2, texture.Height / 2);
            this.world = world;
            tempControlRequiredTime = 1f;
            if (faction != null)
            {
                faction.ResourceNum++;
            }
            
        }


        public void Update(GameTime gameTime)
        {

            //占领判断
            if (t <= 0)
            {
                bool converted = false ;
                Faction bestFaction = controllingFaction;
                int highestnum = 0;
                if (controllingFaction!= null)
                {
                    highestnum = FactionUnitInArea(controllingFaction);
                }
                foreach (var f in world.Factions)
                {
                    if (f!= controllingFaction)
                    {
                        int num = FactionUnitInArea(f);
                        if (num > highestnum)
                        {
                            if (!converted)
                            {

                                highestnum = num;
                                bestFaction = f;
                                converted = true;
                                
                            }

                        }
                        else if (converted && num == highestnum)//为了解决双方同时到达的问题
                        {
                            bestFaction = controllingFaction;
                            tempController = null;
                            tempControlledTime = 0;
                        }
                    }
      
                }

                if (controllingFaction != bestFaction)
                {
                    //if (tempController == null)
                    //{
                    //    tempController = bestFaction;
                    //}
                    //else 
                    if (tempController != bestFaction)
                    {

                        tempController = bestFaction;
                        tempControlledTime = 0;
                    }
                    else if(tempControlledTime>=tempControlRequiredTime)
                    {
                    
                        if (controllingFaction != null)
                        {
                            controllingFaction.ResourceNum--;
                        }
                        if (bestFaction != null)
                        {
                            bestFaction.ResourceNum++;
                        }
                        controllingFaction = bestFaction;
                        tempController = null;
                        tempControlledTime = 0;
                    }
                }
                else
                {
                    tempController = null;
                    tempControlledTime = 0;
                }

                t = 10;
            }
            else t--;

            if(tempController!= null)
            {
                tempControlledTime=MathHelper.Clamp(tempControlledTime+(float)gameTime.ElapsedGameTime.TotalSeconds,0,tempControlRequiredTime);

            }
    
        }

        /// <summary>
        /// 得到区域内某阵营的单位数（渣优化）
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public int FactionUnitInArea(Faction f)
        {
            int n = 0;
            foreach (var ship in world.Ships)
            {
                if (ship.Faction == f)
                {
                    if (Vector2.Distance(ship.AbsolutePosition, position) < radius)
                    {
                        n++;
                    }
                }
            }
            return n;
        }

        public void Draw(GameTime gameTime)
        {
            Color c;
            if (controllingFaction == null)
            {
                c = Color.Gray.CrossAlpha(0.25f);
            }

            else c = controllingFaction.FactionColor.CrossAlpha(0.75f);

            GameOperators.SpriteBatch.Begin();
            Vector2 p = world.CurrentCamera.TransformPoint(position);
            GameOperators.SpriteBatch.Draw(texture, p, null, c, 0, center, scale * world.CurrentCamera.Scale, SpriteEffects.None, 0.9f);
           
            if (tempController != null)
            {
                float rate = 0;
                rate = MathHelper.Clamp(tempControlledTime / tempControlRequiredTime, 0, 1);
                GameOperators.SpriteBatch.Draw(blank, new Rectangle((int)(p.X - 25 * world.CurrentCamera.Scale), (int)p.Y - 10, (int)(rate / 50f), 20), null, tempController.FactionColor.CrossAlpha(0.9f), 0, center, SpriteEffects.None, 0.9f);

            }
            GameOperators.SpriteBatch.End();
        }

        public void DrawUI()
        {
            if (tempController != null)
            {
                GameOperators.SpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.NonPremultiplied);
                Vector2 p = world.CurrentCamera.TransformPoint(position);


                float rate = 0;
                rate = MathHelper.Clamp(tempControlledTime / tempControlRequiredTime, 0, 1);
                GameOperators.SpriteBatch.Draw(blank, new Rectangle((int)(p.X - 50 * world.CurrentCamera.Scale), (int)(p.Y - 10 * world.CurrentCamera.Scale), (int)(rate * 100f * world.CurrentCamera.Scale), (int)(20 * world.CurrentCamera.Scale)), tempController.FactionColor.CrossAlpha(0.9f));


                GameOperators.SpriteBatch.End();
            }
        }
    }
}
