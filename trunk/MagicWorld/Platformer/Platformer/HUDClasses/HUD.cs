using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MagicWorld.Services;

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
        /// The texture for the current left spell.
        /// </summary>
        Texture2D leftSpell;
        /// <summary>
        /// The texture for the current right spell.
        /// </summary>
        Texture2D rightSpell;
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
        IPlayerService playerService;
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
            spellBarLeft = new SpellBar(new Vector2(resolution.X * 0.75f, position.Y));
            spellBarLeft.Width = 100;
            spellBarRight = new SpellBar(new Vector2(resolution.X * 0.75f + spellBarLeft.Width, position.Y));
        }

        protected override void LoadContent()
        {
            bottleTex = content.Load<Texture2D>("HUDTextures/bottle");
            liquidTex = content.Load<Texture2D>("HUDTextures/liquid");
            antigrav = content.Load<Texture2D>("SpellRunes/antigrav");
            electricity = content.Load<Texture2D>("SpellRunes/electricty");
            heat = content.Load<Texture2D>("SpellRunes/heat");
            cold = content.Load<Texture2D>("SpellRunes/cold");
            pull = content.Load<Texture2D>("SpellRunes/pull");
            push = content.Load<Texture2D>("SpellRunes/push");
            matter = content.Load<Texture2D>("SpellRunes/matter");
            wind = content.Load<Texture2D>("SpellRunes/wind");
            water = content.Load<Texture2D>("SpellRunes/water");
            font = content.Load<SpriteFont>("Fonts/Hud");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            manaBar.Filling = new Rectangle((int)position.X + 3, (int)position.Y + 30, bottleTex.Width - 6, bottleTex.Height - 30);
            manaBar.fullY = manaBar.Filling.Y;
            manaBar.fullHeight = manaBar.Filling.Height;
            manaBar.Height = bottleTex.Height;
            manaBar.Width = bottleTex.Width;

            leftSpell = cold;
            rightSpell = heat;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
            if (visible)
            {
                if ( playerService!= null)
                {
                    //TODO: remove 1000 and get the value dynamically
                    manaBar.Update(playerService.Mana.CurrentMana, 1000);
                    //TODO: Constant polling is not good, call back is better.
                    spellBarLeft.Update(playerService.selectedSpell_A);
                    spellBarRight.Update(playerService.selectedSpell_B);
                    UpdateRunes(playerService.selectedSpell_A, ref leftSpell);
                    UpdateRunes(playerService.selectedSpell_B, ref rightSpell);
                }

                Level level = (Level)Game.Services.GetService(typeof(Level)); 
                if (level != null)
                {
                    ingredientBar.SetState(level.CollectedIngredients.Count, level.NeededIngredients, level.MaxIngredientsCount);
                }

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
                spriteBatch.Draw(leftSpell, spellBarLeft.Position, Color.White);
                spriteBatch.Draw(rightSpell, spellBarRight.Position, Color.White);
                spriteBatch.End();
                base.Draw(gameTime);
            }
        }

        private void UpdateRunes(Spells.SpellType s, ref Texture2D tex)
        {
            switch (s)
            {
                case Spells.SpellType.ColdSpell:
                    tex = cold;
                    break;
                case Spells.SpellType.CreateMatterSpell:
                    tex = matter;
                    break;
                case Spells.SpellType.NoGravitySpell:
                    tex = antigrav;
                    break;
                case Spells.SpellType.WarmingSpell:
                    tex = heat;
                    break;
                case Spells.SpellType.ElectricSpell:
                    tex = electricity;
                    break;
                case Spells.SpellType.PushSpell:
                    tex = push;
                    break;
                case Spells.SpellType.PullSpell:
                    tex = pull;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
