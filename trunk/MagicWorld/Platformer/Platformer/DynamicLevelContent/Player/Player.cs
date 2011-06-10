using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
using MagicWorld.Spells;
using MagicWorld.DynamicLevelContent.Player;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;
using System.Collections.Generic;
using MagicWorld.Controls;
using MagicWorld.Constants;

namespace MagicWorld
{
    /// <summary>
    /// Our fearless adventurer!
    /// </summary>
    public class Player : BasicGameElement
    {
        #region input constants 
        //TODO PUT INTO CLASS/INTERFACE
        public const Keys FullscreenToggleKey = Keys.F11;
        public const Keys ExitGameKey = Keys.Escape;

        public const Keys DebugToggleKey = Keys.F3;
        public const Keys DEBUG_NO_MANA_COST = Keys.F2;
        public const Keys DEBUG_NEXT_LEVEL = Keys.F4;
        public const Keys DEBUG_TOGGLE_GRAVITY_INFLUECE_ON_PLAYER = Keys.F5;
        
        // Input configuration
        private const float MoveStickScale = 1.0f;

        #endregion

        public SpellType[] UsableSpells { get; private set; }


        #region "Animation & sound"


        // Animations
        //TODO: Draw the die Animation :(
        private Animation dieAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private AnimationPlayer sprite;
        private float rotation = 0.0f;

        private Animation runLeftAnimation;
        private Animation runRightAnimation;
        private Animation jumpLeftAnimation;
        private Animation jumpRightAnimation;
        private Animation idleAnimation;
        private Animation celebrateAnimation;

        // Sounds
        private SoundEffect killedSound;
        private SoundEffect jumpSound;
        private SoundEffect fallSound;
        private SoundEffect spellSound;

        public SoundEffect SpellSound { get { return spellSound; } }

        #endregion

        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        public override Bounds Bounds
        {
            get
            {
                // Calculate bounds within texture size.
                float width = (sprite.Animation.FrameWidth * 0.75f);
                float height = (sprite.Animation.FrameHeight * 0.9f);
                return new Bounds(position, width, height);
            }
        }

        // Physics state
        public override Vector2 Position
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

        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

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

        public bool nogravityHasInfluenceOnPlayer = true;


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
        /// Constructors a new player.
        /// </summary>
        public Player(Level level, Vector2 position, SpellType[] useableSpells)
            : base(level)
        {
            this.UsableSpells = useableSpells;
            this.level = level;

            Mana = new Mana(this);

            LoadContent();

            Reset(position);

            debugColor = Color.Violet;

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
            //TODO: Use the real animations
            dieAnimation = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 4);
            celebrateAnimation = new Animation("Content/Sprites/Player/PlayerSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/Player/PlayerSpriteSheet"), 4);

