using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TRead = PlatformLibrary.SpawnLocations;

namespace PlatformLibrary
{
   class SpawnReader : ContentTypeReader<TRead>
   {
      protected override TRead Read(ContentReader input, TRead existingInstance)
      {
         int count = input.ReadInt32();

         List<Vector2> positions = new List<Vector2>();

         for (int i = 0; i < count; i++)
         {
            positions.Add(input.ReadVector2());
         }

         return new PlatformLibrary.SpawnLocations(positions);
      }
   }
}
