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
    class UpDownDoor:AbstractDoor
    {
        /// <summary>
        /// division ratio of texture height for moveing the door in open or close position 
        /// </summary>
        static float OpenCloseSpeedFactor=2f;

        Vector2 closedPosition;
        Vector2 openedPosition;

        public UpDownDoor(String texture, Level level, Vector2 position,bool opened=false)
            :base(texture,level,position,opened)
        {
            closedPosition = position;
            openedPosition = position + new Vector2(0, -Texture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            float movement = Texture.Height * OpenCloseSpeedFactor*(float)gameTime.ElapsedGameTime.TotalSeconds;
            //open the door step by step
            if (Open && position.Y > openedPosition.Y)
            {
                Position += new Vector2(0, -movement);
            }
            //close the door step by step
            else if (Closed && position.Y < closedPosition.Y)
            {
                Position += new Vector2(0, movement);
            }
            base.Update(gameTime);
        }
    }
}