            // Load sounds.            
            killedSound = level.Content.Load<SoundEffect>("Sounds/PlayerKilled");
            jumpSound = level.Content.Load<SoundEffect>("Sounds/PlayerJump");
            fallSound = level.Content.Load<SoundEffect>("Sounds/PlayerFall");
            spellSound = level.Content.Load<SoundEffect>("Sounds/CreateSpell");
            base.LoadContent("");
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
            DisplayOrientation orientation)
        {
            Mana.update(gameTime);
            GetInput(keyboardState, gamePadState, orientation);

            ApplyPhysics(gameTime);

            if (isAlive)
            {
                //Create Spells
                HandleSpellCreation(gameTime, keyboardState, gamePadState, orientation);
            }

            if (IsAlive && (IsOnGround || (disableGravity && gravityInfluenceMaxTime > 0)))
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0)//player is running and not just falling/sliding
                {
                    if (Velocity.X > 0)
                        sprite.PlayAnimation(runRightAnimation);
                    else
                        sprite.PlayAnimation(runLeftAnimation);
                }
                else
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }

            gravityInfluenceMaxTime -= gameTime.ElapsedGameTime.TotalMilliseconds;

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
            DisplayOrientation orientation)
        {

            IPlayerControl controls = PlayerControlFactory.GET_INSTANCE().getPlayerControl();

            // Get analog horizontal movement.
            movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            // Ignore small movements to prevent running in place.
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            // If any digital horizontal movement input is found, override the analog movement.
            if (gamePadState.IsButtonDown(controls.GamePad_Left) ||
                keyboardState.IsKeyDown(controls.Keys_Left))
            // ||keyboardState.IsKeyDown(LeftKeyAlternative))
            {
                movement = -1.0f;
                lastDirection.X = -1.0f;
            }
            else if (gamePadState.IsButtonDown(controls.GamePad_Left) ||
                     keyboardState.IsKeyDown(controls.Keys_Right))
            //keyboardState.IsKeyDown(RightKeyAlternative))
            {
                movement = 1.0f;
                lastDirection.X = 1.0f;
            }
            else
            {
                movement = 0.0f;
                lastDirection.X = 0.0f;
                lastDirection.Y = 0.0f;
            }

            // Check if the player wants to jump.
            isJumping =
                gamePadState.IsButtonDown(controls.GamePad_Jump) ||
                keyboardState.IsKeyDown(controls.Keys_Jump);
            //Check if the player press Down Button
            isDown =
                gamePadState.IsButtonDown(controls.GamePad_Down) ||
                keyboardState.IsKeyDown(controls.Keys_Down);

            if (isJumping)
            {
                lastDirection.Y = -1.0f;
            }
            else if (isDown)
            {
                lastDirection.Y = 1.0f;
            }
            else if (lastDirection.Y != 0.0f)
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
            velocity.X += movement * PhysicValues.PLAYER_MOVE_ACCELERATION * elapsed;
            if (disableGravity && gravityInfluenceMaxTime > 0)
            {
                if (isFalling)
                {
                    velocity.Y = 0;
                }
                else
                {
                    velocity.Y = MathHelper.Clamp(velocity.Y, -PhysicValues.PLAYER_MAX_FALL_SPEED, PhysicValues.PLAYER_MAX_FALL_SPEED);
                }
            }
            else
            {
                velocity.Y = MathHelper.Clamp(velocity.Y + PhysicValues.PLAYER_GRAVITY_ACCELERATIOPM * elapsed, -PhysicValues.PLAYER_MAX_FALL_SPEED, PhysicValues.PLAYER_MAX_FALL_SPEED);
            }

            velocity.Y = DoJump(velocity.Y, gameTime);

            // Apply pseudo-drag horizontally.
            if ((IsOnGround || (disableGravity && gravityInfluenceMaxTime > 0)))
                velocity.X *= PhysicValues.PLAYER_GROUND_DRAG_FACTOR;
            else
                velocity.X *= PhysicValues.PLAYER_AIR_DRAG_FACTOR;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -PhysicValues.PLAYER_MAX_MOVE_SPEED, PhysicValues.PLAYER_MAX_MOVE_SPEED);

            if (IsCasting)
            {
                Position += velocity * elapsed * PhysicValues.SLOW_MOTION_FACTOR;
                Debug.WriteLine("PHYSICSPLAYER " +( velocity * elapsed * PhysicValues.SLOW_MOTION_FACTOR));
            }
            else
            {
                Position += velocity * elapsed;
                Debug.WriteLine("PHYSICSPLAYER " + (velocity * elapsed));
            }
            

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
                if ((!wasJumping && (IsOnGround || (disableGravity && gravityInfluenceMaxTime > 0))) || jumpTime > 0.0f)
                {
                    if (jumpTime == 0.0f)
                        jumpSound.Play();

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (Velocity.X > 0)
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

        /// <summary>
        /// Detects and resolves all collisions between the player and his neighboring
        /// tiles. When a collision is detected, the player is pushed away along one
        /// axis to prevent overlapping. There is some special logic for the Y axis to
        /// handle platforms which behave differently depending on direction of movement.
        /// </summary>
        private void HandleCollisions()
        {
            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();
            level.CollisionManager.CollidateWithGeneralLevelElements(this, ref collisionObjects);

            //// Reset flag to search for ground collision.
            IsOnGround = false;

            foreach (BasicGameElement t in collisionObjects)
            {
                CollisionType collision = t.Collision;
                if (collision == CollisionType.Impassable || collision == CollisionType.Platform)
                {
                    Vector2 depth = CollisionManager.GetCollisionDepth(this, t);
                    if (depth != Vector2.Zero)
                    {
                        float absDepthX = Math.Abs(depth.X);
                        float absDepthY = Math.Abs(depth.Y);

                        // Resolve the collision along the shallow axis.
                        if (absDepthY < absDepthX || collision == CollisionType.Platform)
                        {
                            // If we crossed the top of a tile, we are on the ground.
                            if (previousBottom <= t.Bounds.getRectangle().Top)
                                IsOnGround = true;

                            // Ignore platforms, unless we are on the ground.
                            if (collision == CollisionType.Impassable || IsOnGround)
                            {
                                if (depth.Y < 0 && velocity.Y > 0 || depth.Y >= 0 && velocity.Y < 0)
                                {
                                    // Resolve the collision along the Y axis.
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);
                                }
                                //Debug.WriteLine("Velocity Y " + velocity.Y);
                                //Debug.WriteLine("Depth Y " + depth.Y);
                            }
                        }
                        else if (collision == CollisionType.Impassable && velocity != Vector2.Zero) // Ignore platforms. //only handles this if player objects is in move
                        {
                            // Resolve the collision along the X axis.
                            Position = new Vector2(Position.X + depth.X, Position.Y);
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            previousBottom = Bounds.getRectangle().Bottom;
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
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            //if (Velocity.X < 0)
            //    flip = SpriteEffects.FlipHorizontally;
            //else if (Velocity.X > 0)
            //    flip = SpriteEffects.None;

            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip, rotation);

            base.Draw(gameTime, spriteBatch);
        }


        KeyboardState oldKeyboardState;
        GamePadState oldGamePadState;

        private int selectedSpellIndex_A = 0;
        private int selectedSpellIndex_B = 1;
        public SpellType selectedSpell_A { get { return UsableSpells[selectedSpellIndex_A]; } }
        public SpellType selectedSpell_B { get { return UsableSpells[selectedSpellIndex_B]; } }

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
            DisplayOrientation orientation)
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
                else if (gamePadState.IsButtonDown(controls.GamePad_SelectedSpellA) || keyboardState.IsKeyUp(controls.Keys_SelectedSpellA) && oldKeyboardState.IsKeyDown(controls.Keys_SelectedSpellA)) // spell A select
                {
                    selectedSpellIndex_A = selectNextSpell(selectedSpellIndex_A);
                    Debug.WriteLine("changed selection for SpellSlot A: " + System.Enum.GetName(typeof(SpellType), selectedSpellIndex_A));
                }
                else if (gamePadState.IsButtonDown(controls.GamePad_SelectedSpellB) || keyboardState.IsKeyUp(controls.Keys_SelectedSpellB) && oldKeyboardState.IsKeyDown(controls.Keys_SelectedSpellB)) // spell B select
                {
                    selectedSpellIndex_B = selectNextSpell(selectedSpellIndex_B);
                    Debug.WriteLine("changed selection for SpellSlot B: " + System.Enum.GetName(typeof(SpellType), selectedSpellIndex_B));
                }
            }
            else
            {
                if (this.isSpellAButtonPressed(controls, gamePadState, keyboardState) || this.isSpellBButtonPressed(controls, gamePadState, keyboardState))
                {
                    isCastingSpell = SpellCreationManager.furtherSpellCasting(this, this.level, gameTime);
                }
                else
                {
                    SpellCreationManager.releaseSpell(this);
                }
            }

            oldKeyboardState = keyboardState;
            oldGamePadState = gamePadState;
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
    }
}
