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
        private Vector2 oldPathPosition;
        private int oldPosition = 0;
        private int pathPosition = 0;
        private int nextPosition = 1;
        private float steps = 0;
        private const float MoveSpeed = 150;
        private float movementSpeedX = 0;
        private float movementSpeedY = 0;
        private PathItem path = null;
        private Bounds oldBounds = null;
        private Bounds currentBounds = null;

        protected PushPullHandler pushPullHandler = new PushPullHandler();

        Vector2 lastMovement = Vector2.Zero;

        public Vector2 LastMovement
        {
            get { return lastMovement; }
        }


        public SpellMoveablePlatform(String texture, CollisionType collision, Level level, Vector2 position, PathItem path, Color drawColor)
            : base(texture, collision, level, position, drawColor)
        {
            currentPathPosition = path.WorldPoints[pathPosition];
            nextPathPosition = path.WorldPoints[nextPosition];
            currentBounds = new Bounds(this.Position, Width, Height);
            oldBounds = currentBounds;
        }

        public override void LoadContent(string spriteSet)
        {
            base.LoadContent(spriteSet);
        }

        #region Update
        public override void Update(GameTime gameTime)
        {
            Vector2 lastPosition = Position;
            pushPullHandler.Update(gameTime, currentPathPosition, nextPathPosition, currentBounds);
            if (level.Player.IsOnGround && level.CollisionManager.CollidateWithPlayer(this))
            {
                level.Player.Position += Position - lastPosition;
            }
        }

        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        void setNextPath()
        {
            if (pathPosition >= path.WorldPoints.Length - 1)
            {
                pathPosition = 0;
            }
            else
            {
                pathPosition++;
            }
            if (nextPosition >= path.WorldPoints.Length - 1)
            {
                nextPosition = 0;
            }
            else
            {
                nextPosition++;
            }
            oldPosition = pathPosition - 1;
            if(oldPosition <0){
                oldPosition = 0;
            }

            currentPathPosition = path.WorldPoints[pathPosition];
            nextPathPosition = path.WorldPoints[nextPosition];
            oldPathPosition = path.WorldPoints[oldPosition];
        }

        private Vector2 calculatesVelocity(Boolean push)
        {
            float deltaX = 0;
            float deltaY = 0;
            deltaX = currentPathPosition.X - nextPathPosition.X;
            deltaX = Math.Abs(deltaX);
            deltaY = currentPathPosition.Y - nextPathPosition.Y;
            deltaY = Math.Abs(deltaY);
            if (deltaX > deltaY)
            {
                steps = deltaX / MoveSpeed;
                if (currentPathPosition.X > nextPathPosition.X)
                    movementSpeedX = MoveSpeed;
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
                    movementSpeedY = -MoveSpeed;
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
            if (!push)
            {
                movementSpeedX = -movementSpeedX;
                movementSpeedY = -movementSpeedY;
            }
            return new Vector2(movementSpeedX, movementSpeedY);
        }


        private TimeSpan currentPullingTime = new TimeSpan(0, 0, 0);

        private TimeSpan currentPushingTime = new TimeSpan(0, 0, 0);

        public override Boolean SpellInfluenceAction(Spell spell)
        {
            if (spell.SpellType == SpellType.PushSpell)
            {  
                Vector2 push = calculatesVelocity(true);
                push.Normalize();
                pushPullHandler.setXAcceleration(SpellConstantsValues.PUSHPULL_MOVEABEL_PLATFORMS_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);
                pushPullHandler.setYAcceleration(SpellConstantsValues.PUSHPULL_MOVEABEL_PLATFORMS_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);

                if (this.position.X + this.Bounds.Width / 2 < spell.Position.X)
                {
                    push.X = -push.X;
                }
                if (this.position.Y >= spell.Position.Y)
                {
                    push.Y = -push.Y;
                }
                
                pushPullHandler.start(this,1000, push);
                return false;
            }
            else if (spell.SpellType == SpellType.PullSpell)
            {
                Vector2 pull = calculatesVelocity(false);
                pull.Normalize();
                pushPullHandler.setXAcceleration(SpellConstantsValues.PUSHPULL_MOVEABEL_PLATFORMS_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);
                pushPullHandler.setYAcceleration(SpellConstantsValues.PUSHPULL_MOVEABEL_PLATFORMS_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);

                if (this.position.X + this.Bounds.Width / 2 < spell.Position.X)
                {
                    pull.X = -pull.X;
                }
                if (this.position.Y > spell.Position.Y)
                {
                    pull.Y = -pull.Y;
                }

                pushPullHandler.start(this, 1000,pull);
                return false;
            }
            return base.SpellInfluenceAction(spell);
        }
    }
}
