using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = System.String;

namespace PlatformLibraryExtension
{

   [ContentImporter(".spwn", DisplayName = "Spawn Importer - Spawn", DefaultProcessor = "SpawnProcessor")]
   class SpawnImporter : ContentImporter<TImport>
   {
      public override string Import(string filename, ContentImporterContext context)
      {
         return System.IO.File.ReadAllText(filename);
      }

   }
}
