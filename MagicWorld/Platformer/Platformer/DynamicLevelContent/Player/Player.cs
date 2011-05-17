#region File Description
//-----------------------------------------------------------------------------
// Player.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
using Platformer.Spells;
using Platformer.DynamicLevelContent.Player;

namespace Platformer
{
    /// <summary>
    /// Our fearless adventurer!
    /// </summary>
    class Player:ISpellInfluenceable
    {

        public Mana Mana { get; set; }

        /// <summary>
        /// true if the player is casting a spell
        /// </summary>
        public bool IsCasting {get{return currentSpell != null;}}
        private bool isCasting = false;

        #region "Animation & sound"


        // Animations
        private Animation idleAnimation;
        private Animation runAnimation;
        private Animation jumpAnimation;
        private Animation celebrateAnimation;
        private Animation dieAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private AnimationPlayer sprite;
        private float rotation = 0.0f;

        // Sounds
        private SoundEffect killedSound;
        private SoundEffect jumpSound;
        private SoundEffect fallSound;

        #endregion

        public Level Level
        {
            get { return level; }
        }
        Level level;

        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        // Physics state
        public Vector2 Position
        {
            get { return position; }
            set
            {
                //move spell in creation together with player
                if (currentSpell != null)
                {
                    currentSpell.Position = currentSpell.Position + (value - position);
                }
                position = value;
            }
        }
        Vector2 position;

        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        #region control constants

        //gamepad
        //spells
        public const Buttons WarmButton = Buttons.A;
        public const Buttons ColdButton = Buttons.B;
        public const Buttons MatterButton = Buttons.X;
        public const Buttons GravityButton = Buttons.Y;

        //movement
        public const Buttons LeftButton = Buttons.DPadLeft;
        public const Buttons RightButton = Buttons.DPadRight;
        public const Buttons JumpButton = Buttons.DPadUp;
        public const Buttons DownButton = Buttons.DPadDown;

        //keyboard
        //spells
        public const Keys WarmKey = Keys.J;
        public const Keys ColdKey = Keys.I;
        public const Keys MatterKey = Keys.O;
        public const Keys GravityKey = Keys.P;

        public const Keys LeftKey = Keys.A;
        public const Keys RightKey = Keys.D;
        public const Keys JumpKey = Keys.W;
        public const Keys DownKey = Keys.S;

        public const Keys LeftKeyAlternative = Keys.Left;
        public const Keys RightKeyAlternative = Keys.Right;
        public const Keys JumpKeyAlternative = Keys.Up;
        public const Keys DownKeyAlternative = Keys.Down;

        public const Keys FullscreenToggleKey = Keys.F11;
        public const Keys DebugToggleKey = Keys.F3;
        public const Keys ExitGameKey = Keys.Escape;

        // Input configuration
        private const float MoveStickScale = 1.0f;
        private const float AccelerometerScale = 1.5f;

        #endregion

        #region "movment constants"

        // Constants for controling horizontal movement
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;

        #endregion



        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        /// <summary>
        /// Current user movement input.
        /// </summary>
        private float movement;
        private Vector2 lastDirection;
        public Vector2 Direction
        {
            get { return lastDirection; }
        }

        // Jumping state
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        private bool isDown;

        private Rectangle localBounds;
        /// <summary>
        /// Gets a rectangle which bounds this player in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }



        /// <summary>
        /// 4 Spell Slots
        /// </summary>
        public Spell SpellSlotA { get; set; }
        public Spell SpellSlotB { get; set; }
        public Spell SpellSlotC { get; set; }
        public Spell SpellSlotD { get; set; }


        //only one spell at a time
        Spell currentSpell = null;
        public Spell CurrentSpell{get{return currentSpell;}}

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Player(Level level, Vector2 position, Spell spellSlotA, Spell spellSlotB, Spell spellSlotC, Spell spellSlotD)
        {
            SpellSlotA = spellSlotA;
            SpellSlotB = spellSlotB;
            SpellSlotC = spellSlotC;
            SpellSlotD = spellSlotD;

            this.level = level;

            Mana = new Mana(this);

            LoadContent();

            Reset(position);
        }

