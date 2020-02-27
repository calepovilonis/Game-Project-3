using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   class KingIdle : KingState
   {
      public KingIdle() { }

      public void HandleInput(King king, KeyboardState state)
      {
         if (state.GetPressedKeys().Count() == 0) {
            king.state = king.idle;
            return;
         }

         var key = state.GetPressedKeys()[0];
         switch (key)
         {
            case Keys.W:
            case Keys.A:
            case Keys.S:
            case Keys.D:
               king.state = king.walking;
               break;
            default:
               return;
         }
      }

      public void Update(King king, GameTime gameTime)
      {
         king.animationTimer += gameTime.ElapsedGameTime;

         if (king.animationTimer.TotalMilliseconds > 150 * 6)
         {
            king.animationTimer = new TimeSpan(0);
         }
         king.currentFrame = (int)Math.Floor(king.animationTimer.TotalMilliseconds / 150) + 8;
      }
   }
}
