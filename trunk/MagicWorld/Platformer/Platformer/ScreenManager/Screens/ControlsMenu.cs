using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MagicWorld.Controls;

namespace MagicWorld
{
    class ControlsMenu : MenuScreen
    {

        MenuEntry defaultConfig;
        MenuEntry laptopConfig;  
        
        public ControlsMenu() 
            : base("Controls")
        {
            //init
            defaultConfig = new MenuEntry("Default Config");
            laptopConfig = new MenuEntry("LaptopControl");

            //add handler
            defaultConfig.Selected += defaultConfigSelected;
            laptopConfig.Selected += laptopConfigSelected;

            //add to menu
            MenuEntries.Add(defaultConfig);
            MenuEntries.Add(laptopConfig);
        }

        void defaultConfigSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
            PlayerControlFactory.GET_INSTANCE().ChangeControl(ControlType.defaultControl);
            ScreenManager.Game.GameStatus.Control = "Default";
        }

        void laptopConfigSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
            PlayerControlFactory.GET_INSTANCE().ChangeControl(ControlType.laptopControl);
            ScreenManager.Game.GameStatus.Control = "Laptop";
        }

        private SpriteBatch spriteBatch;
        private Vector2 position;
        private SpriteFont font;

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            spriteBatch = ScreenManager.SpriteBatch;
            font = ScreenManager.Font;
            position = new Vector2(100,250);

            IPlayerControl controls = PlayerControlFactory.GET_INSTANCE().getPlayerControl();

            spriteBatch.Begin();

            drawString("Action", "Keyboard", "Gamepad");
            drawString("Cast Spell A", controls.Keys_CastSelectedSpellA.ToString(), controls.GamePad_CastSelectedSpellA.ToString());
            drawString("Cast Spell B", controls.Keys_CastSelectedSpellB.ToString(), controls.GamePad_CastSelectedSpellB.ToString());
            drawString("Select Spell A", controls.Keys_SelectedSpellA.ToString(), controls.GamePad_SelectedSpellA.ToString());
            drawString("Select Spell B", controls.Keys_SelectedSpellB.ToString(), controls.GamePad_SelectedSpellB.ToString());

            drawString("jump", controls.Keys_Jump.ToString(), controls.GamePad_Jump.ToString());

            drawString("up", controls.Keys_Up.ToString(), controls.GamePad_Up.ToString());
            drawString("down", controls.Keys_Down.ToString(), controls.GamePad_Down.ToString());
            drawString("left", controls.Keys_Left.ToString(), controls.GamePad_Left.ToString());
            drawString("rigth", controls.Keys_Right.ToString(), controls.GamePad_Right.ToString());

            spriteBatch.End();
            base.Draw(gameTime);
        }

        
        private void drawString(String buttonType, String keyboard, String button)
        {
            String str = buttonType + ": " + keyboard + " / " + button;
            spriteBatch.DrawString(font, str, position, Color.Azure);
            position.Y += 25;
        }

    }
}
