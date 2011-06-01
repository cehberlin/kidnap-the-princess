using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.HUD
{
    /// <summary>
    /// The game component representing the HUD.
    /// The HUD shows the mana bar, spell selector and collected items.
    /// </summary>
    class HUD : DrawableGameComponent
    {
        //TODO: Make the HUD beautiful, right now it's just a draft concentrating on functionality
        Vector2 position = new Vector2(10, 10);

        ManaBar manaBar;
        SpellSelector spellSelector;
        CollectedItems collectedItems;

        SpriteBatch spriteBatch;
        SpriteFont font;
        
        public HUD(Game game)
            : base(game)
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("dummyFont");
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            manaBar = new ManaBar(Game.Content.Load<Texture2D>("HUD\\manaTex"), position);
            collectedItems = new CollectedItems(manaBar.Position + new Vector2(0, manaBar.Height), Game.Content.Load<Texture2D>("Ingredients\\dummyCollectible"));
            spellSelector = new SpellSelector(collectedItems.Position + new Vector2(0, collectedItems.Height));
            //TODO: automate rune loading
            spellSelector.runes[0] = Game.Content.Load<Texture2D>("Runes\\antigrav");
            spellSelector.runes[1] = Game.Content.Load<Texture2D>("Runes\\cold");
            spellSelector.runes[2] = Game.Content.Load<Texture2D>("Runes\\electricity");
            spellSelector.runes[3] = Game.Content.Load<Texture2D>("Runes\\heat");
            spellSelector.runes[4] = Game.Content.Load<Texture2D>("Runes\\magnetic pull");
            spellSelector.runes[5] = Game.Content.Load<Texture2D>("Runes\\magnetic push");
            spellSelector.runes[6] = Game.Content.Load<Texture2D>("Runes\\matter");
            spellSelector.runes[7] = Game.Content.Load<Texture2D>("Runes\\water");
            spellSelector.runes[8] = Game.Content.Load<Texture2D>("Runes\\wind");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(manaBar.Texture, manaBar.EmptyManaRectangle, Color.Gray);
            spriteBatch.Draw(manaBar.Texture, manaBar.CurrentManaRectangle, Color.Blue);
            spriteBatch.DrawString(font, collectedItems.InfoString, collectedItems.InfoPosition, Color.White);
            spriteBatch.Draw(collectedItems.Texture, collectedItems.Position, Color.White);
            spriteBatch.Draw(spellSelector.runes[spellSelector.Index], spellSelector.Position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