        /// <summary>
        /// Loads the player sprite sheet and sounds.
        /// </summary>
        public void LoadContent()
        {
            // Load animated textures.
            /*
            idleAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Idle"), 0.1f, true,1);
            runAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Run"), 0.1f, true,10);
            jumpAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Jump"), 0.1f, false,11);
            celebrateAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Celebrate"), 0.1f, false,11);
            dieAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Die"), 0.1f, false,12);
            */

            idleAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Idle"), 0.1f, true, 1);
            runAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Run"), 0.1f, true, 8);
            jumpAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Jump"), 0.1f, false, 4);
            celebrateAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Celebrate"), 0.1f, false, 3);
            dieAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Die"), 0.1f, false, 5);

            // Calculate bounds within texture size.            
            int width = (int)(idleAnimation.FrameWidth * 0.4);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameWidth * 0.8);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            // Load sounds.            
            killedSound = Level.Content.Load<SoundEffect>("Sounds/PlayerKilled");
            jumpSound = Level.Content.Load<SoundEffect>("Sounds/PlayerJump");
            fallSound = Level.Content.Load<SoundEffect>("Sounds/PlayerFall");
        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            isAlive = true;
            sprite.PlayAnimation(idleAnimation);
        }

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
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            Mana.update(gameTime);
            GetInput(keyboardState, gamePadState, touchState, accelState, orientation);

            ApplyPhysics(gameTime);

