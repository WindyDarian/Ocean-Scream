using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MSTCOS.GameWorld
{
    public class Island:GameObject,IBoundingObject
    {
        public Island(World world,string tex, float scale, Vector2 position, float boundingRadius):base(world)
        {
            this.Texture = tex;
            this.Scale = scale;
            this.Position = position;
            this.boundingRadius = boundingRadius;
        }

        float boundingRadius;
        public float BoundingRadius
        {
            get { return boundingRadius; }
        }
    }
}
