using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MagicWorld.HUDClasses
{
    /// <summary>
    /// The HUD as proposed by MArian in the game design document.
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
        Texture2D bottleTex;
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
            resolution = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            content = Game.Content;
            visible = false;
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            manaBar = new ManaBar(position);
            ingredientBar = new IngredientBar(new Vector2(resolution.X / 2, 0));
            spellBarLeft = new SpellBar(new Vector2(resolution.X / 0.75f, 0));
            spellBarLeft.Width = 100;
            spellBarRight = new SpellBar(new Vector2(resolution.X / 0.75f + spellBarLeft.Width, 0));
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
                spriteBatch.Draw(bottleTex, manaBar.Position, Color.White);
                spriteBatch.Draw(liquidTex, manaBar.Filling, Color.White);
                spriteBatch.DrawString(font, ingredientBar.IngredientString, ingredientBar.Position, Color.White);
                spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
