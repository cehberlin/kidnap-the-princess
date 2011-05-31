using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MagicWorld.AbstractGameElements.BasicShapes;

namespace MagicWorld.AbstractGameElements
{
    /// <summary>
    /// generic element in the game world 
    /// the element can be drawn to the screen and has defined boundaries for collision detection
    /// </summary>
    public abstract class DrawableGameElement : IDrawable
    {
        /// <summary>
        /// Bounds for collision check
        /// </summary>
        /// <returns></returns>
        public Entity BoundingShape{get; set;}

        private Vector2 position;
        /// <summary>
        /// upper left Position of the object
        /// </summary>
        /// <returns></returns>
        public Vector2 Position { 
            get { return position; } 
            set 
            {
                position = value;
                BoundingShape.Position = position;
            } 
        }


        public ContentManager ContentManager { get; set; }


        public DrawableGameElement(ContentManager contentManager, Vector2 position, Entity bounds)
        {
            this.BoundingShape = bounds;
            this.Position = position;
            this.ContentManager = contentManager;
        }

        #region "iDrawable"

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public int DrawOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public bool Visible
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> VisibleChanged;

        #endregion
    }
}
