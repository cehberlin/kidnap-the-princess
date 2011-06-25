using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent;
using System.Collections.Generic;
using MagicWorld.Constants;
using System.Diagnostics;
using MagicWorld.Services;
using MagicWorld.Gleed2dLevelContent;
using MagicWorld.Spells;

namespace MagicWorld.StaticLevelContent
{
    class SpellMoveablePlatform : Platform
    {
        private Vector2 currentPathPosition;
        private Vector2 nextPathPosition;
        private int pathPosition = 0;
        private int nextPosition = 1;
        private float steps = 0;
        private const float MoveSpeed = 150;
        private float movementSpeedX = 0;
        private float movementSpeedY = 0;
        private PathItem path = null;
        private Bounds oldBounds = null;
        private float acceleration = 1;

        private Boolean isPushed;
        private Boolean isPulled;
        protected PushPullHandler pushPullHandler = new PushPullHandler();


        public SpellMoveablePlatform(String texture, CollisionType collision, Level level, Vector2 position, PathItem path)
            : base(texture, collision, level, position)
        {
            currentPathPosition = path.WorldPoints[pathPosition];
            nextPathPosition = path.WorldPoints[nextPosition];
            this.path = path;
            oldBounds = this.Bounds;
        }

        public override void LoadContent(string spriteSet)
        {
            base.LoadContent(spriteSet);
        }

        #region Update
        public override void Update(GameTime gameTime)
        {
            pushPullHandler.Update(gameTime, currentPathPosition, nextPathPosition);
            //if (isPushed && Position.X < nextPathPosition.X)
            //{
            //    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    // Calculates the delta for x and y axis
            //    Vector2 velocity = calculatesVelocity();
            //    velocity = new Vector2(movementSpeedX, movementSpeedY);
            //    Position = Position + velocity * elapsed * acceleration;
            //    currentPushingTime = currentPushingTime.Add(gameTime.ElapsedGameTime);
            //    if (currentPushingTime >= SpellInfluenceValues.maxPushingTime && isPushed)
            //    {
            //        isPushed = false;
            //    }
            //}
            //else if (isPulled && Position.X > currentPathPosition.X)
            //{
            //    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    // Calculates the delta for x and y axis
            //    Vector2 velocity = calculatesVelocity();
            //    velocity = new Vector2(-movementSpeedX, movementSpeedY);
            //    Position = Position + velocity * elapsed * acceleration;
            //    currentPullingTime = currentPullingTime.Add(gameTime.ElapsedGameTime);
            //    if (currentPullingTime >= SpellInfluenceValues.maxPullingTime && isPulled)
            //    {
            //        isPulled = false;
            //    }
            //}
        }

        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        void setNextPath()
        {
            if (pathPosition >= path.WorldPoints.Length - 1)
                pathPosition = 0;
            else
                pathPosition++;
            if (nextPosition >= path.WorldPoints.Length - 1)
                nextPosition = 0;
            else
                nextPosition++;
        }

        private Vector2 calculatesVelocity(Boolean push)
        {
            float deltaX = 0;
            float deltaY = 0;
            deltaX = Math.Abs(currentPathPosition.X) - Math.Abs(nextPathPosition.X);
            deltaX = Math.Abs(deltaX);
            deltaY = Math.Abs(currentPathPosition.Y) - Math.Abs(nextPathPosition.Y);
            deltaY = Math.Abs(deltaY);
            if (deltaX > deltaY)
            {
                steps = deltaX / MoveSpeed;
                if (currentPathPosition.X > nextPathPosition.X)
                    movementSpeedX = -MoveSpeed;
                else
                    movementSpeedX = MoveSpeed;
                if (currentPathPosition.Y > nextPathPosition.Y)
                    movementSpeedY = deltaY / steps * -1;
                else
                    movementSpeedY = deltaY / steps;
            }
            else if (deltaY > deltaX)
            {
                steps = deltaY / MoveSpeed;
                if (currentPathPosition.Y > nextPathPosition.Y)
                    movementSpeedY = -MoveSpeed;
                else
                    movementSpeedY = MoveSpeed;
                movementSpeedX = deltaX / steps;
            }
            else if (deltaY == deltaX)
            {
                steps = deltaX / MoveSpeed;
                if (currentPathPosition.X > nextPathPosition.X)
                    movementSpeedX = -MoveSpeed;
                else
                    movementSpeedX = MoveSpeed;

                if (currentPathPosition.Y > nextPathPosition.Y)
                    movementSpeedY = -MoveSpeed;
                else
                    movementSpeedY = MoveSpeed;
            }
            if(!push)
            {
                movementSpeedX = -movementSpeedX;
            }
            return new Vector2(movementSpeedX, movementSpeedY);
        }


        private TimeSpan currentPullingTime = new TimeSpan(0, 0, 0);

        private TimeSpan currentPushingTime = new TimeSpan(0, 0, 0);

        double spellDurationOfActionMs = 0;

        public override Boolean SpellInfluenceAction(Spell spell)
        {
            if (spell.SpellType == SpellType.PushSpell)
            {                
                Vector2 push = calculatesVelocity(true);
                push.Normalize();
                pushPullHandler.setXAcceleration(SpellConstantsValues.PUSHPULL_DEFAULT_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);                
                pushPullHandler.start(this,1000, push);
                return false;
            }
            else if (spell.SpellType == SpellType.PullSpell)
            {
                Vector2 pull = calculatesVelocity(false);
                pull.Normalize();
                pushPullHandler.setXAcceleration(SpellConstantsValues.PUSHPULL_DEFAULT_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);                           
                pushPullHandler.start(this, 1000,pull);
                return false;
            }
            return base.SpellInfluenceAction(spell);
        }
    }
}
