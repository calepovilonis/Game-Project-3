using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   public class Fire
   {
      Vector2 position;

      Random random = new Random();

      public ParticleSystem system;

      public Fire(Vector2 pos, ParticleSystem s)
      {
         this.position = pos;
         this.system = s;

         system.SpawnParticle = (ref Particle particle) =>
         {
            particle.Position = new Vector2(pos.X, pos.Y);
            particle.Velocity = new Vector2(
                MathHelper.Lerp(-20, 20, (float)random.NextDouble()), // X between -50 and 50
                MathHelper.Lerp(-25, 0, (float)random.NextDouble()) // Y between 0 and 100
                );
            particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
            particle.Color = Color.White * .4f;
            particle.Scale = .0625f;
            particle.Life = 1f;
         };

         // Set the UpdateParticle method
         system.UpdateParticle = (float deltaT, ref Particle particle) =>
         {
            particle.Velocity += deltaT * particle.Acceleration;
            particle.Position += deltaT * particle.Velocity;
            particle.Life -= deltaT;
         };
      }

      public void Update(GameTime gameTime)
      {
         system.Update(gameTime);
      }
   }
}
