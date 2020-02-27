using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformerExample
{
   /// <summary>
   /// This is the main type for your game.
   /// </summary>
   public class Game1 : Game
   {


      GraphicsDeviceManager graphics;
      SpriteBatch spriteBatch;
      SpriteSheet knight_sheet;
      SpriteSheet mage_sheet;
      SpriteSheet swordsman_sheet;
      SpriteSheet king_sheet;
      Texture2D background;
      Sprite temp;
      List<IEntity> allies;
      List<IEntity> enemies;
      List<IEntity> all;
      Random random = new Random();
      int clickDur = 0;
      Grid grid;
      bool startGame = false;
      bool endGame = false;
      private SpriteFont details;
      Camera camera = new Camera();
      King king;

      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
         this.IsMouseVisible = true;
         Content.RootDirectory = "Content";
         allies = new List<IEntity>();
         enemies = new List<IEntity>();
         all = new List<IEntity>();
         grid = new Grid(1042, 1);
      }

      public void GenerateEnemies()
      {
         for (int i = 1; i < 5; i++)
         {
            int val = random.Next(1, 4);
            switch (val)
            {
               case 1:
                  var knight = new Knight(knight_sheet.Sprites, i * 200, 100 + (int)(random.NextDouble() * 50), grid);
                  grid.Add(knight);
                  enemies.Add(knight);
                  break;
               case 2:
                  var mage = new Mage(mage_sheet.Sprites, i * 200, 100 + (int)(random.NextDouble() * 50), grid);
                  grid.Add(mage);
                  enemies.Add(mage);
                  break;
               case 3:
                  var sword = new Swordsman(swordsman_sheet.Sprites, i * 200, 100 + (int)(random.NextDouble() * 50), grid);
                  grid.Add(sword);
                  enemies.Add(sword);
                  break;
            }
         }
      }
      public void GenerateAllies()
      {
         for (int i = 1; i < 5; i++)
         {
            int val = random.Next(1, 4);
            switch (val)
            {
               case 1:
                  var knight = new Knight(knight_sheet.Sprites, i * 200, 450, grid);
                  grid.Add(knight);
                  allies.Add(knight);
                  break;
               case 2:
                  var mage = new Mage(mage_sheet.Sprites, i * 200, 450, grid);
                  grid.Add(mage);
                  allies.Add(mage);
                  break;
               case 3:
                  var sword = new Swordsman(swordsman_sheet.Sprites, i * 200, 450, grid);
                  grid.Add(sword);
                  allies.Add(sword);
                  break;
            }
         }
      }
      public void GenerateTemps()
      {
         for (int i = 1; i < 5; i++)
         {
            if (i == 1 || i == 4) allies.Add(new TempUnit(temp, i * 200, 450));
            else allies.Add(new TempUnit(temp, i * 200, 525));

         }
      }

      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override void Initialize()
      {
         // TODO: Add your initialization logic here
         graphics.PreferredBackBufferWidth = 1042;
         graphics.PreferredBackBufferHeight = 742;
         graphics.ApplyChanges();
         camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
         camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;

         base.Initialize();
      }

      /// <summary>
      /// LoadContent will be called once per game and is the place to load
      /// all of your content.
      /// </summary>
      protected override void LoadContent()
      {
#if VISUAL_DEBUG
         VisualDebugging.LoadContent(Content);
#endif
         // Create a new SpriteBatch, which can be used to draw textures.
         spriteBatch = new SpriteBatch(GraphicsDevice);

         var x = Content.Load<Texture2D>("knight_sheet");
         knight_sheet = new SpriteSheet(x, 42, 42, 0, 0);
         var y = Content.Load<Texture2D>("sword_sheet");
         swordsman_sheet = new SpriteSheet(y, 184, 137, 0, 0);
         var z = Content.Load<Texture2D>("wizard_sheet");
         mage_sheet = new SpriteSheet(z, 231, 190, 0, 0);
         var k = Content.Load<Texture2D>("king_sheet");
         king_sheet = new SpriteSheet(k, 155, 155, 0, 0);

         king = new King(king_sheet.Sprites, 400, 400, new KingIdle(), new KingWalking());

         var t = Content.Load<Texture2D>("temp");
         temp = new SpriteSheet(t, 50, 75, 0, 0).Sprites[0];

         background = Content.Load<Texture2D>("background");

         details = Content.Load<SpriteFont>("Text");

         camera.Reset();

         GenerateEnemies();
         foreach (IEntity e in enemies)
         {
            e.Ally = enemies;
            all.Add(e);
         }
         GenerateAllies();
         foreach (IEntity e in allies)
         {
            e.Ally = allies;
            all.Add(e);
         }

      }

      /// <summary>
      /// UnloadContent will be called once per game and is the place to unload
      /// game-specific content.
      /// </summary>
      protected override void UnloadContent()
      {
         // TODO: Unload any non ContentManager content here
      }

      /// <summary>
      /// Allows the game to run logic such as updating the world,
      /// checking for collisions, gathering input, and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Update(GameTime gameTime)
      {
         if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

         if (Keyboard.GetState().IsKeyDown(Keys.R))
         {
            startGame = true;
         }

         camera.Update(all, Keyboard.GetState());

         king.HandleInput(Keyboard.GetState());
         king.Update(gameTime);

         for (int i = 0; i < allies.Count; i++)
         {
            allies[i].Update(gameTime, startGame);
         }
         for (int i = 0; i < enemies.Count; i++)
         {
            enemies[i].Update(gameTime, startGame);
         }

         endGame = isWon();

         base.Update(gameTime);
      }

      private bool isWon()
      {
         bool allyDead = true;
         foreach(IEntity e in allies)
         {
            if (e.Health > 0) allyDead = false;
         }
         bool enemyDead = true;
         foreach (IEntity e in enemies)
         {
            if (e.Health > 0) enemyDead = false;
         }

         if ((allyDead == true && enemyDead == false) || (enemyDead == true && allyDead == false)) return true;
         else return false;
      }

      /// <summary>
      /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.Black);

         // TODO: Add your drawing code here
         spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.TranslationMatrix);

         spriteBatch.Draw(background, new Rectangle(0, 0, 1042, 742), Color.White);

         enemies.ForEach(IEntity =>
         {
            IEntity.Draw(spriteBatch);
         });

         king.Draw(spriteBatch);

         allies.ForEach(IEntity =>
         {
            IEntity.Draw(spriteBatch);
         });

         if (startGame == false) spriteBatch.DrawString(details, "Press 'R' to begin Game.", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 125, graphics.GraphicsDevice.Viewport.Height / 2 - 40), Color.ForestGreen);
         else
         {
            if (endGame == true)
            {
               bool winner = false;
               foreach (IEntity e in enemies)
               {
                  if (e.Health > 0) winner = true;
               }
               if (winner) spriteBatch.DrawString(details, "Enemies Win!", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 55, graphics.GraphicsDevice.Viewport.Height / 2 - 40), Color.ForestGreen);
               else spriteBatch.DrawString(details, "Allies Win!", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50, graphics.GraphicsDevice.Viewport.Height / 2 - 40), Color.ForestGreen);
            }
         }


         spriteBatch.End();

         base.Draw(gameTime);
      }
   }
}
