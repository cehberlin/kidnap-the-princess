using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.AbstractGameElements.BasicShapes;
using Microsoft.Xna.Framework.Content;

namespace MagicWorld.AbstractGameElements
{
    /// <summary>
    /// dynamic element in the game world
    /// an element that can change its state
    /// </summary>
    public abstract class DynamicGameElement : DrawableGameElement, IUpdateable
    {

        public DynamicGameElement(ContentManager contentManager, Vector2 position, Entity bounds) : 
            base(contentManager, position, bounds)
        { 

        }


        #region "IUpdateable"

        public bool Enabled
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public int UpdateOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}
