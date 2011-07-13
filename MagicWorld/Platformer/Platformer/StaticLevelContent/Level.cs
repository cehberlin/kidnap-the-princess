using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent;
using MagicWorld.StaticLevelContent;
using Microsoft.Xna.Framework.Media;
using MagicWorld.HelperClasses.Collision;
using MagicWorld.Spells;
using MagicWorld.BlendInClasses;
using MagicWorld.Services;
using System.Diagnostics;
using MagicWorld.Constants;
using MagicWorld.Ingredients;
using MagicWorld.Audio;

namespace MagicWorld
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    public class Level : DrawableGameComponent
    {        
       
        #region properties
        // Entities in the level.
        public Player Player
        {
            get { return player; }
        }
        Player player;

        public IVisibility visibilityService;
        ISimpleAnimator simpleAnimator;
        
        Effect effect;

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

        protected ILevelLoader levelLoader;

        public ILevelLoader LevelLoader
        {
            get { return levelLoader; }
        }

        private int levelNumer;
        public int LevelNumber
        {
            set { levelNumer = value; }
            get {return levelNumer;}
        }

        private bool bPause;
        public bool Pause
        {
            set { bPause = value;
            if (bPause) { 
                clearAllParticles();
                clearAllSounds();
            }
            }
            get { return bPause; }
        }

        private MagicWorldGame game;

        public new MagicWorldGame Game
        {
            get { return game; }
        }

        TutorialManager tutManager;
        public TutorialManager TutorialManager
        {
            get { return tutManager; }
        }

        private InputState inputState = new InputState();

        private SpriteBatch spriteBatch;

        ICameraService camera;

        protected IAudioService audioService;               

        #endregion

        #region Loading

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        public Level( MagicWorldGame game):
            base(game)
        {
            this.game = game;
            
            // Create a new content manager to load content used just by this level.
            content = game.Content;
            effect = content.Load<Effect>("HelperShader"); 

            collisionManager = new CollisionManager(this);

            physicsManager = new PhysicsManager(this);

            player = new Player(this, Vector2.Zero);

            Visible = false;

            this.game.Services.AddService(typeof(Level), this);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
            audioService = (IAudioService)Game.Services.GetService(typeof(IAudioService));
        }


        public void initLevel(ILevelLoader levelLoader)
        {
            ingredients.Clear();

            visibilityService = (IVisibility)game.Services.GetService(typeof(IVisibility));

            camera = (ICameraService)Game.Services.GetService(typeof(ICameraService));

            this.levelLoader = levelLoader;

            game.GameData.Time = 0;

            this.CollectedIngredients.Clear();

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
            foreach (SpellType spell in levelLoader.UsableSpells)
            {
                Debug.Write(System.Enum.GetName(typeof(SpellType), spell) + ", ");
            }
            Debug.WriteLine("");
#endif
            player.Reset(startPoint);          
            
            endPoint = levelLoader.getLevelExit();

            levelBounds = levelLoader.getLevelBounds();

            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away

            audioService.playBackgroundmusic();

            reachedExit = false;

            simpleAnimator = (ISimpleAnimator)game.Services.GetService(typeof(ISimpleAnimator));
            //TODO: add portals to the service/ask christopher about the best way to "get" them

            tutManager = (TutorialManager)Game.Services.GetService(typeof(TutorialManager));
            tutManager.Initialize();
            tutManager.AddInstructionSet(levelLoader.GetTutorialInstructions());

            //reset particle effects
            clearAllParticles();
            clearAllSounds();

            Visible = true;
        }

        private void clearAllParticles()
        {
            game.ExplosionParticleSystem.Clear();
            game.ExplosionSmokeParticleSystem.Clear();
            game.FireParticleSystem.Clear();
            game.IceParticleSystem.Clear();
            game.LightningCreationParticleSystem.Clear();
            game.MagicItemParticleSystem.Clear();
            game.MagicParticleSystem.Clear();
            game.MatterCreationParticleSystem.Clear();
            game.PullCreationParticleSystem.Clear();
            game.PushCreationParticleSystem.Clear();
            game.SmokeParticleSystem.Clear();
        }

        private void clearAllSounds()
        {
            audioService.clearAllSounds();
        }

        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public new void Dispose()
        {
            Content.Unload();
            base.Dispose();
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (bPause)
            {
                return;
            }

            if (Visible)
            {
                inputState.Update();
                if (player.IsAlive && !reachedExit)
                {
                    game.GameData.Time += gameTime.ElapsedGameTime.Milliseconds;
                }

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

                    Player.Update(gameTime, inputState);

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

            GameTime objectTime;

            if (player.IsCasting) //slow motion
            {
                TimeSpan total = new TimeSpan((long)(gameTime.TotalGameTime.Ticks * PhysicValues.SLOW_MOTION_FACTOR));
                TimeSpan elapsed = new TimeSpan((long)(gameTime.ElapsedGameTime.Ticks * PhysicValues.SLOW_MOTION_FACTOR));
                objectTime = new GameTime(total, elapsed);
            }
            else
            {
                objectTime = gameTime;
            }

            List<BasicGameElement> removableObjects = new List<BasicGameElement>();
            for (int i = 0; i < generalColliadableGameElements.Count;i++ )                
                {
                    BasicGameElement elem = generalColliadableGameElements[i];
                    elem.Update(objectTime);

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
            //if (!reachedExit)
            //{
            //    audioService.playSound(SoundType.exitreached);
            //}
            reachedExit = true;
            clearAllParticles();
            clearAllSounds();
        }

        /// <summary>
        /// Restores the player to the starting point to try the level again.
        /// </summary>
        public void StartNewLife()
        {
            clearAllParticles();
            clearAllSounds();
            Player.Reset(startPoint);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void DrawPlayer(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Player.Draw(gameTime, spriteBatch);
        }

        public void DrawForeground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (player.IsCasting)
                effect.CurrentTechnique.Passes[0].Apply();
            //update foreground
            foreach (BasicGameElement elem in foregroundGameElements)
            {
                elem.Draw(gameTime, spriteBatch);
            }
        }

        public void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (player.IsCasting)
                effect.CurrentTechnique.Passes[0].Apply();
            //update background
            foreach (BasicGameElement elem in backgroundGameElements)
            {
                elem.Draw(gameTime, spriteBatch);
            }

            endPoint.Draw(gameTime, spriteBatch);

        }

        public void DrawColideableGameelements(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (player.IsCasting)
                effect.CurrentTechnique.Passes[0].Apply();
            foreach (BasicGameElement elem in generalColliadableGameElements)
            {
                if (!elem.GetType().IsSubclassOf(typeof(Spell)))
                    elem.Draw(gameTime, spriteBatch);

            }
        }

        public void DrawSpells(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (BasicGameElement elem in generalColliadableGameElements)
            {
                if (elem.GetType().IsSubclassOf(typeof(Spell)))
                    elem.Draw(gameTime, spriteBatch);
            }
        }

        public override void Draw(GameTime gameTime)
        {
                //Draw Background
                spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                camera.TransformationMatrix);

                DrawBackground(gameTime, spriteBatch);

                spriteBatch.End();

                //Draw Colideable gameelements except Spells
                spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                camera.TransformationMatrix);

                DrawColideableGameelements(gameTime, spriteBatch);

                spriteBatch.End();

                //Draw Spells
                spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                camera.TransformationMatrix);

                DrawSpells(gameTime, spriteBatch);

                spriteBatch.End();


                foreach (BasicGameElement elem in generalColliadableGameElements){
                    spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    null,
                    null,
                    null,
                    null,
                    camera.TransformationMatrix);

                    if(player.CurrentSpell != null)
                    if (elem.GetType().Equals(typeof(SpellMoveablePlatform)) && elem.Bounds.Box.Intersects(player.CurrentSpell.Bounds.Sphere)
                        || elem.GetType().Equals(typeof(PushPullElement)) && elem.Bounds.Box.Intersects(player.CurrentSpell.Bounds.Sphere))
                        elem.Draw(gameTime, spriteBatch);

                    spriteBatch.End();
                }

                //Draw Player
                spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                camera.TransformationMatrix);

                DrawPlayer(gameTime, spriteBatch);

                spriteBatch.End();

                //Draw Foreground
                spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                camera.TransformationMatrix);

                DrawForeground(gameTime, spriteBatch);

                spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion
    }
}
