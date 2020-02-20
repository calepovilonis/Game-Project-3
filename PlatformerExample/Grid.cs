using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   public class Grid
   {
      int cellSize;

      IEntity[,] cells;

      public Grid(int width, int cellSize)
      {
         if (cellSize > 1)
         {
            this.cellSize = cellSize;
            int num = width / cellSize;
            cells = new IEntity[num, num];
         }
         else
         {
            this.cellSize = cellSize;
            cells = new IEntity[cellSize, cellSize];
         }
      }

      public void Add(IEntity entity)
      {
         int cX = (int)entity.Bounds.X;
         int cY = (int)entity.Bounds.Y;

         entity.Prev = null;
         entity.Next = cells[0, 0];

         cells[0, 0] = entity;

         if (entity.Next != null) entity.Next.Prev = entity;
      }

      public IEntity FindClosest(IEntity entity)
      {
         IEntity foe;
         if (cellSize > 1)
         {
            int cX = (int)(entity.Bounds.X / cellSize);
            int cY = (int)(entity.Bounds.Y / cellSize);
            foe = cells[cX, cY];
         }
         else
         {
            foe = cells[0, 0];
         }

         IEntity closestFoe = null;
         float bestDist = 10000;

         while (foe != null)
         {
            var d1 = new Vector2(foe.Bounds.X, foe.Bounds.Y);
            var d2 = new Vector2(entity.Bounds.X, entity.Bounds.Y);
            float dist = (float)Math.Sqrt(Math.Pow((d2.X - d1.X), 2) + Math.Pow((d2.Y - d1.Y), 2));

            if (dist < bestDist && entity.Ally.Contains(foe) == false && foe.Health > 0)
            {
               bestDist = dist;
               closestFoe = foe;
            }
            foe = foe.Next;
         }
         return closestFoe;
      }

   }
}
