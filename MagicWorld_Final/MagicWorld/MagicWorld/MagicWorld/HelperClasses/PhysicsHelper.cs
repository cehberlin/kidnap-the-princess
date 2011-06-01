using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.AbstractGameElements;
using Microsoft.Xna.Framework;

namespace MagicWorld.HelperClasses
{
    class PhysicsHelper
    {
        public static void  ApplyGravity(ref DynamicGameElement element,GameTime gameTime){
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = element.Position;


            //TODO

            //// Base velocity is a combination of horizontal movement control and
            //// acceleration downward due to gravity.
            //velocity.X += movement * MoveAcceleration * elapsed;
            //if (disableGravity && gravityInfluenceMaxTime > 0)
            //{
            //    if (isFalling)
            //    {
            //        velocity.Y = 0;
            //    }
            //    else
            //    {
            //        velocity.Y = MathHelper.Clamp(velocity.Y, -MaxFallSpeed, MaxFallSpeed);
            //    }
            //}
            //else
            //{
            //    velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
            //}

            //velocity.Y = DoJump(velocity.Y, gameTime);

            //// Apply pseudo-drag horizontally.
            //if ((IsOnGround || (disableGravity && gravityInfluenceMaxTime > 0)))
            //    velocity.X *= GroundDragFactor;
            //else
            //    velocity.X *= AirDragFactor;

            //// Prevent the player from running faster than his top speed.            
            //velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);


            //Position += velocity * elapsed;

            //Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
        }
    }
}
