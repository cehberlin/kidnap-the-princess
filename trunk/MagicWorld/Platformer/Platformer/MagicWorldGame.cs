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
    }

    public class MagicWorldGame : Microsoft.Xna.Framework.Game
    {

        #region properties
         
        ScreenManager screenManager;
        HUD hud;
        AimingAid aid;
        TutorialManager tutManager;
        IcedVisibility ice;

        public SaveGameData GameData=new SaveGameData();
        ConstantChanger constantChanger;

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
           
            //Content.RootDirectory = "Content";
            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            
            //with this entry the framework call the update itself
            //don't need to add in update method
            Components.Add(screenManager);
            Services.AddService(typeof(ScreenManager), screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            //The camera needs access to the graphics device which is only loaded before LoadContent.
            Camera2d camera = new Camera2d(this);
            Components.Add(camera);
            base.LoadContent();
        }

        protected override void Initialize()
        {
            hud = new HUD(this);
            Components.Add(hud);
            aid = new AimingAid(this);
            Components.Add(aid);
            ice = new IcedVisibility(this);
            Components.Add(ice);
            Services.AddService(typeof(IVisibility),ice);

            #if DEBUG
                constantChanger = new ConstantChanger(this);
                Components.Add(constantChanger);
            #endif

            TutorialManager tut = new TutorialManager(this);
            Services.AddService(typeof(TutorialManager), tut);
            Components.Add(tut);
            
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
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(gameTime);
        }

        /// <summary>
        /// Save the game
        /// the game name is Level<level Number>
        /// </summary>
        /// <param name="level"></param>
        public void SaveGame(int level)
        {
            IAsyncResult result;
            
            string fileName="Level" + level.ToString();

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

        public void LoadGame(int level)
        {
            string fileName;
            IAsyncResult result;

            fileName = "Level" + level.ToString();

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
            GameData = (SaveGameData)serializer.Deserialize(stream);

            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();

            // Report the data to the console.
            Debug.WriteLine("Completed:     " + GameData.Completed);
            Debug.WriteLine("Level:    " + GameData.Level.ToString());
            Debug.WriteLine("Items:    " + GameData.ItemsCollected.ToString());
            //Debug.WriteLine("Position: " + GameData.AvatarPosition.ToString());
        }
        public string[] GetSavedFiles()
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


            files = container.GetFileNames();
            return files;
            
        }
        
    }
        
}
