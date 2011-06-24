using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.Constants;
using MagicWorld.StaticLevelContent;
using MagicWorld.Spells;
using MagicWorld.DynamicLevelContent.SwitchRiddles;

namespace MagicWorld.DynamicLevelContent
{
    /// <summary>
    /// block element with gravity influence (if activated)
    /// could be activated by member access or by IActivation Interface
    /// </summary>
    class GravityElement : BlockElement,IActivation
    {
        Bounds oldBounds;
        protected bool isOnGround = false;

        protected bool enableGravity;

        public bool EnableGravity
        {
            get { return enableGravity; }
            set { enableGravity = value; }
        }

        protected bool checkCollision;

        public bool CheckCollision
        {
            get { return checkCollision; }
            set { checkCollision = value; }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="texture">texture for this object</param>
        /// <param name="collision">collision type</param>
        /// <param name="level">reference to level</param>
        /// <param name="position">startposition</param>
        /// <param name="enableGravity">true=object collision is checked,makes sense in combination with enabled gravity</param>
        /// <param name="enableGravity">true=object is influenced by gravity; false only influence by push and pull</param>
        public GravityElement(String texture, CollisionType collision, Level level, Vector2 position, bool checkCollision=false, bool enableGravity = false)
            : base (texture, collision, level, position)
        {
            this.enableGravity = enableGravity;
            this.checkCollision = checkCollision;
            oldBounds = this.Bounds;
        }

        public override void Update(GameTime gameTime)
        {  
            
            if(enableGravity){
                level.PhysicsManager.ApplyGravity(this, PhysicValues.DEFAULT_GRAVITY, gameTime);
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = Position + velocity * elapsed;

            if (checkCollision)
            {
                level.CollisionManager.HandleGeneralCollisions(this, ref oldBounds, ref isOnGround);

                if (isOnGround)
                {
                    ResetVelocity();
                }
            }
 
        }

        protected void ResetVelocity()
        {
            velocity = Vector2.Zero;
        }

        #region IActivation Member

        public void activate()
        {
            enableGravity = true;
        }

        public void deactivate()
        {
            enableGravity = false;
        }

        #endregion
    }
}
