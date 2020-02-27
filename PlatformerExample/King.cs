using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   class King
   {
      public KingIdle idle;
      public KingWalking walking;

      public KingState state;

      public const int FRAME_RATE = 150;

      // The player sprite frames
      public Sprite[] frames;

      // The currently rendered frame
      public int currentFrame = 0;

      // A timer for animations
      public TimeSpan animationTimer;

      // The currently applied SpriteEffects
      public SpriteEffects spriteEffects = SpriteEffects.None;

      // The color of the sprite
      Color color = Color.White;

      // The origin of the sprite (centered on its feet)
      Vector2 origin = new Vector2(-30, -33);

      public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.8f * origin, 55, 90);

      /// <summary>
      /// Gets and sets the position of the player on-screen
      /// </summary>
      public Vector2 Position;

      public King(IEnumerable<Sprite> frames, int x, int y, KingIdle idle, KingWalking walking)
      {
         this.frames = frames.ToArray();
         this.idle = idle;
         this.walking = walking;
         this.state = idle;
         this.Position = new Vector2(x, y);
      }

      public void HandleInput(KeyboardState keys)
      {
         state.HandleInput(this, keys);
      }

      public void Update(GameTime gameTime)
      {
         state.Update(this, gameTime);
      }

      public void Draw(SpriteBatch spriteBatch)
      {
#if VISUAL_DEBUG
            VisualDebugging.DrawRectangle(spriteBatch, Bounds, Color.Red);
#endif
         frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 1, spriteEffects, 1);
      }
   }
}
