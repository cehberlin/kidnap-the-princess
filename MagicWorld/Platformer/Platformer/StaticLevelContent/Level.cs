#region File Description
//-----------------------------------------------------------------------------
// Level.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

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

namespace MagicWorld
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    class Level : IDisposable
    {    
        // Entities in the level.
        public Player Player
        {
            get { return player; }
        }
        Player player;

        private List<Enemy> enemies = new List<Enemy>();

        internal List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        // Physical structure of the level.

        private List<BasicGameElement> generalColliadableGameElements = new List<BasicGameElement>();

        internal List<BasicGameElement> GeneralColliadableGameElements
        {
            get { return generalColliadableGameElements; }
            set { generalColliadableGameElements = value; }
        }

        private List<Gem> gems = new List<Gem>();

        internal List<Gem> Gems
        {
            get { return gems; }
            set { gems = value; }
        }

        /// <summary>
        /// All Spells on Stack
        /// </summary>
        private List<Spell> spells = new List<Spell>();
        public List<Spell> Spells { 
            get {return spells;}
            set{spells = value;}
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

        public int Score
        {
            get { return score; }
        }
        int score;

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

        private const int PointsPerSecond = 5;

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

        private SoundEffect exitReachedSound;

        #region Loading

        protected LevelLoader levelLoader;

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that will be used to construct a ContentManager.
        /// </param>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        public Level(IServiceProvider serviceProvider, LevelLoader levelLoader)
        {
            // Create a new content manager to load content used just by this level.
            content = new ContentManager(serviceProvider, "Content");

            collisionManager = new CollisionManager(this);

            this.levelLoader = levelLoader;

            initLevel();
        }


        protected void initLevel(){
            levelLoader.Level = this;

            timeRemaining = TimeSpan.FromMinutes(levelLoader.getMaxLevelTime());

            enemies = levelLoader.getEnemies();

            generalColliadableGameElements = levelLoader.getGeneralObjects();

            startPoint = levelLoader.getPlayerStartPosition();

            player = new Player(this, startPoint);

            endPoint = levelLoader.getLevelExit();

            score = 0;

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
                score += seconds * PointsPerSecond;
            }
            else
            {
                timeRemaining -= gameTime.ElapsedGameTime;

                //IMPORTANT SPELLS MUST BE UPDATED BEFORE PLAYER AND TILES MAKES SPELL IMPLEMENTATION MUCH EASIER!!
                //FOR DETAILS CHECK NoGravity in DynamicTile.cs
                //Update Spells which are inside the level
                UpdateSpells(gameTime);

                Player.Update(gameTime, keyboardState, gamePadState,orientation);
                UpdateGems(gameTime);
                UpdateObjects(gameTime);

                // Falling off the bottom of the level kills the player.               
                if (collisionManager.CollidateWithLevelBounds(player))
                {
                    OnPlayerKilled(null);
                }

                UpdateEnemies(gameTime);

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
            spells.Add(spell);
        }

        private void UpdateSpells(GameTime gameTime)
        {
            List<Spell> removeableSpells = new List<Spell>();
            //first update
            foreach (Spell spell in spells)
            {
                spell.Update(gameTime);
                if (spell.SpellState == Spell.State.REMOVE)
                {
                    removeableSpells.Add(spell);
                }
            }

            //remove
            foreach (Spell spell in removeableSpells)
            {
                if (spell.SpellState == Spell.State.REMOVE)
                {
                    spells.Remove(spell);
                }
            }
        }


        //NOT USED IN THE MOMENT
        /// <summary>
        /// Animates each gem and checks to allows the player to collect them.
        /// </summary>
        private void UpdateGems(GameTime gameTime)
        {
            //for (int i = 0; i < gems.Count; ++i)
            //{
            //    Gem gem = gems[i];

            //    gem.Update(gameTime);

            //    if (gem.BoundingCircle.Intersects(Player.BoundingRectangle))
            //    {
            //        gems.RemoveAt(i--);
            //        OnGemCollected(gem, Player);
            //    }
            //}
        }

        /// <summary>
        /// Update all other objecs
        /// Icecicles
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateObjects(GameTime gameTime)
        {
            List<BasicGameElement> removableObjects = new List<BasicGameElement>();
            foreach(BasicGameElement elem in   generalColliadableGameElements)               
            {
                elem.Update(gameTime);

                //check for removements of blocks
                //handle special iceicles
                if ((elem.GetType() == typeof(IceBlockElement) &&
                    ((IceBlockElement)elem).State == BlockElement.SpellState.DESTROYED))
                {
                    removableObjects.Add(elem);
                }
                else if ((elem.GetType() == typeof(Icecicle) &&  //handle special iceicles
                    ((Icecicle)elem).icecicleState == IcecicleState.DESTROYED))
                {
                    removableObjects.Add(elem);                 
                }
            }
            //remove destroyed elements
            foreach (BasicGameElement elem in removableObjects)
            {
                generalColliadableGameElements.Remove(elem);
            }
        }

        /// <summary>
        /// Animates each enemy and allow them to kill the player.
        /// </summary>
        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);

                if(collisionManager.CollidateWithPlayer(enemy)){
                    OnPlayerKilled(enemy);
                }
            }
        }


        /// <summary>
        /// Called when a gem is collected.
        /// </summary>
        /// <param name="gem">The gem that was collected.</param>
        /// <param name="collectedBy">The player who collected this gem.</param>
        private void OnGemCollected(Gem gem, Player collectedBy)
        {
            score += Gem.PointValue;

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
            endPoint.Draw(gameTime, spriteBatch);

            foreach (Gem gem in gems)
                gem.Draw(gameTime, spriteBatch);

            foreach (BasicGameElement elem in generalColliadableGameElements)
                elem.Draw(gameTime, spriteBatch);

            Player.Draw(gameTime, spriteBatch);

            foreach (Enemy enemy in enemies)
                enemy.Draw(gameTime, spriteBatch);
      
            foreach (Spell spell in spells)
                spell.Draw(gameTime, spriteBatch);
        }

        #endregion
    }
}
