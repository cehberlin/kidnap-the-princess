using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using MagicWorld.Spells;
using MagicWorld.DynamicLevelContent.Player;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;
using MagicWorld.Controls;
using MagicWorld.Constants;
using MagicWorld.Services;
using MagicWorld.StaticLevelContent;

namespace MagicWorld
{
    /// <summary>
    /// Our fearless adventurer!
    /// </summary>
    public class Player : BasicGameElement, IPlayerService
    {
        IEnemyService enemyService;

        #region input constants


        // Input configuration
        private const float MoveStickScale = 1.0f;

        #endregion

        public SpellType[] UsableSpells { get; set; }

        #region "Animation & sound"
        // Animations
        private Animation dieAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private AnimationPlayer sprite;
        private float rotation = 0.0f;

        private Animation runLeftAnimation;
        private Animation runRightAnimation;
        private Animation jumpLeftAnimation;
        private Animation jumpRightAnimation;
        private Animation idleAnimation;
        private Animation idleAnimationFacingLeft;
        private Animation celebrateAnimation;

        #endregion

        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;


        /// <summary>
        /// true if player is Facing Left
        /// </summary>
        private Boolean isFacingLeft = false;

        private Vector2 oldposition;

        public override Bounds Bounds
        {
            get
            {
                // Calculate bounds within texture size.
                float width = (sprite.Animation.FrameWidth * 0.22f);
                float height = (sprite.Animation.FrameHeight * 0.67f);
                float left = (float)Math.Round(Position.X - width / 2) + 5;
                float top = (float)Math.Round(Position.Y - height / 2 + 25); //25 special correctur factor for player head
                return new Bounds(left, top, width, height - 5);
            }
        }

        // Physics state
        public override Vector2 Position
        {
            get { return position; }
            set
            {
                Vector2 newPosition = new Vector2((float)Math.Round(value.X), (float)Math.Round(value.Y));
                //move spell in creation together with player
                if (currentSpell != null)
                {
                    currentSpell.Position = currentSpell.Position + (newPosition - position);
                }
                position = newPosition;
            }
        }

        Vector2 lastVelocity;

        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        bool isOnGround;
        public bool IsOnGround
        {
            get { return isOnGround; }
            set
            {
                isOnGround = value;
                if (isOnGround)
                {
                    gravityInfluenceMaxTime = PhysicValues.PLAYER_MAX_NO_GRAVITY_TIME;
                }
            }
        }

        /// <summary>
        /// Current user movement input.
        /// </summary>
        private float movementX;

        // Jumping state
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        private bool isDown;

        public bool nogravityHasInfluenceOnPlayer = true;

        /// <summary>
        /// save last movement direction on x axis
        /// </summary>
        private bool lastMovementRight = true;

        public Mana Mana { get; set; }

        /// <summary>
        /// true if the player is casting a spell
        /// </summary>
        public bool IsCasting { get { return currentSpell != null; } }

        //only one spell at a time
        Spell currentSpell = null;
        /// <summary>
        /// spell that is currently being casted by the player
        /// returns null if no spell is casted
        /// </summary>
        public Spell CurrentSpell { get { return currentSpell; } set { currentSpell = value; } }

        /// <summary>
        /// callback delegate for collision with specific objects
        /// </summary>
        protected CollisionManager.OnCollisionWithCallback collisionCallback;

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Player(Level level, Vector2 position)
            : base(level)
        {
            this.collisionManager = new CollisionManager(level);
            this.level = level;

            Mana = new Mana(this);

            LoadContent();

            debugColor = Color.Violet;
            level.Game.Services.AddService(typeof(IPlayerService), this);

            collisionCallback = HandleCollisionForOneObject;

            Reset(position);
        }

