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
   public class Knight : IEntity
   {
      enum AnimState
      {
         Idle,
         Moving,
         Attacking,
         Death
      }

      const int FRAME_RATE = 150;

      public int health;

      public IEntity prev;
      public IEntity next;

      Grid grid;

      public IEntity focus;

      public string role => "knight";

      private List<IEntity> ally;

      // The player sprite frames
      Sprite[] frames;

      // The currently rendered frame
      int currentFrame = 0;

      // The player's animation state
      AnimState animationState = AnimState.Idle;

      // A timer for animations
      TimeSpan animationTimer;

      // The currently applied SpriteEffects
      SpriteEffects spriteEffects = SpriteEffects.None;

      // The color of the sprite
      Color color = Color.White;

      // The origin of the sprite (centered on its feet)
      Vector2 origin = new Vector2(30, 6);

      /// <summary>
      /// Gets and sets the position of the player on-screen
      /// </summary>
      public Vector2 Position;

      public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.8f * origin, 60, 115);

      /// <summary>
      /// Constructs a new player
      /// </summary>
      /// <param name="frames">The sprite frames associated with the player</param>
      public Knight(IEnumerable<Sprite> frames, int x, int y, Grid g)
      {
         this.frames = frames.ToArray();
         animationState = AnimState.Idle;
         Position = new Vector2(x, y);
         grid = g;
         health = 100;
      }

      /// <summary>
      /// Updates the player, applying movement and physics
      /// </summary>
      /// <param name="gameTime">The GameTime object</param>
      public void Update(GameTime gameTime, bool start)
      {
         if (start)
         {
            focus = grid.FindClosest(this);
            Debug.WriteLine($"Focus: {focus}");

            Debug.WriteLine($"Health: {health}");

            if (focus == null && health > 0) animationState = AnimState.Idle;

            else if (health <= 0)
            {
               animationState = AnimState.Death;
               focus = null;
            }

            else if (focus != null && animationState != AnimState.Attacking && health > 0)
            {
               animationState = AnimState.Moving;
               if (Bounds.X > focus.Bounds.X) Position.X -= 1;
               if (Bounds.X < focus.Bounds.X) Position.X += 1;
               if (Bounds.Y > focus.Bounds.Y) Position.Y -= 1;
               if (Bounds.Y < focus.Bounds.Y) Position.Y += 1;
               if (focus.Bounds.CollidesWith(Bounds))
               {
                  animationState = AnimState.Attacking;
               }
            }

            else if (currentFrame == 15 && focus != null && health > 0)
            {
               if (focus.Health <= 0)
               {
                  focus = grid.FindClosest(this);
                  animationState = AnimState.Moving;
               }
               else if (focus.Bounds.CollidesWith(Bounds))
               {
               }
               else { animationState = AnimState.Moving; }
            }
         }

         // Apply animations
         switch (animationState)
         {
            case AnimState.Idle:
               animationTimer += gameTime.ElapsedGameTime;
               if (animationTimer.TotalMilliseconds > FRAME_RATE * 7)
               {
                  animationTimer = new TimeSpan(0);
               }
               currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE);
               break;

            case AnimState.Moving:
               animationTimer += gameTime.ElapsedGameTime;
               // Walking frames are 9 & 10
               if (animationTimer.TotalMilliseconds > FRAME_RATE * 8)
               {
                  animationTimer = new TimeSpan(0);
               }
               currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 18;
               break;

            case AnimState.Attacking:
               animationTimer += gameTime.ElapsedGameTime;
               if (animationTimer.TotalMilliseconds > FRAME_RATE * 7)
               {
                  animationTimer = new TimeSpan(0);
               }
               currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
               break;

            case AnimState.Death:
               animationTimer += gameTime.ElapsedGameTime;
               spriteEffects = SpriteEffects.None;
               /*if (animationTimer.TotalMilliseconds > FRAME_RATE * 9)
               {
                  animationTimer = new TimeSpan(0);
               }*/
               if (currentFrame >= 35) currentFrame = 35;
               else currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 27;
               break;
         }
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
         frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 3, spriteEffects, 1);
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
