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
   public class Mage : IEntity
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

      public string role => "mage";

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
      Vector2 origin = new Vector2(-100, -55);

      ParticleSystem system;

      /// <summary>
      /// Gets and sets the position of the player on-screen
      /// </summary>
      public Vector2 Position;

      Random random = new Random();

      public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.8f * origin, 60, 100);

      /// <summary>
      /// Constructs a new player
      /// </summary>
      /// <param name="frames">The sprite frames associated with the player</param>
      public Mage(IEnumerable<Sprite> frames, int x, int y, Grid g, ParticleSystem particles)
      {
         this.frames = frames.ToArray();
         animationState = AnimState.Idle;
         Position = new Vector2(x - 100, y);
         grid = g;
         health = 50;

         system = particles;
         system.SpawnPerFrame = 150;

         system.SpawnParticle = (ref Particle particle) =>
         {
            if (AnimState.Attacking != animationState || focus == null)
            {
               particle.Life = 0.0f;
               particle.Position = new Vector2(0, 0);
            }
            else
            {
               var focPos = focus.Bounds;
               particle.Position = new Vector2(focPos.X+25, focPos.Y+50);
               particle.Life = 3.0f;
            }
            particle.Velocity = new Vector2(
               MathHelper.Lerp(-50* (float)random.NextDouble(), 50 * (float)random.NextDouble(), (float)random.NextDouble()), // X between -50 and 50
               MathHelper.Lerp(-100*(float)random.NextDouble(), 30 * (float)random.NextDouble(), (float)random.NextDouble()) // Y between 0 and 100
               );
            particle.Acceleration = 1.35f * new Vector2(0, (float)-random.NextDouble());
            particle.Color = Color.MediumPurple * .4f;
            particle.Scale = 1f;
         };


         system.UpdateParticle = (float deltaT, ref Particle particle) =>
         {
            particle.Velocity += deltaT * particle.Acceleration;
            particle.Position += deltaT * particle.Velocity;
            particle.Scale -= deltaT;
            particle.Life -= deltaT;
         };
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

            system.Update(gameTime);

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
               if (focus.Bounds.CollidesWith(new BoundingRectangle(Bounds.X - 50, Bounds.Y - 50, 260, 300)))
               {
                  animationState = AnimState.Attacking;
               }
            }

            else if (currentFrame == 23 && focus != null && health > 0)
            {
               if (focus.Health <= 0)
               {
                  focus = grid.FindClosest(this);
                  animationState = AnimState.Moving;
               }
               else if (focus.Bounds.CollidesWith(new BoundingRectangle(Bounds.X - 50, Bounds.Y - 50, 260, 300)))
               {
                  focus.Health -= 2;
                  //focus.Ally.Remove(focus);
                  //focus.Ally.Add(focus);
               }
               else
               {
                  animationState = AnimState.Moving;
               }
            }
         }

         // Apply animations
         switch (animationState)
         {
            case AnimState.Idle:
               animationTimer += gameTime.ElapsedGameTime;
               if (animationTimer.TotalMilliseconds > FRAME_RATE * 6)
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
               currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 8;
               break;

            case AnimState.Attacking:
               animationTimer += gameTime.ElapsedGameTime;
               if (animationTimer.TotalMilliseconds > FRAME_RATE * 8)
               {
                  animationTimer = new TimeSpan(0);
               }
               currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 16;
               break;

            case AnimState.Death:
               animationTimer += gameTime.ElapsedGameTime;
               spriteEffects = SpriteEffects.None;
               /*if (animationTimer.TotalMilliseconds > FRAME_RATE * 7)
               {
                  animationTimer = new TimeSpan(0);
               }*/
               if (currentFrame >= 30) currentFrame = 30;
               else currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 24;
               break;
         }
         if (currentFrame >= 30) currentFrame = 30;
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
         frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 1, spriteEffects, 1);
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
