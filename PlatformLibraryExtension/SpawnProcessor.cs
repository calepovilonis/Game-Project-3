using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TInput = System.String;
using TOutput = PlatformLibrary.SpawnLocations;

namespace PlatformLibraryExtension
{
   [ContentProcessor(DisplayName = "Spawn Processor - Spawn")]
   class SpawnProcessor : ContentProcessor<TInput, TOutput>
   {
      public override TOutput Process(TInput input, ContentProcessorContext context)
      {
         string[] lines = input.Split(new char[] { '\n' });

         List<Vector2> positions = new List<Vector2>();

         foreach (string s in lines)
         {
            string[] coords = s.Split(new char[] { ',' });
            Vector2 pos = new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
            positions.Add(pos);
         }

         return new PlatformLibrary.SpawnLocations(positions);
      }
   }
}
