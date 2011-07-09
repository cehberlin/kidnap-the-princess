using System;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using MagicWorld.HUDClasses;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent.ParticleEffects;
using ParticleEffects;
using MagicWorld.BlendInClasses;
using MagicWorld.Constants;
using MagicWorld.Services;
using MagicWorld.HelperClasses.Animation;
using MagicWorld.Controls;
using System.Collections.Generic;

namespace MagicWorld
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    [Serializable]
    public class SaveGameData
    {        
        public int Level;
        public int ItemsCollected;
        public int TotalItems;
        public string Completed;
        public float Time;
        public int TotalPoints;
    }

    [Serializable]
    public class SaveGameStatus
    {
        public bool PlayBackGroundMusic=false;
        public bool FullScreenMode=false;
        public Vector2 Resolution;
        public string Control;
    }

    public class MagicWorldGame : Microsoft.Xna.Framework.Game
    {

        #region properties
         
        ScreenManager screenManager;
        HUD hud;
        AimingAid aid;
        TutorialManager tutManager;
        IcedVisibility ice;
        SimpleAnimator simpleAnimator;
        Level level;

        public Level Level
        {
            get { return level; }
        }

        public SaveGameData GameData=new SaveGameData();
        public SaveGameData BestGameData = new SaveGameData();
        public SaveGameStatus GameStatus = new SaveGameStatus();

#if DEBUG
        ConstantChanger constantChanger;
        DikiLib.extensions.GARBAGE_COLLECTIONS_COUNTER gc_counter;
        DikiLib.extensions.FPS_COUNTER fps_counter;
#endif

        ParticleSystem explosionParticleSystem;
        public ParticleSystem ExplosionParticleSystem
        {
            get { return explosionParticleSystem; }
            set { explosionParticleSystem = value; }
        }

        ParticleSystem iceParticleSystem;
        public ParticleSystem IceParticleSystem
        {
            get { return iceParticleSystem; }
            set { iceParticleSystem = value; }
        }

        ParticleSystem magicParticleSystem;
        public ParticleSystem MagicParticleSystem
        {
            get { return magicParticleSystem; }
            set { magicParticleSystem = value; }
        }

        ParticleSystem iceMagicParticleSystem;
        public ParticleSystem IceMagicParticleSystem
        {
            get { return iceMagicParticleSystem; }
            set { iceMagicParticleSystem = value; }
        }

        ParticleSystem fireMagicParticleSystem;
        public ParticleSystem FireMagicParticleSystem
        {
            get { return fireMagicParticleSystem; }
            set { fireMagicParticleSystem = value; }
        }

        ParticleSystem smokeParticleSystem;
        public ParticleSystem SmokeParticleSystem
        {
            get { return smokeParticleSystem; }
            set { smokeParticleSystem = value; }
        }

        ParticleSystem fireParticleSystem;
        public ParticleSystem FireParticleSystem
        {
            get { return fireParticleSystem; }
            set { fireParticleSystem = value; }
        }

        ParticleSystem explosionSmokeParticleSystem;
        public ParticleSystem ExplosionSmokeParticleSystem
        {
            get { return explosionSmokeParticleSystem; }
            set { explosionSmokeParticleSystem = value; }
        }

        ParticleSystem matterCreationParticleSystem;
        public ParticleSystem MatterCreationParticleSystem
        {
            get { return matterCreationParticleSystem; }
            set { matterCreationParticleSystem = value; }
        }

        ParticleSystem lightningCreationParticleSystem;
        public ParticleSystem LightningCreationParticleSystem
        {
            get { return lightningCreationParticleSystem; }
            set { lightningCreationParticleSystem = value; }
        }

        ParticleSystem pushCreationParticleSystem;

        public ParticleSystem PushCreationParticleSystem
        {
            get { return pushCreationParticleSystem; }
            set { pushCreationParticleSystem = value; }
        }

        ParticleSystem pullCreationParticleSystem;

        public ParticleSystem PullCreationParticleSystem
        {
            get { return pullCreationParticleSystem; }
            set { pullCreationParticleSystem = value; }
        }

        // Resources for drawing.
        public GraphicsDeviceManager graphics;

        // The particle systems will all need a SpriteBatch to draw their particles,
        // so let's make one they can share. We'll use this to draw our SpriteFont
        // too.
        SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        
        #endregion

        public MagicWorldGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
           
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            base.LoadContent();
        }

        protected override void Initialize()
        {
            //The camera needs access to the graphics device which is only loaded before LoadContent.
            Camera2d camera = new Camera2d(this);
            Components.Add(camera);

            level = new Level(this);            
            Components.Add(level);

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            //with this entry the framework call the update itself
            //don't need to add in update method
            Components.Add(screenManager);
            Services.AddService(typeof(ScreenManager), screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

            ice = new IcedVisibility(this);
            Components.Add(ice);
            Services.AddService(typeof(IVisibility),ice);
            simpleAnimator = new SimpleAnimator(this);
            //Components.Add(simpleAnimator);
                        
            explosionParticleSystem = ParticleSystemFactory.getExplosion(this, 10);
            magicParticleSystem = ParticleSystemFactory.getMagic(this, 10);
            iceMagicParticleSystem = ParticleSystemFactory.getIceMagic(this, 10);
            fireMagicParticleSystem = ParticleSystemFactory.getFireMagic(this, 10);
            smokeParticleSystem = ParticleSystemFactory.getSmoke(this, 10);
            explosionSmokeParticleSystem = ParticleSystemFactory.getExplosionSmoke(this, 10);
            matterCreationParticleSystem = ParticleSystemFactory.getMatterCreation(this, 10);
            fireParticleSystem = ParticleSystemFactory.getFire(this, 10);
            lightningCreationParticleSystem = ParticleSystemFactory.getLightning(this, 100);
            iceParticleSystem = ParticleSystemFactory.getIce(this, 10);
            pullCreationParticleSystem = ParticleSystemFactory.getPull(this, 20);
            pushCreationParticleSystem = ParticleSystemFactory.getPush(this, 20);

            Components.Add(explosionParticleSystem);
            Components.Add(iceParticleSystem);
            Components.Add(magicParticleSystem);
            Components.Add(iceMagicParticleSystem);
            Components.Add(fireMagicParticleSystem);
            Components.Add(smokeParticleSystem);
            Components.Add(explosionSmokeParticleSystem);
            Components.Add(matterCreationParticleSystem);
            Components.Add(fireParticleSystem);
            Components.Add(lightningCreationParticleSystem);
            Components.Add(pullCreationParticleSystem);
            Components.Add(pushCreationParticleSystem);

#if DEBUG
            constantChanger = new ConstantChanger(this);
            Components.Add(constantChanger);

            gc_counter = new DikiLib.extensions.GARBAGE_COLLECTIONS_COUNTER(this);
            fps_counter = new DikiLib.extensions.FPS_COUNTER(this);
            Components.Add(gc_counter);
            Components.Add(fps_counter);
#endif

            hud = new HUD(this);
            Components.Add(hud);
            aid = new AimingAid(this);
            Components.Add(aid);

            tutManager = new TutorialManager(this);
            Services.AddService(typeof(TutorialManager), tutManager);
            Components.Add(tutManager);

            //Load saved config
            ConfigGame();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            
            base.Draw(gameTime);
        }

        #region save game

        /// <summary>
        /// Save the game
        /// the game name is Level<level Number>
        /// </summary>
        /// <param name="level"></param>
        public void SaveGame(int level,bool bestGame)
        {
            IAsyncResult result;
            string fileName;
            if (bestGame)
            {
                fileName = "HighScore" + level.ToString(); 
            }
            else
            {
                fileName = "Level" + level.ToString();
            }

            // Open a storage container.

            result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);
            

            StorageDevice device = StorageDevice.EndShowSelector(result);

            result = device.BeginOpenContainer("MagicWorld", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            
            // Check to see whether the save exists.
            if (container.FileExists(fileName))
                // Delete it so that we can create one fresh.
                container.DeleteFile(fileName);

            // Create the file.
            Stream stream = container.CreateFile(fileName);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            serializer.Serialize(stream, GameData);

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();

        }

        public void LoadGame(int level, bool bestGame)
        {
            string fileName;
            IAsyncResult result;
            SaveGameData tempData = new SaveGameData();

            ClearGameData(bestGame);

            if (bestGame)
            {
                fileName = "HighScore" + level.ToString();
            }
            else
            {
                fileName = "Level" + level.ToString();
            }            

            // Open a storage container.

            result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);


            StorageDevice device = StorageDevice.EndShowSelector(result);

            // Open a storage container.
            result =device.BeginOpenContainer("MagicWorld", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            

            // Check to see whether the save exists.
            if (!container.FileExists(fileName))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                return;
            }

            // Open the file.
            Stream stream = container.OpenFile(fileName, FileMode.Open);

            // Read the data from the file.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            tempData = (SaveGameData)serializer.Deserialize(stream);

            if (bestGame)
            {
                BestGameData = tempData;
            }
            else
            {
                GameData = tempData;
            }


            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();

            // Report the data to the console.
            //Debug.WriteLine("Completed:     " + GameData.Completed);
            //Debug.WriteLine("Level:    " + GameData.Level.ToString());
            //Debug.WriteLine("Items:    " + GameData.ItemsCollected.ToString());
            //Debug.WriteLine("Position: " + GameData.AvatarPosition.ToString());
        }
        public string[] GetSavedFiles(bool bestGame)
        {
            string[] files;

            IAsyncResult result;
            

            // Open a storage container.

            result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);


            StorageDevice device = StorageDevice.EndShowSelector(result);

            // Open a storage container.
            result = device.BeginOpenContainer("MagicWorld", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            if (bestGame)
            {
                files = container.GetFileNames("High*");
            }
            else
            {
                files = container.GetFileNames("Level*");
            }

            container.Dispose();
            return files;

        }

        #endregion

        #region Erase Profile

        public void EraseProfile()
        {
            IAsyncResult result;
            

            // Open a storage container.

            result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);


            StorageDevice device = StorageDevice.EndShowSelector(result);

            // Open a storage container.
            result = device.BeginOpenContainer("MagicWorld", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();
            
            foreach (string file in container.GetFileNames("High*"))
            {
                container.DeleteFile(file);

            }

            foreach (string file in container.GetFileNames("Level*"))
            {
                container.DeleteFile(file);

            }

            container.Dispose();

        }

        #endregion

        #region save config

        /// <summary>
        /// Save game configuration
        /// the game name is Level<level Number>
        /// </summary>
        /// <param name="level"></param>
        public void SaveGameConfig()
        {
            IAsyncResult result;

            string fileName = "Config.sav";

            // Open a storage container.

            result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);


            StorageDevice device = StorageDevice.EndShowSelector(result);

            result = device.BeginOpenContainer("MagicWorld", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();


            // Check to see whether the save exists.
            if (container.FileExists(fileName))
                // Delete it so that we can create one fresh.
                container.DeleteFile(fileName);

            // Create the file.
            Stream stream = container.CreateFile(fileName);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameStatus));
            serializer.Serialize(stream, GameStatus);

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();

        }

        private void LoadGameConfig()
        {
            string fileName;
            IAsyncResult result;

            fileName = "Config.sav";

            // Open a storage container.

            result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);


            StorageDevice device = StorageDevice.EndShowSelector(result);

            // Open a storage container.
            result = device.BeginOpenContainer("MagicWorld", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();


            // Check to see whether the save exists.
            if (!container.FileExists(fileName))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                SetGameDefaultParam();
                return;
            }

            // Open the file.
            Stream stream = container.OpenFile(fileName, FileMode.Open);

            // Read the data from the file.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameStatus));
            GameStatus = (SaveGameStatus)serializer.Deserialize(stream);

            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();
           
        }

        private void SetGameDefaultParam()
        {
            GameStatus.Control = "Laptop";
            GameStatus.FullScreenMode = false;
            GameStatus.PlayBackGroundMusic = false;
            GameStatus.Resolution = new Vector2 (800,600);
        }

        public void ConfigGame()
        {
            LoadGameConfig();

            if (GameStatus.FullScreenMode)
            {
                graphics.ToggleFullScreen();
            }

            if (GameStatus.Control.Equals("Default"))
            {
                PlayerControlFactory.GET_INSTANCE().ChangeControl(ControlType.defaultControl);
            }
            else
            {
                PlayerControlFactory.GET_INSTANCE().ChangeControl(ControlType.laptopControl);
            }

            screenManager.setScreenResolution((int)GameStatus.Resolution.X, (int)GameStatus.Resolution.Y);
        }
        #endregion

        #region Calculate Points

        public int CalulateTotalPoints()
        {
            return (int)(((GameData.ItemsCollected / GameData.TotalItems) * GeneralValues.PointsParam1)/ GameData.Time);
        }

        public bool PlayerGotBetterPerformance()
        {
            bool result=false;
            if (BestGameData.ItemsCollected <= GameData.ItemsCollected)
            {
                if (BestGameData.Time>0)
                    result = (BestGameData.Time >= GameData.Time) ? true : false;
                else                
                    result = true;                
            }
            return result;
        }

        private void ClearGameData(bool bestGame)
        {
            if (!bestGame)
            {
                GameData.ItemsCollected = 0;
                GameData.Completed = "Failed";
                GameData.Level = 0;
                GameData.Time = 0;
                GameData.TotalItems = 0;
                GameData.TotalPoints = 0;
            }
            else
            {
                BestGameData.ItemsCollected = 0;
                BestGameData.Completed = "Failed";
                BestGameData.Level = 0;
                BestGameData.Time = 0;
                BestGameData.TotalItems = 0;
                BestGameData.TotalPoints = 0;
            }


        }
        #endregion
    }
        
}
