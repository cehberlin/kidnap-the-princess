using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld
{
    /// <summary>
    /// Interface which encapsulates default Load Update Draw behavior of a autonomus object (all objects which handle this on its own)
    /// exception is the player!the player is another type of special gameobjects
    /// </summary>
    interface IAutonomusGameObject
    {
        void LoadContent(string spriteSet);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}
