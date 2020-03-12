using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using TWrite = PlatformLibrary.SpawnLocations;

namespace PlatformLibraryExtension
{
   class SpawnWriter : ContentTypeWriter<TWrite>
   {
      protected override void Write(ContentWriter output, TWrite value)
      {
         output.Write(value.spawns.Count);
         foreach(Vector2 v in value.spawns)
         {
            output.Write(v);
         }
      }

      /// <summary>
      /// Gets the reader needed to read the binary content written by this writer
      /// </summary>
      /// <param name="targetPlatform"></param>
      /// <returns></returns>
      public override string GetRuntimeReader(TargetPlatform targetPlatform)
      {
         return "PlatformLibrary.SpawnReader, PlatformLibrary";
      }
   }
}
