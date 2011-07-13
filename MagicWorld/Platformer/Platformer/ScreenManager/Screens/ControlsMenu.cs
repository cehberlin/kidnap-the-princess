using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MagicWorld.Controls;
using MagicWorld.HelperClasses;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MagicWorld
{
    class ControlsMenu : MenuScreen
    {

        MenuEntry defaultConfig;
        MenuEntry laptopConfig;
        MenuEntry back;
        Texture2D gamePad;
        ContentManager content;
        
        public ControlsMenu() 
            : base("Controls")
        {
            //init
            defaultConfig = new MenuEntry("controls A");
            laptopConfig = new MenuEntry("controls B");
            back = new MenuEntry("Back");

            //add handler
            defaultConfig.Selected += defaultConfigSelected;
            laptopConfig.Selected += laptopConfigSelected;
            back.Selected +=OnCancel;

            //add to menu
            MenuEntries.Add(defaultConfig);
            MenuEntries.Add(laptopConfig);
            MenuEntries.Add(back);
            
        }

        public override void LoadContent()
        {            
            content = ScreenManager.ContentManager;
            gamePad = content.Load<Texture2D>("MenuScreen/gamepad");
           // ScreenManager.Game.IsMouseVisible = true;

        }

        void defaultConfigSelected(object sender, PlayerIndexEventArgs e)
        {
            //ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
            PlayerControlFactory.GET_INSTANCE().ChangeControl(ControlType.defaultControl);
            ScreenManager.Game.GameStatus.Control = "Default";
        }

        void laptopConfigSelected(object sender, PlayerIndexEventArgs e)
        {
            //ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
            PlayerControlFactory.GET_INSTANCE().ChangeControl(ControlType.laptopControl);
            ScreenManager.Game.GameStatus.Control = "Laptop";
        }


        private SpriteBatch spriteBatch;
        private Vector2 position;
        private SpriteFont font;

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            MouseState mouseStateCurrent = Mouse.GetState();

            spriteBatch = ScreenManager.SpriteBatch;
            font = ScreenManager.Font;
            Viewport viewPort = ScreenManager.GraphicsDevice.Viewport;
            position = new Vector2(viewPort.X/2 , laptopConfig.Position.Y+70);
            Vector2 v_jump=new Vector2(621,421);
            Vector2 v_up = new Vector2(10, 10);
            Vector2 v_inc = new Vector2(611, 342);
            Vector2 v_dec = new Vector2(612, 372);
            Vector2 v_move = new Vector2(358, 421);
            Vector2 v_cspa = new Vector2(625, 486);
            Vector2 v_cspb = new Vector2(335, 490);
            Vector2 v_sspa = new Vector2(619, 515);
            Vector2 v_sspb = new Vector2(335, 515);
            Vector2 v_pause = new Vector2(547, 334);
            Vector2 v_back = new Vector2(335, 515);





            IPlayerControl controls = PlayerControlFactory.GET_INSTANCE().getPlayerControl();

            spriteBatch.Begin();
            //gamepad
            float fscale = 0.5f;

            Text.DrawOutlinedText(ScreenManager.SpriteBatch, ScreenManager.Font, "GamePad", new Vector2(position.X + 430, position.Y), Color.Black, new Vector2(-50, 0), 0.7f);
            spriteBatch.Draw(gamePad, new Rectangle((int)position.X+400, (int)position.Y+50, gamePad.Bounds.Width, gamePad.Bounds.Height), Color.White);
            
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

            Text.DrawOutlinedText(ScreenManager.SpriteBatch, ScreenManager.Font, buttonType, position, Color.Black, new Vector2(-50, 0), 0.6f);
            Text.DrawOutlinedText(ScreenManager.SpriteBatch, ScreenManager.Font, keyboard, position, Color.Black, new Vector2(-330, 0), 0.6f);
           
            position.Y += 30;
        }

    }
}
