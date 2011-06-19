using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// a basic door implementation, could be pased to a Switch
    /// open and close are only possible via IActivation Interface
    /// start position is the close position on openning the door is moved ahead 
    /// </summary>
    class Door:BlockElement,IActivation
    {

        /// <summary>
        /// division ratio of texture height for moveing the door in open or close position 
        /// </summary>
        static float OpenCloseSpeedFactor=2f;

        bool open;

        public bool Open
        {
            get { return open; }
        }

        bool closed;

        public bool Closed
        {
            get { return closed; }
        }

        Vector2 closedPosition;
        Vector2 openedPosition;

        public Door(String texture, Level level, Vector2 position)
            :base(texture,CollisionType.Impassable,level,position)
        {
            closedPosition = position;
            openedPosition = position + new Vector2(0, -Texture.Height);
            closed = true;
            open = false;
        }

        public override void Update(GameTime gameTime)
        {
            float movement = Texture.Height * OpenCloseSpeedFactor*(float)gameTime.ElapsedGameTime.TotalSeconds;
            //open the door step by step
            if (open && position.Y > openedPosition.Y)
            {
                Position += new Vector2(0, -movement);
            }
            //close the door step by step
            else if (closed && position.Y < closedPosition.Y)
            {
                Position += new Vector2(0, movement);
            }
            base.Update(gameTime);
        }


        #region IActivation Member
        /// <summary>
        /// open command
        /// </summary>
        public void activate()
        {
            open = true;
            closed = false;

        }

        /// <summary>
        /// close command
        /// </summary>
        public void deactivate()
        {
            closed = true;
            open = false;
        }

        #endregion
    }
}
