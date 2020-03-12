using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformLibrary
{
   public class SpawnLocations
   {
      public List<Vector2> spawns { get; set; }

      public SpawnLocations () { }

      public SpawnLocations (List<Vector2> spawns)
      {
         this.spawns = spawns;
      }
   }
}
