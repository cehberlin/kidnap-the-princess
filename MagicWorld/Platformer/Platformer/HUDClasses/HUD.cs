using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MagicWorld.HUDClasses
{
    /// <summary>
    /// The HUD as proposed by Marian in the game design document.
    /// </summary>
    class HUD : DrawableGameComponent
    {
        #region Declarations
        /// <summary>
        /// The ScreenManager of the game.
        /// </summary>
        ScreenManager screenManager;
        /// <summary>
        /// Used for drawing textures and strings.
        /// </summary>
        SpriteBatch spriteBatch;
        /// <summary>
        /// The resolution of the game.
        /// </summary>
        Vector2 resolution;
        /// <summary>
        /// The position of the hud.
        /// </summary>
        Vector2 position;
        /// <summary>
        /// The left spell bar.
        /// </summary>
        SpellBar spellBarLeft;
        /// <summary>
        /// The right spell bar.
        /// </summary>
        SpellBar spellBarRight;
        /// <summary>
        /// Ingredients that are in the level, that we collected and must be collected.
        /// </summary>
        IngredientBar ingredientBar;
        /// <summary>
        /// Used to track the players current and max mana.
        /// </summary>
        ManaBar manaBar;
        /// <summary>
        /// Font used to write in the hud.
        /// </summary>
        SpriteFont font;
        /// <summary>
        /// Texture representing the max mana.
        /// </summary>
        Texture2D bottleTex;//TODO: Draw a better looking bottle.
        /// <summary>
        /// Texture representing the current mana.
        /// </summary>
        Texture2D liquidTex;
        //Rune Textures
        Texture2D antigrav;
        Texture2D electricity;
        Texture2D heat;
        Texture2D cold;
        Texture2D pull;
        Texture2D push;
        Texture2D matter;
        Texture2D wind;
        Texture2D water;
        /// <summary>
        /// True if the gameplay screen is active and in the foreground. 
        /// If false hud is neither updated nor drawn.
        /// </summary>
        bool visible;
        /// <summary>
        /// Used for loading the hud textures.
        /// </summary>
        ContentManager content;
        #endregion

        public HUD(Game game)
            : base(game)
        {
            position = new Vector2(10, 10);
            resolution = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            content = Game.Content;
            visible = false;
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            manaBar = new ManaBar(position);
            ingredientBar = new IngredientBar(new Vector2(resolution.X / 2, position.Y));
            spellBarLeft = new SpellBar(new Vector2(resolution.X / 0.75f, position.Y));
            spellBarLeft.Width = 100;
            spellBarRight = new SpellBar(new Vector2(resolution.X / 0.75f + spellBarLeft.Width, position.Y));
        }

        protected override void LoadContent()
        {
            bottleTex = content.Load<Texture2D>("Content/HUDTextures/bottle");
            liquidTex = content.Load<Texture2D>("Content/HUDTextures/liquid");
            antigrav = content.Load<Texture2D>("Content/SpellRunes/antigrav");
            electricity = content.Load<Texture2D>("Content/SpellRunes/electricty");
            heat = content.Load<Texture2D>("Content/SpellRunes/heat");
            cold = content.Load<Texture2D>("Content/SpellRunes/cold");
            pull = content.Load<Texture2D>("Content/SpellRunes/pull");
            push = content.Load<Texture2D>("Content/SpellRunes/push");
            matter = content.Load<Texture2D>("Content/SpellRunes/matter");
            wind = content.Load<Texture2D>("Content/SpellRunes/wind");
            water = content.Load<Texture2D>("Content/SpellRunes/water");
            font = content.Load<SpriteFont>("Content/Fonts/Hud");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            manaBar.Filling = new Rectangle((int)position.X+3, (int)position.Y+30, bottleTex.Width-6, bottleTex.Height-30);
            manaBar.Height = bottleTex.Height;
            manaBar.Width = bottleTex.Width;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (visible)
            {
                //manabar.filling
                //ingredients
                //spells
                if (!screenManager.IsGameplayScreenActive())
                {
                    visible = false;
                }
                base.Update(gameTime);
            }
            else
            {
                if (screenManager.IsGameplayScreenActive())
                {
                    visible = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (visible)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(liquidTex, manaBar.Filling, Color.White);
                spriteBatch.Draw(bottleTex, manaBar.Position, Color.White);
                spriteBatch.DrawString(font, ingredientBar.IngredientString, ingredientBar.Position, Color.White);
                spriteBatch.End();
                base.Draw(gameTime);
            }
        }


    }
}
