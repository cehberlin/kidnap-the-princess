using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent;
using MagicWorld.StaticLevelContent;
using Microsoft.Xna.Framework.Media;
using MagicWorld.HelperClasses.Collision;
using ParticleEffects;
using MagicWorld.Spells;
using MagicWorld.DynamicLevelContent.ParticleEffects;
using System.Diagnostics;

namespace MagicWorld
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    public class Level : IDisposable
    {
        // Entities in the level.
        public Player Player
        {
            get { return player; }
        }
        Player player;

        // Physical structure of the level.

        private List<BasicGameElement> generalColliadableGameElements = new List<BasicGameElement>();

        internal List<BasicGameElement> GeneralColliadableGameElements
        {
            get { return generalColliadableGameElements; }
            set { generalColliadableGameElements = value; }
        }

        private List<BasicGameElement> backgroundGameElements = new List<BasicGameElement>();

        private List<BasicGameElement> foregroundGameElements = new List<BasicGameElement>();

        private List<BasicGameElement> collectedIngredients = new List<BasicGameElement>();

        public List<BasicGameElement> CollectedIngredients
        {
            get { return collectedIngredients; }
            set { collectedIngredients = value; }
        }

        // needed ingredients
        private int neededIngredients = 2;
        public int NeededIngredients
        {
            get { return neededIngredients; }
        }

        private List<BasicGameElement> ingredients = new List<BasicGameElement>();

        public List<BasicGameElement> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }

        private int maxIngredientsCount = 0;
        public int MaxIngredientsCount
        {
            get { return maxIngredientsCount; }
        }

        /// <summary>
        /// minimum of Collectable items the player has to collect in this level to finish it
        /// </summary>
        /// <returns></returns>
        public int GetMinimumItemsToEndLevel { get; protected set; }

        //// Key locations in the level.     

        /// <summary>
        /// position where the player starts
        /// </summary>
        protected Vector2 startPoint;

        private BasicGameElement endPoint;

        public BasicGameElement EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; }
        }

        

        // Level game state.
        private Random random = new Random(354668); // Arbitrary, but constant seed

        public bool ReachedExit
        {
            get { return reachedExit; }
            set { reachedExit = value; }
        }
        bool reachedExit;

        public TimeSpan TimeRemaining
        {
            get { return timeRemaining; }
        }
        TimeSpan timeRemaining;

        // Level content.        
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        CollisionManager collisionManager;

        public CollisionManager CollisionManager
        {
            get { return collisionManager; }
        }

        PhysicsManager physicsManager;

        public PhysicsManager PhysicsManager
        {
            get { return physicsManager; }
        }

        /// <summary>
        /// max area for the level
        /// </summary>
        Bounds levelBounds;

        public Bounds LevelBounds
        {
            get { return levelBounds; }
            set { levelBounds = value; }
        }

        private SoundEffect exitReachedSound;

        #region Loading

        protected ILevelLoader levelLoader;

        private MagicWorldGame game;

        public MagicWorldGame Game
        {
            get { return game; }
        }

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that will be used to construct a ContentManager.
        /// </param>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        public Level(IServiceProvider serviceProvider, ILevelLoader levelLoader,MagicWorldGame game)
        {
            this.game = game;
            // Create a new content manager to load content used just by this level.
            content = new ContentManager(serviceProvider, "Content");

            collisionManager = new CollisionManager(this);

            physicsManager = new PhysicsManager(this);

            this.levelLoader = levelLoader;

            initLevel();
        }


        protected void initLevel()
        {
            levelLoader.init(this);

            Debug.WriteLine("load level ");

            neededIngredients = levelLoader.getMinimumItemsToEndLevel();

            maxIngredientsCount = levelLoader.getMaxItmesToCollect();

            timeRemaining = TimeSpan.FromMinutes(levelLoader.getMaxLevelTime());

            generalColliadableGameElements = levelLoader.getInteractingObjects();

            backgroundGameElements = levelLoader.getBackgroundObjects();

            foregroundGameElements = levelLoader.getForegroundObjects();

            //disable debug for background and foreground objects
            foreach (BasicGameElement b in backgroundGameElements)
            {
                b.PrivateDebug = false;
            }
            foreach (BasicGameElement b in foregroundGameElements)
            {
                b.PrivateDebug = false;
            }

            maxIngredientsCount = ingredients.Count;

            startPoint = levelLoader.getPlayerStartPosition();

#if DEBUG
            Debug.Write("usable spells in level: ");
            foreach(SpellType spell in levelLoader.UsableSpells)
            {
                Debug.Write(System.Enum.GetName(typeof(SpellType),spell) + ", ");
            }
            Debug.WriteLine("");
#endif
            player = new Player(this, startPoint, levelLoader.UsableSpells);

            endPoint = levelLoader.getLevelExit();

            levelBounds = levelLoader.getLevelBounds();

            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away
            try
            {
                MediaPlayer.IsMuted = true;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(levelLoader.getBackgroundMusic());
            }
            catch { }

            reachedExit = false;

            // Load sounds.
            exitReachedSound = Content.Load<SoundEffect>("Sounds/ExitReached");
        }

        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            StopBackgroundSound();
            Content.Unload();
        }

        //Stop sound
        public void StopBackgroundSound()
        {
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Stop();
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState,
            DisplayOrientation orientation)
        {
            // Pause while the player is dead or time is expired.
            if (!Player.IsAlive || TimeRemaining == TimeSpan.Zero)
            {
                // Still want to perform physics on the player.
                Player.ApplyPhysics(gameTime);
            }
            //else if (ReachedExit)
            //{
            //    // Animate the time being converted into points.
            //    int seconds = (int)Math.Round(gameTime.ElapsedGameTime.TotalSeconds * 100.0f);
            //    seconds = Math.Min(seconds, (int)Math.Ceiling(TimeRemaining.TotalSeconds));
            //    timeRemaining -= TimeSpan.FromSeconds(seconds);
            //}
            else
            {
                timeRemaining -= gameTime.ElapsedGameTime;

                Player.Update(gameTime, keyboardState, gamePadState, orientation);

                UpdateObjects(gameTime);
            }

            // Clamp the time remaining at zero.
            if (timeRemaining < TimeSpan.Zero)
                timeRemaining = TimeSpan.Zero;

            if (backgroundGameElements.Count > 0)
            {
                backgroundGameElements[0].Position = player.Position - new Vector2(backgroundGameElements[0].Bounds.Width / 2, backgroundGameElements[0].Bounds.Height / 2);
            }
        }


        public void addSpell(Spell spell)
        {
            generalColliadableGameElements.Add(spell);
        }

        /// <summary>
        /// Update all other objecs
        /// Icecicles
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateObjects(GameTime gameTime)
        {
            //update background
            foreach (BasicGameElement elem in backgroundGameElements)
            {
                elem.Update(gameTime);
            }

            List<BasicGameElement> removableObjects = new List<BasicGameElement>();
            foreach (BasicGameElement elem in generalColliadableGameElements)
            {
                elem.Update(gameTime);

                if (elem.IsRemovable)
                {
                    removableObjects.Add(elem);
                }
            }
            //remove destroyed elements
            foreach (BasicGameElement elem in removableObjects)
            {
                generalColliadableGameElements.Remove(elem);
            }

            //update foreground
            foreach (BasicGameElement elem in foregroundGameElements)
            {
                elem.Update(gameTime);
            }
        }

        /// <summary>
        /// Called when the player reaches the level's exit.
        /// </summary>
        public void OnExitReached()
        {
            //exitReachedSound.Play();
            reachedExit = true;
        }

        /// <summary>
        /// Restores the player to the starting point to try the level again.
        /// </summary>
        public void StartNewLife()
        {
            Player.Reset(startPoint);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //update background
            foreach (BasicGameElement elem in backgroundGameElements)
            {
                elem.Draw(gameTime, spriteBatch);
            }

            endPoint.Draw(gameTime, spriteBatch);

            foreach (BasicGameElement elem in generalColliadableGameElements)
            {
                elem.Draw(gameTime, spriteBatch);
            }

            Player.Draw(gameTime, spriteBatch);

            //update background
            foreach (BasicGameElement elem in foregroundGameElements)
            {
                elem.Draw(gameTime, spriteBatch);
            }
        }

        #endregion
    }
}
