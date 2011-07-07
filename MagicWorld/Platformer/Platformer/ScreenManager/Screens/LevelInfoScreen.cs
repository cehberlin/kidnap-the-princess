#region Using Statements
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Spells;
using System.Collections.Generic;
using MagicWorld.HelperClasses;
#endregion

namespace MagicWorld
{

    class SpellsInfo
    {
        public Vector2 pos;
        public SpellType spell;
        public Texture2D texture;
    }
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class LevelInfoScreen : GameScreen
    {
        #region Fields

        string message;
        Texture2D gradientTexture;

        Texture2D antigrav;
        Texture2D electricity;
        Texture2D heat;
        Texture2D cold;
        Texture2D pull;
        Texture2D push;
        Texture2D matter;
        Texture2D wind;
        Texture2D water;

        List<SpellsInfo> levelSpells=new List<SpellsInfo>();

        
        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        Level level;

        #endregion

        #region Initialization


        
        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public LevelInfoScreen(string message,Level level)
            
        {
            this.level = level;
            this.message = message;// +usageText;
 
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
            
            
            
        }

        

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>("MenuScreen/gradient");
            antigrav = content.Load<Texture2D>("SpellRunes/antigrav");
            electricity = content.Load<Texture2D>("SpellRunes/electricty");
            heat = content.Load<Texture2D>("SpellRunes/heat");
            cold = content.Load<Texture2D>("SpellRunes/cold");
            pull = content.Load<Texture2D>("SpellRunes/pull");
            push = content.Load<Texture2D>("SpellRunes/push");
            matter = content.Load<Texture2D>("SpellRunes/matter");
            wind = content.Load<Texture2D>("SpellRunes/wind");
            water = content.Load<Texture2D>("SpellRunes/water");
            LoadSpells();
            CalculateSpeelsPosition();
        }


        private void LoadSpells()
        {
            

            foreach (SpellType spell in level.LevelLoader.UsableSpells)
            {
                SpellsInfo spellsInfo = new SpellsInfo();
                spellsInfo.spell = spell;
                switch (spell)
                {
                    case SpellType.ColdSpell:
                        spellsInfo.texture = cold;
                        break;
                    case SpellType.CreateMatterSpell:
                        spellsInfo.texture = matter;
                        break;
                    case SpellType.ElectricSpell:
                        spellsInfo.texture = electricity;
                        break;
                    case SpellType.NoGravitySpell:
                        spellsInfo.texture = antigrav;
                        break;
                    case SpellType.PullSpell:
                        spellsInfo.texture = pull;
                        break;
                    case SpellType.PushSpell:
                        spellsInfo.texture = push;
                        break;
                    case SpellType.WarmingSpell:
                        spellsInfo.texture = heat;
                        break;
                }
                levelSpells.Add(spellsInfo);
            }
        }

        #endregion



        #region Handle Input


        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsContinuePress(ControllingPlayer,out playerIndex))
            {
                if (Accepted != null)
                {
                    ScreenManager.RemoveScreen(this);
                    Accepted(this, null);
                    
                }
            }
            
        }


        #endregion

        #region Draw


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            string levelName = "Level " + level.LevelNumber.ToString();
            string tempString = "You can use the spells";

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            
            


            // Fade the popup alpha during transitions.
            Color color = Color.Black * TransitionAlpha;

            spriteBatch.Begin();            

            // Draw the message box text.            
            //textPosition += vecIncrease*3;  
            Vector2 textSize = font.MeasureString(levelName);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            textPosition.Y -= 40;
            Text.DrawOutlinedText(spriteBatch, font, levelName, textPosition, color);

            textSize = font.MeasureString(tempString);
            textPosition = (viewportSize - textSize) / 2;
            Text.DrawOutlinedText(spriteBatch, font, tempString, textPosition, color);

            textPosition.Y += 60;            
            foreach (SpellsInfo spell in levelSpells)
            {
                spell.pos.Y = textPosition.Y;
                spriteBatch.Draw(spell.texture, spell.pos, Color.White);
            }
           
            spriteBatch.End();
        }

        void CalculateSpeelsPosition()
        {
            int textureSize = cold.Width+ 20;//assuming all are equal
            int i = levelSpells.Count;
            int allsize = textureSize * i;
            int startpos;
            //get the screen size
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            startpos =(int) viewportSize.X / 2 - allsize / 2;


            foreach (SpellsInfo spell in levelSpells)
            {
                spell.pos.X = startpos;
                startpos += textureSize;
            }
        }

        #endregion
    }
}
