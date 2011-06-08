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

namespace MagicWorld
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    public class Level : IDisposable,IServiceProvider
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
        private const int neededIngredients = 5;
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

        public ParticleSystem ExplosionParticleSystem;
        public ParticleSystem SmokeParticleSystem;
        public ParticleSystem MagicParticleSystem;
        public ParticleSystem ExplosionSmokeParticleSystem;

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

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that will be used to construct a ContentManager.
        /// </param>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        public Level(IServiceProvider serviceProvider, ILevelLoader levelLoader)
        {
            // Create a new content manager to load content used just by this level.
            content = new ContentManager(serviceProvider, "Content");

            collisionManager = new CollisionManager(this);

            physicsManager = new PhysicsManager(this);

            this.levelLoader = levelLoader;

            ExplosionParticleSystem = new ExplosionParticleSystem(this, 20);

            MagicParticleSystem = new MagicParticleSystem(this, 20);

            SmokeParticleSystem = new SmokePlumeParticleSystem(this, 20);

            ExplosionSmokeParticleSystem = new ExplosionSmokeParticleSystem(this, 20);

            initLevel();
        }


        protected void initLevel(){
            levelLoader.init(this);

            timeRemaining = TimeSpan.FromMinutes(levelLoader.getMaxLevelTime());

            generalColliadableGameElements = levelLoader.getInteractingObjects();

            backgroundGameElements = levelLoader.getBackgroundObjects();

            foregroundGameElements = levelLoader.getForegroundObjects();

            //disable debug for background and foreground objects
            foreach(BasicGameElement b in backgroundGameElements){
                b.PrivateDebug = false;
            }
            foreach (BasicGameElement b in foregroundGameElements)
            {
                b.PrivateDebug = false;
            }

            maxIngredientsCount = ingredients.Count;

            startPoint = levelLoader.getPlayerStartPosition();

            player = new Player(this, startPoint, levelLoader.UsableSpells);

            endPoint = levelLoader.getLevelExit();

            levelBounds = levelLoader.getLevelBounds();

            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away
            try
            {
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
            Content.Unload();
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
            else if (ReachedExit)
            {
                // Animate the time being converted into points.
                int seconds = (int)Math.Round(gameTime.ElapsedGameTime.TotalSeconds * 100.0f);
                seconds = Math.Min(seconds, (int)Math.Ceiling(TimeRemaining.TotalSeconds));
                timeRemaining -= TimeSpan.FromSeconds(seconds);
            }
            else
            {
                timeRemaining -= gameTime.ElapsedGameTime;

                Player.Update(gameTime, keyboardState, gamePadState,orientation);

                UpdateObjects(gameTime);

                // Falling off the bottom of the level kills the player.               
                if (collisionManager.CollidateWithLevelBounds(player))
                {
                    OnPlayerKilled(null);
                }

                UpdateParticleEffects(gameTime);

                // The player has reached the exit if they are standing on the ground and
                // his bounding rectangle contains the center of the exit tile. They can only
                // exit when they have collected all of the gems.
                if (Player.IsAlive &&
                    Player.IsOnGround &&
                    collisionManager.CollidateWithLevelExit(player))
                {
                    OnExitReached();
                }
            }

            // Clamp the time remaining at zero.
            if (timeRemaining < TimeSpan.Zero)
                timeRemaining = TimeSpan.Zero;
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
            foreach(BasicGameElement elem in   generalColliadableGameElements)               
            {
                elem.Update(gameTime);

                if (elem.IsRemovable)
                {
                    removableObjects.Add(elem);
                }

                //special update behavior for some classes

                //enemies
                if (elem.GetType() == typeof(Enemy))
                {
                    Enemy enemy = (Enemy)elem;
                    UpdateEnemy(enemy);
                }
                //spells
                else if (elem.GetType() == typeof(Spell))
                {
                    Spell spell = (Spell)elem;
                    UpdateSpell(spell);
                }
                //Gems
                else if (elem.GetType() == typeof(Gem))
                {
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

        private void UpdateEnemy(Enemy enemy)
        {
            if (!enemy.isFroozen && collisionManager.CollidateWithPlayer(enemy))
            {
                OnPlayerKilled(enemy);
            }
        }

        private void UpdateSpell(Spell spell)
        {
            //add if necessary
        }

        private void UpdateParticleEffects(GameTime gameTime)
        {
            ExplosionParticleSystem.Update(gameTime);
            SmokeParticleSystem.Update(gameTime);
            MagicParticleSystem.Update(gameTime);
            ExplosionSmokeParticleSystem.Update(gameTime);
        }


        /// <summary>
        /// Called when a gem is collected.
        /// </summary>
        /// <param name="gem">The gem that was collected.</param>
        /// <param name="collectedBy">The player who collected this gem.</param>
        private void OnGemCollected(Gem gem, Player collectedBy)
        {
            gem.OnCollected(collectedBy);
        }

        /// <summary>
        /// Called when the player is killed.
        /// </summary>
        /// <param name="killedBy">
        /// The enemy who killed the player. This is null if the player was not killed by an
        /// enemy, such as when a player falls into a hole.
        /// </param>
        private void OnPlayerKilled(Enemy killedBy)
        {
            Player.OnKilled(killedBy);
        }

        /// <summary>
        /// Called when the player reaches the level's exit.
        /// </summary>
        public void OnExitReached()
        {
            Player.OnReachedExit();
            exitReachedSound.Play();
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

            ExplosionParticleSystem.Draw(gameTime, spriteBatch);
            SmokeParticleSystem.Draw(gameTime, spriteBatch);
            MagicParticleSystem.Draw(gameTime, spriteBatch);
            ExplosionSmokeParticleSystem.Draw(gameTime, spriteBatch);

            //update background
            foreach (BasicGameElement elem in foregroundGameElements)
            {
                elem.Draw(gameTime, spriteBatch);
            }

        }

        #endregion
        //TODO: Level is not going to remain a service but it will provide a service that exhibits all important values.
        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