        /// <summary>
        /// Loads the player sprite sheet and sounds.
        /// </summary>
        public void LoadContent()
        {
            // Load animated textures.
            runRightAnimation = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 0);
            runLeftAnimation = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 1);
            jumpLeftAnimation = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 2);
            jumpRightAnimation = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 3);
            idleAnimation = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 1f, 1, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 4);

            idleAnimationFacingLeft = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 1f, 1, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 29);

            dieAnimation = new Animation("Content/Sprites/Player/dieSpriteSheet", 0.1f, 8, level.Content.Load<Texture2D>("Sprites/Player/dieSpriteSheet"), 0);
            //Celebrate wont be necessary if the exit will be added properly
            celebrateAnimation = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 4);

            base.LoadContent("");
        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position)
        {
            if (level.LevelLoader != null)
            {
                UsableSpells = level.LevelLoader.UsableSpells;
            }
            if (UsableSpells != null)
            {
                selectedSpellIndex_A = 0;
                selectedSpellIndex_B = selectNextSpell(selectedSpellIndex_B);
                if (selectedSpell_A == selectedSpell_B)
                {
                    selectedSpellIndex_B = selectNextSpell(selectedSpellIndex_B);
                }
            }
            enemyService = (IEnemyService)level.Game.Services.GetService(typeof(IEnemyService));
            Position = position;
            Velocity = Vector2.Zero;
            isAlive = true;
            sprite.PlayAnimation(idleAnimation);
        }

        private CollisionManager collisionManager;

        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        /// <remarks>
        /// We pass in all of the input states so that our game is only polling the hardware
        /// once per frame. We also pass the game's orientation because when using the accelerometer,
        /// we need to reverse our motion when the orientation is in the LandscapeRight orientation.
        /// </remarks>
        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState)
        {
            if (level.Pause)
            {
                return;
            }

            if (isAlive)
            {
                if (collisionManager.CollidateWithLevelExit(this))
                {
                    this.level.ReachedExit = true;
                }
                else
                {
                    this.level.ReachedExit = false;
                }


                Mana.update(gameTime);
                GetInput(keyboardState, gamePadState);


                HandleSpellCreation(gameTime, keyboardState, gamePadState);


                ApplyPhysics(gameTime);

                if (IsAlive && (IsOnGround || (disableGravity && gravityInfluenceMaxTime > 0)))
                {
                    if (Math.Abs(Velocity.X) - 0.02f > 0)//player is running and not just falling/sliding
                    {
                        if (lastMovementRight)
                        {
                            if (isFacingLeft)
                            {
                                spellAimAngle = -spellAimAngle;
                            }
                            isFacingLeft = false;
                            sprite.PlayAnimation(runRightAnimation);

                        }
                        else
                        {
                            if (!isFacingLeft)
                            {
                                spellAimAngle = -spellAimAngle;
                            }
                            isFacingLeft = true;
                            sprite.PlayAnimation(runLeftAnimation);
                        }
                    }
                    else
                    {
                        if (isFacingLeft)
                        {
                            sprite.PlayAnimation(idleAnimationFacingLeft);
                        }
                        else
                        {
                            sprite.PlayAnimation(idleAnimation);
                        }
                    }
                }

                gravityInfluenceMaxTime -= gameTime.ElapsedGameTime.TotalMilliseconds;

                // Clear input.
                movementX = 0.0f;
                isJumping = false;
                oldposition = position;
            }
        }

        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private void GetInput(
            KeyboardState keyboardState,
            GamePadState gamePadState)
        {
            IPlayerControl controls = PlayerControlFactory.GET_INSTANCE().getPlayerControl();

            // Get analog horizontal movement.
            movementX = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            // Ignore small movements to prevent running in place.
            if (Math.Abs(movementX) < 0.5f)
                movementX = 0.0f;

            // If any digital horizontal movement input is found, override the analog movement.
            if (gamePadState.IsButtonDown(controls.GamePad_Left) ||
                keyboardState.IsKeyDown(controls.Keys_Left) ||
                gamePadState.ThumbSticks.Left.X < 0
                )
            // ||keyboardState.IsKeyDown(LeftKeyAlternative))
            {
                movementX = -1.0f;
                if (!IsCasting)
                {
                    lastMovementRight = false;
                }
            }
            else if (gamePadState.IsButtonDown(controls.GamePad_Right) ||
                     keyboardState.IsKeyDown(controls.Keys_Right) ||
                gamePadState.ThumbSticks.Left.X > 0
                )
            //keyboardState.IsKeyDown(RightKeyAlternative))
            {
                movementX = 1.0f;
                if (!IsCasting)
                {
                    lastMovementRight = true;
                }
            }
            else if (Math.Abs(gamePadState.ThumbSticks.Left.X) > 0.5f)
            {
                movementX = gamePadState.ThumbSticks.Left.X;
            }
            else
            {
                movementX = 0.0f;
            }

            // Check if the player wants to jump.
            if (!this.IsCasting)
            {
                isJumping =
                    gamePadState.IsButtonDown(controls.GamePad_Jump) ||
                    keyboardState.IsKeyDown(controls.Keys_Jump);
            }
            //Check if the player press Down Button
            isDown =
                gamePadState.IsButtonDown(controls.GamePad_Down) ||
                keyboardState.IsKeyDown(controls.Keys_Down);

            if (!isJumping && !isDown && velocity.Y != 0.0f)
            {
                isFalling = true;
            }
            else
            {
                isFalling = false;
            }
        }

        Boolean isFalling = false;
        Boolean disableGravity = false;

        Double gravityInfluenceMaxTime = PhysicValues.PLAYER_MAX_NO_GRAVITY_TIME;

        /// <summary>
        /// Updates the player's velocity and position based on input, gravity, etc.
        /// </summary>
        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movementX * PhysicValues.PLAYER_MOVE_ACCELERATION * elapsed;
            if (disableGravity && gravityInfluenceMaxTime > 0)
            {
                if (isFalling)
                {
                    if (IsCasting)
                    {
                        lastVelocity.Y = 0;
                    }
                    else
                    {
                        velocity.Y = 0;
                    }
                }
                else
                {
                    if (IsCasting)
                    {
                        lastVelocity.Y = MathHelper.Clamp(lastVelocity.Y, -PhysicValues.PLAYER_MAX_FALL_SPEED, PhysicValues.PLAYER_MAX_FALL_SPEED);
                    }
                    else
                    {
                        velocity.Y = MathHelper.Clamp(velocity.Y, -PhysicValues.PLAYER_MAX_FALL_SPEED, PhysicValues.PLAYER_MAX_FALL_SPEED);
                    }
                }
            }
            else
            {
                if (IsCasting)
                {
                    lastVelocity.Y = MathHelper.Clamp(lastVelocity.Y + PhysicValues.PLAYER_GRAVITY_ACCELERATIOPM * elapsed, -PhysicValues.PLAYER_MAX_FALL_SPEED, PhysicValues.PLAYER_MAX_FALL_SPEED);
                }
                else
                {
                    velocity.Y = MathHelper.Clamp(velocity.Y + PhysicValues.PLAYER_GRAVITY_ACCELERATIOPM * elapsed, -PhysicValues.PLAYER_MAX_FALL_SPEED, PhysicValues.PLAYER_MAX_FALL_SPEED);
                }
            }

            // Apply pseudo-drag horizontally.
            if ((IsOnGround || (disableGravity && gravityInfluenceMaxTime > 0)))
                velocity.X *= PhysicValues.PLAYER_GROUND_DRAG_FACTOR;
            else
                velocity.X *= PhysicValues.PLAYER_AIR_DRAG_FACTOR;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -PhysicValues.PLAYER_MAX_MOVE_SPEED, PhysicValues.PLAYER_MAX_MOVE_SPEED);

            if (IsCasting)
            {
                lastVelocity.Y = DoJump(lastVelocity.Y, gameTime);
                Position += lastVelocity * elapsed * PhysicValues.SLOW_MOTION_FACTOR;
            }
            else
            {
                velocity.Y = DoJump(velocity.Y, gameTime);
                Position += velocity * elapsed;
                lastVelocity = velocity;
            }

            // If the player is now colliding with the level, separate them.
            HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                velocity.X = 0;


            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;

            //reset gravity flag it will be set again before next update cycle if we have still collison
            disableGravity = false;
        }

        /// <summary>
        /// Calculates the Y velocity accounting for jumping and
        /// animates accordingly.
        /// </summary>
        /// <remarks>
        /// During the accent of a jump, the Y velocity is completely
        /// overridden by a power curve. During the decent, gravity takes
        /// over. The jump velocity is controlled by the jumpTime field
        /// which measures time into the accent of the current jump.
        /// </remarks>
        /// <param name="velocityY">
        /// The player's current velocity along the Y axis.
        /// </param>
        /// <returns>
        /// A new Y velocity if beginning or continuing a jump.
        /// Otherwise, the existing Y velocity.
        /// </returns>
        private float DoJump(float velocityY, GameTime gameTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && (IsOnGround || (disableGravity && gravityInfluenceMaxTime > 0))) || jumpTime > 0.0f)
                {
                    if (jumpTime == 0.0f)
                        audioService.playSound(Audio.SoundType.playerjump);

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (!isFacingLeft)
                        sprite.PlayAnimation(jumpRightAnimation);
                    else
                        sprite.PlayAnimation(jumpLeftAnimation);
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= PhysicValues.PLAYER_MAX_JUMP_TIME)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = PhysicValues.PLAYER_JUMP_LAUNCH_VELOCITY * (1.0f - (float)Math.Pow(jumpTime / PhysicValues.PLAYER_MAX_JUMP_TIME, PhysicValues.PLAYER_JUMP_CONTROL_POWER));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }


        private void HandleCollisions()
        {

            // Falling off the bottom of the level kills the player.               
            if (level.CollisionManager.CollidateWithLevelBounds(this))
            {
                OnKilled(null);
                position.Y = oldposition.Y;
            }

            // The player has reached the exit if they are standing on the ground and
            // his bounding rectangle contains the center of the exit tile. They can only
            // exit when they have collected all of the gems.
            if (IsAlive &&
                IsOnGround &&
                level.CollisionManager.CollidateWithLevelExit(this) && level.Ingredients.Count == level.NeededIngredients)
            {
                OnReachedExit();
                level.OnExitReached();
            }

            if (IsCasting)
            {
                level.CollisionManager.HandleGeneralCollisions(this, ref isOnGround, collisionCallback);
            }
            else
            {
                //ignore plattforms if pushing movement downwards
                level.CollisionManager.HandleGeneralCollisions(this, ref isOnGround, collisionCallback, isDown);
            }
        }

        protected void HandleCollisionForOneObject(BasicGameElement element, bool xAxisCollision, bool yAxisCollision)
        {
            if (element.GetType().IsSubclassOf(typeof(Enemy)))
            {
                Enemy e = (Enemy)element;
                enemyService = (IEnemyService)e.GetService(typeof(IEnemyService));
                if (!enemyService.IsFroozen)
                {
                    OnKilled(e);
                }
            }
        }

        /// <summary>
        /// Called when the player has been killed.
        /// </summary>
        /// <param name="killedBy">
        /// The enemy who killed the player. This parameter is null if the player was
        /// not killed by an enemy (fell into a hole).
        /// </param>
        public void OnKilled(Enemy killedBy)
        {
            if (isAlive)
            {
                if (killedBy != null)
                    audioService.playSound(Audio.SoundType.playerkilled);
                else
                    audioService.playSound(Audio.SoundType.playerfall);

                sprite.PlayAnimation(dieAnimation);
            }
            isAlive = false;
        }

        /// <summary>
        /// Called when this player reaches the level's exit.
        /// </summary>
        public void OnReachedExit()
        {
            sprite.PlayAnimation(celebrateAnimation);
        }

        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip, rotation);

            base.Draw(gameTime, spriteBatch);
        }


        KeyboardState oldKeyboardState;
        GamePadState oldGamePadState;

        private int selectedSpellIndex_A = 0;
        private int selectedSpellIndex_B = 0;
        public SpellType selectedSpell_A { get { return UsableSpells[selectedSpellIndex_A]; } }
        public SpellType selectedSpell_B { get { return UsableSpells[selectedSpellIndex_B]; } }

        //current angle for spell casting
        double spellAimAngle = Math.PI / 2;

        /// <summary>
        /// keeps in mind working direction of the player
        /// </summary>
        public double SpellAimAngle
        {
            get
            {
                if (isPlayerFacingRight() || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.3f)
                {
                    return spellAimAngle;
                }
                else
                {
                    return -spellAimAngle;
                }
            }
            set { spellAimAngle = value; }
        }


        /// <summary>
        /// create the spells
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboardState"></param>
        /// <param name="gamePadState"></param>
        /// <param name="orientation"></param>
        private void HandleSpellCreation(GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState)
        {
            Boolean isCastingSpell = false;

            IPlayerControl controls = PlayerControlFactory.GET_INSTANCE().getPlayerControl();

            if (currentSpell == null) // no spell casted?
            {
                if (this.isSpellAButtonPressed(controls, gamePadState, keyboardState)) //spell A casted ?
                {
                    isCastingSpell = SpellCreationManager.tryStartCasting(this, selectedSpell_A, this.level);
                }
                else if (this.isSpellBButtonPressed(controls, gamePadState, keyboardState)) //spell B casted ?
                {
                    isCastingSpell = SpellCreationManager.tryStartCasting(this, selectedSpell_B, this.level);
                }
                else if (gamePadState.IsButtonUp(controls.GamePad_SelectedSpellA) && oldGamePadState.IsButtonDown(controls.GamePad_SelectedSpellA) || keyboardState.IsKeyUp(controls.Keys_SelectedSpellA) && oldKeyboardState.IsKeyDown(controls.Keys_SelectedSpellA)) // spell A select
                {
                    selectedSpellIndex_A = selectNextSpell(selectedSpellIndex_A);
                    if (selectedSpellIndex_A == selectedSpellIndex_B)
                    {
                        selectedSpellIndex_A = selectNextSpell(selectedSpellIndex_A);
                    }
                    Debug.WriteLine("changed selection for SpellSlot A: " + System.Enum.GetName(typeof(SpellType), selectedSpellIndex_A));
                }
                else if (gamePadState.IsButtonUp(controls.GamePad_SelectedSpellB) && oldGamePadState.IsButtonDown(controls.GamePad_SelectedSpellB) || keyboardState.IsKeyUp(controls.Keys_SelectedSpellB) && oldKeyboardState.IsKeyDown(controls.Keys_SelectedSpellB)) // spell B select
                {
                    selectedSpellIndex_B = selectNextSpell(selectedSpellIndex_B);
                    if (selectedSpellIndex_A == selectedSpellIndex_B)
                    {
                        selectedSpellIndex_B = selectNextSpell(selectedSpellIndex_B);
                    }
                    Debug.WriteLine("changed selection for SpellSlot B: " + System.Enum.GetName(typeof(SpellType), selectedSpellIndex_B));
                }
            }
            else
            {
                SpellCreationManager.furtherSpellCasting(this, this.level, gameTime);
                if (this.isSpellAButtonPressed(controls, gamePadState, keyboardState) || this.isSpellBButtonPressed(controls, gamePadState, keyboardState))
                {
                    // casting angle
                    //Debug.WriteLine("SpellConstantsValues.spellAimingRotationSpeed " + SpellConstantsValues.spellAimingRotationSpeed);
                    if (keyboardState.IsKeyDown(controls.Keys_Up))
                    {
                        if (isFacingLeft)
                        {
                            spellAimAngle -= SpellConstantsValues.spellAngleChangeStep * gameTime.ElapsedGameTime.TotalSeconds * SpellConstantsValues.spellAimingRotationSpeed;
                        } else {
                            spellAimAngle += SpellConstantsValues.spellAngleChangeStep * gameTime.ElapsedGameTime.TotalSeconds * SpellConstantsValues.spellAimingRotationSpeed;
                        }
                    }
                    else if (keyboardState.IsKeyDown(controls.Keys_Down))
                    {
                        if (!isFacingLeft)
                        {
                            spellAimAngle -= SpellConstantsValues.spellAngleChangeStep * gameTime.ElapsedGameTime.TotalSeconds * SpellConstantsValues.spellAimingRotationSpeed;
                        } else {
                            spellAimAngle += SpellConstantsValues.spellAngleChangeStep * gameTime.ElapsedGameTime.TotalSeconds * SpellConstantsValues.spellAimingRotationSpeed;
                        }
                    }
                    //Use Thumbstick to aim the spell
                    if (gamePadState.ThumbSticks.Left.Length() > 0.1f)
                    {
                        spellAimAngle = Math.Atan2(gamePadState.ThumbSticks.Left.Y, gamePadState.ThumbSticks.Left.X) - 3 * Math.PI / 2;
                    }
                    //casting power
                    if (keyboardState.IsKeyDown(controls.Keys_Right) || gamePadState.IsButtonDown(controls.GamePad_Right)) // more power
                    {
                        if (isPlayerFacingRight())
                        {
                            SpellCreationManager.morePower(this, this.level, gameTime);
                        }
                        else
                        {
                            SpellCreationManager.lessPower(this, this.level, gameTime);
                        }
                    }
                    else if (keyboardState.IsKeyDown(controls.Keys_Left) || gamePadState.IsButtonDown(controls.GamePad_Left)) // less power
                    {
                        if (isPlayerFacingRight())
                        {
                            SpellCreationManager.lessPower(this, this.level, gameTime);
                        }
                        else
                        {
                            SpellCreationManager.morePower(this, this.level, gameTime);
                        }
                    }
                    else if (gamePadState.IsButtonDown(controls.GamePad_IncreaseSpell))
                    {
                        SpellCreationManager.morePower(this, this.level, gameTime);
                    }
                    else if (gamePadState.IsButtonDown(controls.GamePad_DecreaseSpell))
                    {
                        SpellCreationManager.lessPower(this, this.level, gameTime);
                    }
                }
                else
                {
                    SpellCreationManager.releaseSpell(this);
                }
            }

            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
        }

        /// <summary>
        /// calculate spell position on a cycle bow around the player
        /// </summary>
        /// <returns></returns>
        public Vector2 getCurrentSpellPosition()
        {
            Vector2 pos;

            double angle;

            /// keeps in mind working direction of the player (mirrors the angle)
            // TODO DISABLE WHILE GAMEPAD AIMING, THIS SHOULD FIX ONE OF THE BUGS
            if (lastMovementRight || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.3f)
            {
                angle = spellAimAngle;
            }
            else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.3f)
            {
                angle = spellAimAngle;
            }
            else
            {
                angle = -spellAimAngle;
            }

            pos = Position + new Vector2((float)(Math.Sin(angle) * SpellConstantsValues.spellDistanceToPlayerMidPoint), (float)(Math.Cos(angle) * SpellConstantsValues.spellDistanceToPlayerMidPoint));
            return pos;
        }

        private bool isSpellAButtonPressed(IPlayerControl controls, GamePadState gamePadState, KeyboardState keyboardState)
        {
            return gamePadState.IsButtonDown(controls.GamePad_CastSelectedSpellA) || keyboardState.IsKeyDown(controls.Keys_CastSelectedSpellA);
        }

        private bool isSpellBButtonPressed(IPlayerControl controls, GamePadState gamePadState, KeyboardState keyboardState)
        {
            return gamePadState.IsButtonDown(controls.GamePad_CastSelectedSpellB) || keyboardState.IsKeyDown(controls.Keys_CastSelectedSpellB);
        }

        private int selectNextSpell(int currentIndex)
        {
            currentIndex++;
            if (currentIndex >= this.UsableSpells.Length)
            {
                return 0;
            }
            return currentIndex;
        }

        #region ISpellInfluenceable Member

        public override bool SpellInfluenceAction(Spell spell)
        {

            if (nogravityHasInfluenceOnPlayer && spell.GetType() == typeof(NoGravitySpell))
            {
                disableGravity = true;
            }
            return false; //do not remove spell
        }

        #endregion

        public object GetService(Type serviceType)
        {
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if the player is walking to the rigth </returns>
        public bool isPlayerFacingRight()
        {
            return !isFacingLeft;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if the player is walking to the left</returns>
        public bool isPlayerFacingLeft()
        {
            return isFacingLeft;
        }


        #region IPlayerService Member


        public bool isAiming
        {
            get { return IsCasting && currentSpell.GetType() != typeof(PushSpell) && currentSpell.GetType() != typeof(PullSpell); }
        }

        #endregion
    }
}
