using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerExample
{
   public class Camera
   {
      public Camera()
      {
         Zoom = 1.00f;
      }

      // Centered Position of the Camera in pixels.
      public Vector2 Position { get; private set; }
      // Current Zoom level with 1.0f being standard
      public float Zoom { get; private set; }
      // Current Rotation amount with 0.0f being standard orientation
      public float Rotation { get; private set; }

      // Height and width of the viewport window which we need to adjust
      // any time the player resizes the game window.
      public int ViewportWidth { get; set; }
      public int ViewportHeight { get; set; }

      private IEntity focus;

      // Center of the Viewport which does not account for scale
      public Vector2 ViewportCenter
      {
         get
         {
            return new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
         }
      }

      // Create a matrix for the camera to offset everything we draw,
      // the map and our objects. since the camera coordinates are where
      // the camera is, we offset everything by the negative of that to simulate
      // a camera moving. We also cast to integers to avoid filtering artifacts.
      public Matrix TranslationMatrix
      {
         get
         {
            return Matrix.CreateTranslation(-(int)Position.X,
               -(int)Position.Y, 0) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
         }
      }

      // Call this method with negative values to zoom out
      // or positive values to zoom in. It looks at the current zoom
      // and adjusts it by the specified amount. If we were at a 1.0f
      // zoom level and specified -0.5f amount it would leave us with
      // 1.0f - 0.5f = 0.5f so everything would be drawn at half size.
      public void AdjustZoom(float amount)
      {
         Zoom += amount;
         if (Zoom < 1f)
         {
            Zoom = 1f;
         }
         if (Zoom > 3f)
         {
            Zoom = 3f;
         }
      }

      // Move the camera in an X and Y amount based on the cameraMovement param.
      // if clampToMap is true the camera will try not to pan outside of the
      // bounds of the map.
      public void MoveCamera(Vector2 cameraMovement, IEntity cell, bool clampToMap = false)
      {
         Vector2 newPosition = Position + cameraMovement;

         if (clampToMap)
         {
            Position = MapClampedPosition(newPosition, 742, 1042, (int)cell.Bounds.Height, (int)cell.Bounds.Width);
         }
         else
         {
            Position = newPosition;
         }
      }

      public Rectangle ViewportWorldBoundry()
      {
         Vector2 viewPortCorner = ScreenToWorld(new Vector2(0, 0));
         Vector2 viewPortBottomCorner =
            ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

         return new Rectangle((int)viewPortCorner.X,
            (int)viewPortCorner.Y,
            (int)(viewPortBottomCorner.X - viewPortCorner.X),
            (int)(viewPortBottomCorner.Y - viewPortCorner.Y));
      }

      // Center the camera on specific pixel coordinates
      public void CenterOn(Vector2 position)
      {
         Position = position;
      }

      // Center the camera on a specific cell in the map
      public void CenterOn(IEntity cell)
      {
         Position = CenteredPosition(cell, (int)cell.Bounds.Height, (int)cell.Bounds.Width, true);
      }

      private Vector2 CenteredPosition(IEntity cell, int SpriteHeight, int SpriteWidth, bool clampToMap = false)
      {
         var cameraPosition = new Vector2(cell.getPos().X * SpriteWidth,
            cell.getPos().Y * SpriteHeight);
         var cameraCenteredOnTilePosition =
            new Vector2(cameraPosition.X + SpriteWidth / 2,
                cameraPosition.Y + SpriteHeight / 2);
         if (clampToMap)
         {
            return MapClampedPosition(cameraCenteredOnTilePosition, 742, 1042, SpriteHeight, SpriteWidth);
         }

         return cameraCenteredOnTilePosition;
      }

      // Clamp the camera so it never leaves the visible area of the map.
      private Vector2 MapClampedPosition(Vector2 position, int MapHeight, int MapWidth, int SpriteHeight, int SpriteWidth)
      {
         var cameraMax = new Vector2(MapWidth * SpriteWidth -
             (ViewportWidth / Zoom / 2),
             MapHeight * SpriteHeight -
             (ViewportHeight / Zoom / 2));

         return Vector2.Clamp(position,
            new Vector2(ViewportWidth / Zoom / 2, ViewportHeight / Zoom / 2),
            cameraMax);
      }

      public Vector2 WorldToScreen(Vector2 worldPosition)
      {
         return Vector2.Transform(worldPosition, TranslationMatrix);
      }

      public Vector2 ScreenToWorld(Vector2 screenPosition)
      {
         return Vector2.Transform(screenPosition,
             Matrix.Invert(TranslationMatrix));
      }

      // Move the camera's position based on input
      public void HandleInput(IEntity cell)
      {
         Vector2 cameraMovement = Vector2.Zero;

         cameraMovement.X = cell.Bounds.X;
         cameraMovement.Y = cell.Bounds.Y;

         // When using a controller, to match the thumbstick behavior,
         // we need to normalize non-zero vectors in case the user
         // is pressing a diagonal direction.
         if (cameraMovement != Vector2.Zero)
         {
            cameraMovement.Normalize();
         }

         // scale our movement to move 25 pixels per second
         cameraMovement *= 25f;

         MoveCamera(cameraMovement, cell, true);
      }

      public void Reset()
      {
         CenterOn(new Vector2(ViewportWidth / 2, ViewportHeight / 2));
         Zoom = 1f;
      }

      public void Update(List<IEntity> units, KeyboardState state)
      {
         var keys = state.GetPressedKeys();

         if (keys.Length == 0)
         {
            if (focus == null) return;

            HandleInput(focus);
            CenterOn(new Vector2((focus.Bounds.X + focus.Bounds.Width / 2), (focus.Bounds.Y + focus.Bounds.Height / 2)));
            return;
         }

         if (keys[0] == Keys.R) return;

         if (keys[0] == Keys.OemMinus)
         {
            AdjustZoom(-.1f);
            return;
         }
         if (keys[0] == Keys.OemPlus)
         {
            AdjustZoom(.1f);
            return;
         }
         if (keys[0] == Keys.C)
         {
            Reset();
            focus = null;
            return;
         }

         switch (keys[0])
         {
            case Keys.D1:
               focus = units[0];
               break;
            case Keys.D2:
               focus = units[1];
               break;
            case Keys.D3:
               focus = units[2];
               break;
            case Keys.D4:
               focus = units[3];
               break;
            case Keys.D5:
               focus = units[4];
               break;
            case Keys.D6:
               focus = units[5];
               break;
            case Keys.D7:
               focus = units[6];
               break;
            case Keys.D8:
               focus = units[7];
               break;
            default:
               return;
         }
         HandleInput(focus);
         CenterOn(new Vector2((focus.Bounds.X + focus.Bounds.Width / 2), (focus.Bounds.Y + focus.Bounds.Height / 2)));
      }
   }
}
