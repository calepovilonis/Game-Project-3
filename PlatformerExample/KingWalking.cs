using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   class KingWalking : KingState
   {
      public KingWalking() { }

      public void HandleInput(King king, KeyboardState state)
      {
         if (state.GetPressedKeys().Count() == 0)
         {
            king.state = king.idle;
            return;
         }

         var key = state.GetPressedKeys()[0];
         switch (key)
         {
            case Keys.W:
               if (king.Bounds.Y > 0) king.Position.Y -= 3;
               //else king.Position.Y = 0;
               break;
            case Keys.A:
               king.spriteEffects = SpriteEffects.FlipHorizontally;
               if (king.Bounds.X > -30) king.Position.X -= 3;
               //else king.Position.X = 30;
               break;
            case Keys.S:
               if (king.Bounds.Y + king.Bounds.Height < 690) king.Position.Y += 3;
               //else king.Position.Y = 742;
               break;
            case Keys.D:
               king.spriteEffects = SpriteEffects.None;
               if (king.Bounds.X + king.Bounds.Width < 1036) king.Position.X += 3;
               //else king.Position.X = 1042;
               break;
            default:
               return;
         }
      }

      public void Update(King king, GameTime gameTime)
      {
         king.animationTimer += gameTime.ElapsedGameTime;

         if (king.animationTimer.TotalMilliseconds > 150 * 8)
         {
            king.animationTimer = new TimeSpan(0);
         }
         king.currentFrame = (int)Math.Floor(king.animationTimer.TotalMilliseconds / 150);
      }
   }
}
