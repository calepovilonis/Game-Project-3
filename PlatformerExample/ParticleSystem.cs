using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   public class ParticleSystem : DrawableGameComponent
   {
      Particle[] particles;

      Texture2D texture;

      SpriteBatch spriteBatch;

      Random random = new Random();

      int nextIndex = 0;

      public Vector2 Emitter { get; set; }

      public int SpawnPerFrame { get; set; }

      public ParticleSpawner SpawnParticle { get; set; }

      public Camera camera;

      public ParticleUpdater UpdateParticle { get; set; }

      public delegate void ParticleSpawner(ref Particle particle);

      public delegate void ParticleUpdater(float deltaT, ref Particle particle);

      public ParticleSystem(GraphicsDevice graphicsDevice, int size, Texture2D texture, Game game, Camera camera) : base(game)
      {
         this.particles = new Particle[size];
         this.spriteBatch = new SpriteBatch(graphicsDevice);
         this.texture = texture;
         this.camera = camera;
      }

      public override void Update(GameTime gameTime)
      {
         // Make sure our delegate properties are set
         if (SpawnParticle == null || UpdateParticle == null) return;

         // Part 1: Spawn new particles 
         for (int i = 0; i < SpawnPerFrame; i++)
         {
            // Create the particle
            SpawnParticle(ref particles[nextIndex]);

            // Advance the index 
            nextIndex++;
            if (nextIndex > particles.Length - 1) nextIndex = 0;
         }

         // Part 2: Update Particles
         float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
         for (int i = 0; i < particles.Length; i++)
         {
            // Skip any "dead" particles
            if (particles[i].Life <= 0) continue;

            // Update the individual particle
            UpdateParticle(deltaT, ref particles[i]);
         }
      }

      public override void Draw(GameTime gameTime)
      {
         spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, camera.TranslationMatrix);
         Console.WriteLine(texture);
         // Iterate through the particles
         for (int i = 0; i < particles.Length; i++)
         {
            // Skip any "dead" particles
            if (particles[i].Life <= 0) continue;

            //Console.WriteLine("Drawing Particles @ pos: " + particles[i].Position);
            // Draw the individual particles
            spriteBatch.Draw(texture, particles[i].Position, null, particles[i].Color, 0f, Vector2.Zero, particles[i].Scale, SpriteEffects.None, 0);
         }

         spriteBatch.End();
      }

   }
}
