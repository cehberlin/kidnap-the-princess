#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
#endregion

namespace MagicWorld
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class LoadGameScreen : MenuScreen
    {
        #region Fields

        MenuEntry mnuLevel;
        MenuEntry back;
        string[] files;        
        int selectedFile = 0;
        SpriteFont font;
   
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public LoadGameScreen(ScreenManager screenManager)
            : base("Load Game")            
        {
            ScreenManager = screenManager;
            font = ScreenManager.Font;
            // Create our menu entries.
            mnuLevel = new MenuEntry("Level");            
            back = new MenuEntry("Back");

            // Hook up menu event handlers.
            mnuLevel.Selected += PlayGameMenuEntrySelected;
            
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(mnuLevel);            
            MenuEntries.Add(back);
            LoadFiles();
            
        }

        #endregion

        #region statistics

        private void ShowString(string info,Vector2  pos)
        {            
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;            
            
            Vector2 titleOrigin = Vector2.Zero;
            Color titleColor = Color.Black;
            float titleScale = 0.7f;

            spriteBatch.Begin();

            Text.DrawOutlinedText(spriteBatch, font, info, pos, titleColor, titleOrigin, titleScale);
        
            spriteBatch.End();
        }

        private void ShowGameInfo()
        {
            string tempString = "Items available:     ";
            Vector2 pos;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(tempString);

            int distance =(int) textSize.Y+5;            

            pos = new Vector2(back.Position.X, back.Position.Y);
            pos.X = viewportSize.X/6;
            pos.Y += distance;
            //pos.X = 40;
            ShowString("Last Game",pos);
            pos.Y += distance;
            ShowString("Level " + ScreenManager.Game.GameData.Completed, pos);
            pos.Y += distance;
            ShowString("Elapsed Time: " + ScreenManager.Game.GameData.Time.ToString("#0.0") + "s", pos);
            pos.Y += distance;
            ShowString("Items available: " + ScreenManager.Game.GameData.TotalItems.ToString() , pos);
            pos.Y += distance;
            ShowString("Items collected: " + ScreenManager.Game.GameData.ItemsCollected.ToString(), pos);

            //best performance
            pos.Y = back.Position.Y;
            pos.Y += distance;
            pos.X = 4*viewportSize.X / 6; 
            ShowString("Best Game", pos);
            pos.Y += distance;
            ShowString("Level " + ScreenManager.Game.BestGameData.Completed, pos);
            pos.Y += distance;
            ShowString("Elapsed Time: " + ScreenManager.Game.BestGameData.Time.ToString("#0.0") + "s", pos);
            pos.Y += distance;
            ShowString("Items available: " + ScreenManager.Game.BestGameData.TotalItems.ToString(), pos);
            pos.Y += distance;
            ShowString("Items collected: " + ScreenManager.Game.BestGameData.ItemsCollected.ToString(), pos);


        }

        #endregion

        #region load files
        /// <summary>
        /// Load saved files name to a list
        /// </summary>
        private void LoadFiles()
        {
            files = ScreenManager.Game.GetSavedFiles(false);
            if (files.Length > 0)
            {                
                ScreenManager.Game.LoadGame(1,false);
                ScreenManager.Game.LoadGame(1, true);
            }

        }
        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);
            DrawLevelEntry();
            ShowGameInfo();
            base.Draw(gameTime);
        }

        private void DrawLevelEntry()
        {           

            mnuLevel.Text = "Level    < " + (selectedFile + 1).ToString() + " >";
            return;
            
        }
        #endregion

        #region Handle Input

        public override void HandleInput(InputState input)
        {
            
            
            //check the selection of rigth and left when on "New game" menu
            if (input.IsMenuRight(ControllingPlayer))
            {
                if (SelectedEntry == 0) //New game
                {
                    if (files.Length > 0)
                    {
                        selectedFile++;                        
                        if (selectedFile > (files.Length - 1))
                        {
                            selectedFile = 0;
                        }
                        ScreenManager.Game.LoadGame(selectedFile + 1,false);
                        //best game
                        ScreenManager.Game.LoadGame(selectedFile + 1, true);
                       
                    }

                }
            }

            //check the selection of rigth and left when on "New game" menu
            if (input.IsMenuLeft(ControllingPlayer))
            {
                if (SelectedEntry == 0) //New game
                {
                    if (files.Length > 0)
                    {
                        selectedFile--;
                        if (selectedFile < 0)
                        {
                            selectedFile = files.Length - 1;
                        }
                        ScreenManager.Game.LoadGame(selectedFile + 1,false);
                        //best game
                        ScreenManager.Game.LoadGame(selectedFile + 1, true);
                    }
                }
            }

            base.HandleInput(input);
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            GameplayScreen gameScreen = new GameplayScreen(ScreenManager);
            //gameScreen.ScreenManager = ScreenManager;
            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, gameScreen);
            gameScreen.LoadContent();
            ScreenManager.AddScreen(gameScreen, e.PlayerIndex);  
            gameScreen.LoadLevel(selectedFile + 1);//selectedfile is always one number less levelnumber 
             
            ScreenManager.Game.ResetElapsedTime();
        }        

  
        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            bool a;
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                if (screen.GetType().Equals(typeof(MainMenuScreen)))
                    a=screen.IsActive;
            }
            ExitScreen();
        }

        #endregion
    }
}