            if (isAlive)
            {
                //Create Spells
                HandleSpellCreation(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
            }

            if (IsAlive && (IsOnGround || disableGravity))
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0)
                {
                    sprite.PlayAnimation(runAnimation);
                }
                else
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }

            // Clear input.
            movement = 0.0f;
            isJumping = false;
        }

        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private void GetInput(
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            // Get analog horizontal movement.
            movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            // Ignore small movements to prevent running in place.
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            // Move the player with accelerometer
            if (Math.Abs(accelState.Acceleration.Y) > 0.10f)
            {
                // set our movement speed
                movement = MathHelper.Clamp(-accelState.Acceleration.Y * AccelerometerScale, -1f, 1f);

                // if we're in the LandscapeLeft orientation, we must reverse our movement
                if (orientation == DisplayOrientation.LandscapeRight)
                    movement = -movement;
            }

            // If any digital horizontal movement input is found, override the analog movement.
            if (gamePadState.IsButtonDown(LeftButton) ||
                keyboardState.IsKeyDown(LeftKey) ||
                keyboardState.IsKeyDown(LeftKeyAlternative))
            {
                movement = -1.0f;
                lastDirection.X = -1.0f;
                lastDirection.Y = 0.0f;
            }
            else if (gamePadState.IsButtonDown(RightButton) ||
                     keyboardState.IsKeyDown(RightKey) ||
                     keyboardState.IsKeyDown(RightKeyAlternative))
            {
                movement = 1.0f;
                lastDirection.X = 1.0f;
                lastDirection.Y = 0.0f;
            }
            else
            {
                movement = 0.0f;
                lastDirection.X = 0.0f;
                lastDirection.Y = 0.0f;
            }

            // Check if the player wants to jump.
            isJumping =
                gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(JumpKey) ||
                keyboardState.IsKeyDown(JumpKeyAlternative) ||
                touchState.AnyTouch();
            //Check if the player press Down Button
            isDown =
                gamePadState.IsButtonDown(DownButton) ||
                keyboardState.IsKeyDown(DownKey) ||
                keyboardState.IsKeyDown(DownKeyAlternative) ||
                touchState.AnyTouch();

            if(isJumping)
            {
                lastDirection.Y = -1.0f;
            }
            else if (isDown)
            {
                lastDirection.Y = 1.0f;
            }
            else if (lastDirection.Y != null)
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

        /// <summary>
        /// Updates the player's velocity and position based on input, gravity, etc.
        /// </summary>
        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movement * MoveAcceleration * elapsed;
            if (disableGravity)
            {
                if (isFalling) {
                    velocity.Y = 0;
                }
                else
                {
                    velocity.Y = MathHelper.Clamp(velocity.Y, -MaxFallSpeed, MaxFallSpeed);
                }
            }
            else
            {
                velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
            }

            velocity.Y = DoJump(velocity.Y, gameTime);

            // Apply pseudo-drag horizontally.
            if ((IsOnGround || disableGravity))
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);


            Position += velocity * elapsed;
            
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

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
                if ((!wasJumping && (IsOnGround||disableGravity)) || jumpTime > 0.0f)
                {
                    if (jumpTime == 0.0f)
                        jumpSound.Play();

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    sprite.PlayAnimation(jumpAnimation);
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
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

        /// <summary>
        /// Detects and resolves all collisions between the player and his neighboring
        /// tiles. When a collision is detected, the player is pushed away along one
        /// axis to prevent overlapping. There is some special logic for the Y axis to
        /// handle platforms which behave differently depending on direction of movement.
        /// </summary>
        private void HandleCollisions()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            // Reset flag to search for ground collision.
            isOnGround = false;

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    TileCollision collision = Level.GetCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = Level.GetBounds(x, y);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || collision == TileCollision.Platform)
                            {
                                // If we crossed the top of a tile, we are on the ground.
                                if (previousBottom <= tileBounds.Top)
                                    isOnGround = true;

                                // Ignore platforms, unless we are on the ground.
                                if (collision == TileCollision.Impassable || IsOnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);

                                    // Perform further collisions with the new bounds.
                                    bounds = BoundingRectangle;
                                }
                            }
                            else if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                // Resolve the collision along the X axis.
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                // Perform further collisions with the new bounds.
                                bounds = BoundingRectangle;
                            }
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            previousBottom = bounds.Bottom;
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
            isAlive = false;

            if (killedBy != null)
                killedSound.Play();
            else
                fallSound.Play();

            sprite.PlayAnimation(dieAnimation);
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
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            if (Velocity.X < 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X > 0)
                flip = SpriteEffects.None;

            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip, rotation);
        }


        KeyboardState oldKeyboardState;
        GamePadState oldGamePadState;

        /// <summary>
        /// create the spells
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboardState"></param>
        /// <param name="gamePadState"></param>
        /// <param name="touchState"></param>
        /// <param name="accelState"></param>
        /// <param name="orientation"></param>
        private void HandleSpellCreation(GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            Vector2 pos;
            pos.X = Position.X + 20 * Direction.X;
            pos.Y = Position.Y - BoundingRectangle.Height / 2;


            #region warmspell

            //pressing
            bool bCreateWarmSpellPress = (gamePadState.IsButtonDown(WarmButton) || keyboardState.IsKeyDown(WarmKey));
            if (bCreateWarmSpellPress)
            {
                if (currentSpell != null && currentSpell.GetType() != typeof(WarmSpell)) //release current creating spell if its a different one
                {
                    Debug.WriteLine("WARMSPELL:Old Spell in creation released because of spell change");
                    currentSpell.FireUp();
                    currentSpell = null;
                }
                if (currentSpell == null)
                {
                    Debug.WriteLine("WARMSPELL:START CREATION OF NEW ONE");
                    //create new warm spell
                    currentSpell = new WarmSpell("WarmSpell", pos, level);
                    if(Mana.startCastingSpell(currentSpell)) {
                        currentSpell.Direction = Direction;
                        level.addSpell(currentSpell);
                    } else {
                        currentSpell = null;
                    }
                } //if spell is already a warm spell do nothing because the spell grows on its own
                else
                {
                    Debug.WriteLine("WARMSPELL:GROW");
                    currentSpell.Direction = Direction; //update direction
                }
            }

            if (currentSpell != null && currentSpell.GetType() == typeof(WarmSpell))
              {
                if(!Mana.castingSpell(gameTime)) {
                    Debug.WriteLine("WARMSPELL:FIRED after button release");
                        currentSpell.FireUp();
                        currentSpell = null;
                } else {
                    //releasing
                    bool bWarmSpellRelease = (oldGamePadState.IsButtonDown(WarmButton) && gamePadState.IsButtonUp(WarmButton))
                                            || (oldKeyboardState.IsKeyDown(WarmKey) && keyboardState.IsKeyUp(WarmKey));

                    if (bWarmSpellRelease)
                    {
                
                            Debug.WriteLine("WARMSPELL:FIRED after button release");
                            currentSpell.FireUp();
                            currentSpell = null;
                    }
                }
            }

            #endregion

            #region coldspell

            //pressing
            bool bCreateColdSpellPress = (gamePadState.IsButtonDown(ColdButton) || keyboardState.IsKeyDown(ColdKey));
            if (bCreateColdSpellPress)
            {
                if (currentSpell != null && currentSpell.GetType() != typeof(ColdSpell)) //release current creating spell if its a different one
                {   
                    Debug.WriteLine("COLDSPELL:Old Spell in creation released because of spell change");
                    currentSpell.FireUp();
                    currentSpell = null;
                }
                if (currentSpell == null)
                {
                    Debug.WriteLine("COLDSPELL:START CREATION OF NEW ONE");
                    //create new warm spell
                    currentSpell = new ColdSpell("ColdSpell", pos, level);
                    if (Mana.startCastingSpell(currentSpell))
                    {
                        currentSpell.Direction = Direction;
                        level.addSpell(currentSpell);
                    }
                    else
                    {
                        currentSpell = null;
                    }
                    
                    
                } //if spell is already a cold spell do nothing because the spell grows on its own
                else
                {
                    Debug.WriteLine("COLDSPELL:GROW");
                    currentSpell.Direction = Direction; //update direction
                }
            }

            if (currentSpell != null && currentSpell.GetType() == typeof(ColdSpell))
            {
                if (!Mana.castingSpell(gameTime))
                {
                    Debug.WriteLine("COLDSPELL:FIRED after mana ios empty");
                    currentSpell.FireUp();
                    currentSpell = null;
                }
                else
                {
                    //releasing
                    bool bColdSpellRelease = (oldGamePadState.IsButtonDown(ColdButton) && gamePadState.IsButtonUp(ColdButton))
                                        || (oldKeyboardState.IsKeyDown(ColdKey) && keyboardState.IsKeyUp(ColdKey));

                    if (bColdSpellRelease)
                    {
                        Debug.WriteLine("COLDSPELL:FIRED after button release");
                        currentSpell.FireUp();
                        currentSpell = null;
                    }
                }
            }

            #endregion

            #region matterspell

            //pressing
            bool bCreateMatterSpellPress = (gamePadState.IsButtonDown(MatterButton) || keyboardState.IsKeyDown(MatterKey));
            if (bCreateMatterSpellPress)
            {
                if (currentSpell != null && currentSpell.GetType() != typeof(MatterSpell)) //release current creating spell if its a different one
                {
                    Debug.WriteLine("MATTERSPELL:Old Spell in creation released because of spell change");
                    currentSpell.FireUp();
                    currentSpell = null;
                }
                if (currentSpell == null)
                {
                    Debug.WriteLine("MATTERSPELL:START CREATION OF NEW ONE");
                    //create new matter spell
                    currentSpell = new MatterSpell("MatterSpell", pos, level);
                    if(Mana.startCastingSpell(currentSpell)) {
                    currentSpell.Direction = Direction;
                    level.addSpell(currentSpell);
                    } else {
                        currentSpell = null;
                    }
                } //if spell is already a cold spell do nothing because the spell grows on its own
                else
                {
                    Debug.WriteLine("MATTERSPELL:GROW");
                    currentSpell.Direction = Direction; //update direction
                }
            }

            if (currentSpell != null && currentSpell.GetType() == typeof(MatterSpell))
            {
                if(!Mana.castingSpell(gameTime)) {

                    Debug.WriteLine("COLDSPELL:FIRED after button release");
                            currentSpell.FireUp();
                            currentSpell = null;
                } else {
                    //releasing
                    bool bMatterSpellRelease = (oldGamePadState.IsButtonDown(MatterButton) && gamePadState.IsButtonUp(MatterButton))
                                            || (oldKeyboardState.IsKeyDown(MatterKey) && keyboardState.IsKeyUp(MatterKey));

                    if (bMatterSpellRelease)
                    {
                
                            Debug.WriteLine("COLDSPELL:FIRED after button release");
                            currentSpell.FireUp();
                            currentSpell = null;
                    }
                }
            }

            #endregion

            #region "noGravitySpell"
            //pressing
            bool bCreateNoGravitySpell = (gamePadState.IsButtonDown(GravityButton) || keyboardState.IsKeyDown(GravityKey));
            if (bCreateNoGravitySpell)
            {
                if (currentSpell != null && currentSpell.GetType() != typeof(NoGravitySpell)) //release current creating spell if its a different one
                {
                    Debug.WriteLine("noGravitySpell:Old Spell in creation released because of spell change");
                    currentSpell.FireUp();
                    currentSpell = null;
                }
                if (currentSpell == null)
                {
                    Debug.WriteLine("noGravitySpell:START CREATION OF NEW ONE");
                    //create new matter spell
                    currentSpell = new NoGravitySpell("NoGravitySpell", pos, level);
                    if(Mana.startCastingSpell(currentSpell)) {
                        currentSpell.Direction = Direction;
                        level.addSpell(currentSpell);
                    } else {
                        currentSpell = null;
                    }
                } //if spell is already a cold spell do nothing because the spell grows on its own
                else
                {
                    Debug.WriteLine("noGryvitySpell:GROW");
                    currentSpell.Direction = Direction; //update direction
                }
            }

            if (currentSpell != null && currentSpell.GetType() == typeof(NoGravitySpell))
            {
                if (!Mana.castingSpell(gameTime))
                {
                    Debug.WriteLine("NoGravitySpell:FIRED after button release");
                    currentSpell.FireUp();
                    currentSpell = null;
                } else {
                    //releasing
                    bool bNoGravitySpellRelease = (oldGamePadState.IsButtonDown(GravityButton) && gamePadState.IsButtonUp(GravityButton))
                                            || (oldKeyboardState.IsKeyDown(GravityKey) && keyboardState.IsKeyUp(GravityKey));

                    if (bNoGravitySpellRelease)
                    {

                        Debug.WriteLine("NoGravitySpell:FIRED after button release");
                        currentSpell.FireUp();
                        currentSpell = null;
                    }
                }
            }
            #endregion

            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
        }


        #region ISpellInfluenceable Member

        public bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(NoGravitySpell))
            {
                disableGravity = true;                
            }
            return false; //do not remove spell
        }

        #endregion
    }
}
