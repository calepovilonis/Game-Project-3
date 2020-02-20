using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{

   public class TempUnit : IEntity
   {
      // The color of the sprite
      Color color = Color.White;

      // The origin of the sprite (centered on its feet)
      Vector2 origin = new Vector2(0, 0);

      Sprite tex;

      SpriteEffects spriteEffects = SpriteEffects.None;

      public IEntity prev;
      public IEntity next;

      /// <summary>
      /// Gets and sets the position of the player on-screen
      /// </summary>
      public Vector2 Position = Vector2.Zero;

      public IEntity focus;

      public int health;
      public string role => "temp";

      private List<IEntity> ally;
      public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.8f * origin, 50, 75);

      /// <summary>
      /// Constructs a new player
      /// </summary>
      /// <param name="frames">The sprite frames associated with the player</param>
      public TempUnit(Sprite sprite, int x, int y)
      {
         tex = sprite;
         Position = new Vector2(x, y);
      }

      /// <summary>
      /// Updates the player, applying movement and physics
      /// </summary>
      /// <param name="gameTime">The GameTime object</param>
      public void Update(GameTime gameTime, bool start)
      {
      }
      /// <summary>
      /// Render the player sprite.  Should be invoked between 
      /// SpriteBatch.Begin() and SpriteBatch.End()
      /// </summary>
      /// <param name="spriteBatch">The SpriteBatch to use</param>
      public void Draw(SpriteBatch spriteBatch)
      {
#if VISUAL_DEBUG
            VisualDebugging.DrawRectangle(spriteBatch, Bounds, Color.Red);
#endif
         tex.Draw(spriteBatch, Position, color, 0, origin, 1, spriteEffects, 1);
      }

      public Vector2 getPos()
      {
         return Position;
      }

      public IEntity Prev
      {
         get
         {
            return prev;
         }
         set
         {
            prev = value;
         }
      }

      public IEntity Next
      {
         get
         {
            return next;
         }
         set
         {
            next = value;
         }
      }

      public List<IEntity> Ally
      {
         get
         {
            return ally;
         }
         set
         {
            ally = value;
         }
      }

      public int Health
      {
         get
         {
            return health;
         }
         set
         {
            health = value;
         }
      }
   }
}
