using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KidnapThePrincess
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldState;
        Level level;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 700;

            level = new Level();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the level
            level.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            GetInput();
            level.Update();
            if (level.Heroes[0].IsActive) this.Exit();//The goblin turns active when he has reached the carriage->Level complete

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                level.Camera.get_transformation(graphics.GraphicsDevice));
            level.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Grabs and processes the user input.
        /// </summary>
        private void GetInput()
        {
            // Grab some info from the keyboard
            KeyboardState keyboard = Keyboard.GetState();
            //Player One Input
            if (keyboard.IsKeyDown(Keys.Right))
            {
                level.MoveHeroRight(0);
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                level.MoveHeroLeft(0);
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                level.MoveHeroUp(0);
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                level.MoveHeroDown(0);
            }
            if (keyboard.IsKeyUp(Keys.I)&&oldState.IsKeyDown(Keys.I))
            {
                level.SwitchHero(0);
            }
            //Player Two Input
            if (keyboard.IsKeyDown(Keys.D))
            {
                level.MoveHeroRight(1);
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                level.MoveHeroLeft(1);
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                level.MoveHeroUp(1);
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                level.MoveHeroDown(1);
            }
            if (keyboard.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space))
            {
                level.SwitchHero(1);
            }

            oldState = keyboard;
        }
    }
}
