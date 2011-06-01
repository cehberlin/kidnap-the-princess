using System;
using System.Collections.Generic;
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

        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        Vector2 direction;


        public float Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        float velocity;


        public DynamicGameElement(ContentManager contentManager, Vector2 position) : 
            base(contentManager, position)
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
