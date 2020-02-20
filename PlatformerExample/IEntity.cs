using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   public interface IEntity
   {
      Vector2 getPos();
      string role { get; }
      BoundingRectangle Bounds { get; }

      IEntity Prev { get; set; }
      IEntity Next { get; set; }

      List<IEntity> Ally { get; set; }

      int Health { get; set; }

      void Update(GameTime gameTime, bool start);
      void Draw(SpriteBatch spriteBatch);
   }
}
