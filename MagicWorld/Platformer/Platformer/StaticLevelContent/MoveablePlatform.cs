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

namespace MagicWorld.StaticLevelContent
{
    class MoveablePlatform : Platform
    {
        private float deltaX = 0;
        private float deltaY = 0;
        private Vector2 currentPathPosition;
        private Vector2 nextPathPosition;
        private int pathPosition = 0;
        private int nextPosition = 1;
        private float steps = 0;
        private const float MoveSpeed = 64.0f;
        private float movementSpeedX = 0;
        private float movementSpeedY = 0;
        private PathItem path = null;
        private Bounds oldBounds = null;
        private float acceleration = 1;


        public MoveablePlatform(String texture, CollisionType collision, Level level, Vector2 position, PathItem path)
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
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            deltaX = 0;
            deltaY = 0;
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

            velocity = new Vector2(movementSpeedX, movementSpeedY);

            // Move in the current direction.

            #region Platform Movement
            //from left to right
            if (currentPathPosition.X > nextPathPosition.X && currentPathPosition.Y == nextPathPosition.Y)
            {
                if (pathPosition <= path.WorldPoints.Length - 1 && Position.X < nextPathPosition.X - oldBounds.Width / 2)
                {
                    setNextPath();
                }
                //from bottom to top
            }
            else if (currentPathPosition.X == nextPathPosition.X && currentPathPosition.Y > nextPathPosition.Y)
            {
                if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y < nextPathPosition.Y - oldBounds.Height / 2)
                {
                    setNextPath();
                }

            }
            //from right to left
            else if (currentPathPosition.X < nextPathPosition.X && currentPathPosition.Y == nextPathPosition.Y)
            {
                if (pathPosition <= path.WorldPoints.Length - 1 && Position.X > nextPathPosition.X - oldBounds.Width / 2)
                {
                    setNextPath();
                }
            }
            //from top to bottom
            else if (currentPathPosition.X == nextPathPosition.X && currentPathPosition.Y < nextPathPosition.Y)
            {
                if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y > nextPathPosition.Y - oldBounds.Height / 2)
                {
                    setNextPath();
                }
            }
            //from lower left to upper right
            else if (currentPathPosition.X < nextPathPosition.X && currentPathPosition.Y > nextPathPosition.Y)
            {
                if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y < nextPathPosition.Y - oldBounds.Height / 2
                    && Position.X > nextPathPosition.X - oldBounds.Width / 2)
                {
                    setNextPath();
                }
            }
            //from top left to lower right
            else if (currentPathPosition.X < nextPathPosition.X && currentPathPosition.Y < nextPathPosition.Y)
            {
                if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y > nextPathPosition.Y - oldBounds.Height / 2
                    && Position.X > nextPathPosition.X - oldBounds.Width / 2)
                {
                    setNextPath();
                }
            }
            //from top right to lower left
            else if (currentPathPosition.X > nextPathPosition.X && currentPathPosition.Y < nextPathPosition.Y)
            {
                if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y > nextPathPosition.Y - oldBounds.Height / 2
                    && Position.X < nextPathPosition.X - oldBounds.Width / 2)
                {
                    setNextPath();
                }
            }
            //from lower right to top right
            else if (currentPathPosition.X > nextPathPosition.X && currentPathPosition.Y > nextPathPosition.Y)
            {
                if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y < nextPathPosition.Y - oldBounds.Height / 2
                    && Position.X < nextPathPosition.X - oldBounds.Width / 2)
                {
                    setNextPath();
                }
            }
            #endregion Platform Movement

            currentPathPosition = path.WorldPoints[pathPosition];
            nextPathPosition = path.WorldPoints[nextPosition];
            Position = Position + velocity * elapsed * acceleration;

            int left = (int)Math.Round(Position.X);
            int top = (int)Math.Round(Position.Y);

            int yOffset = 10;
            this.bounds = new Bounds(left + yOffset / 2, top, Width - yOffset, 20);
            DrawRec = new Rectangle(left, top, Width, Height);
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

        public Vector2 getPosition()
        {
            return position;
        }
    }
}
