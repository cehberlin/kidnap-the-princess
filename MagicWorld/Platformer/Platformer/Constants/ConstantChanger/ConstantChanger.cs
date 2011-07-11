using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Services;
using MagicWorld.Constants;
using Microsoft.Xna.Framework.Input;
using MagicWorld.Controls;
using System.Xml.Serialization;
using System.IO;

namespace MagicWorld.Constants
{
    /// <summary>
    /// this gamecomponet displays some  groups of changeable values in upper left corner and you are
    /// able to navigate in this values and change them
    /// </summary>
    class ConstantChanger : DrawableGameComponent
    {
        const float transparencyFactor = 0.65f;

        ContentManager content;
        SpriteBatch spriteBatch;

        SpriteFont font;

        IPlayerService playerService;

        Vector2 positionGroupName;
        Vector2 positionValue;

        public List<ConstantGroup> Constants = new List<ConstantGroup>();

        InputState input = new InputState();

        int currentGroupIdx = 0;

        int currentGroupItemIdx = 0;

        /// <summary>
        /// The resolution of the game.
        /// </summary>
        Vector2 resolution;

        public ConstantChanger(Game game)
            : base(game)
        {
            content = game.Content;
            positionGroupName.X = 60;
            positionValue.X = positionGroupName.X;

            Constants.AddRange(SpellConstantsValues.getChangeableConstants());
            Constants.AddRange(PhysicValues.getChangeableConstants());
            Constants.AddRange(SpellInfluenceValues.getChangeableConstants());

            playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
            refreshPosition();
        }

        ~ConstantChanger()
        {
            //TODO save current configuration I dont know why the destructor is not called
            XmlSerializer ser = new XmlSerializer(typeof(ConstantChanger));
            FileStream str = new FileStream(@"currentChangeableConfiguration.xml", FileMode.Create);
            ser.Serialize(str, this);
            str.Close();
        }

        private void refreshPosition()
        {
            Vector2 newResolution = new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);

            if (newResolution != resolution)
            {
                resolution = newResolution;
                positionGroupName = new Vector2(positionGroupName.X, resolution.Y - 60);
                positionValue = positionGroupName + new Vector2(0, 25);
            }
        }

        protected override void LoadContent()
        {
            font = content.Load<SpriteFont>("Fonts/Hud");

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            base.LoadContent();
        }


        void HandleInput(KeyboardState keyboardState,KeyboardState oldKeyboardState)
        {
            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_CONSTANT_NEXTGROUP) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_CONSTANT_NEXTGROUP))
            {
                currentGroupIdx++;
                currentGroupIdx = currentGroupIdx % Constants.Count;
                currentGroupItemIdx = 0;//reset for next group
            }

            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_CONSTANT_NEXTGROUPITEM) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_CONSTANT_NEXTGROUPITEM))
            {
                currentGroupItemIdx++;
                currentGroupItemIdx = currentGroupItemIdx % Constants[currentGroupIdx].ConstantValues.Count;
            }

            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_CONSTANT_ITEM_SWITCH_INTERNAL) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_CONSTANT_ITEM_SWITCH_INTERNAL))
            {
                Constants[currentGroupIdx].ConstantValues[currentGroupItemIdx].switchInternalValues();
            }

            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_CONSTANT_ITEM_RESET) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_CONSTANT_ITEM_RESET))
            {
                Constants[currentGroupIdx].ConstantValues[currentGroupItemIdx].resetToStart();
            }

            if (keyboardState.IsKeyDown(GameOptionsControls.DEBUG_CONSTANT_ITEM_INCREASE))
            {
                Constants[currentGroupIdx].ConstantValues[currentGroupItemIdx].Increment();
            }

            if (keyboardState.IsKeyDown(GameOptionsControls.DEBUG_CONSTANT_ITEM_DECREASE))
            {
                Constants[currentGroupIdx].ConstantValues[currentGroupItemIdx].Decrement();
            }
        }

        public override void Update(GameTime gameTime)
        {
            input.Update();
            if(Constants.Count>0){
                refreshPosition();
                HandleInput(input.CurrentKeyboardStates[0],input.LastKeyboardStates[0]);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {            
            if (playerService.IsAlive && Constants.Count>0)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                spriteBatch.DrawString(font, Constants[currentGroupIdx].groupName, positionGroupName, Color.Red * transparencyFactor);

                Constants[currentGroupIdx].ConstantValues[currentGroupItemIdx].Draw(spriteBatch, positionValue, font,Color.White*transparencyFactor);

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// one group of changeable constants
    /// </summary>
    class ConstantGroup
    {
        public string groupName;
        public List<IConstantChangerItem> ConstantValues;

        public ConstantGroup(string groupName, List<IConstantChangerItem> ConstantValues)
        {
            this.groupName = groupName;
            this.ConstantValues = ConstantValues;
        }
    }
}
