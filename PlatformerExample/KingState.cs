using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   interface KingState
   {
      void HandleInput(King king, KeyboardState state);
      void Update(King king, GameTime gameTime);
   }
}
