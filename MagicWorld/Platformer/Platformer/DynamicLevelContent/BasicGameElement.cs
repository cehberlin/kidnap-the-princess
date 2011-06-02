using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Platformer.HelperClasses;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.DynamicLevelContent
{
    class BasicGameElement : IAutonomusGameObject,ISpellInfluenceable
    {

        protected Vector2 position;

        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //you need to set a Bound
        protected Bounds bounds=null;

        /// <summary>
        /// override if you want some special behavior
        /// </summary>
        public virtual Bounds Bounds
        {
            get
            {
                if (bounds == null)
                {
                    throw new NullReferenceException();
                }
                bounds.Position = position; //update Position before return
                return bounds;
            }
        }

        #region DEBUG

        protected Texture2D debugTexture;

        protected Color debugColor = Color.White;

        #endregion

        protected Level level;

        public BasicGameElement(Level level)
        {
            this.level = level;
        }


        #region IAutonomusGameObject Member

        public virtual void LoadContent(string spriteSet)
        {
            debugTexture = level.Content.Load<Texture2D>("Sprites\\white");
        }

        public virtual void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (GlobalValues.DEBUG)
            {
                spriteBatch.Draw(debugTexture, Bounds.getRectangle(), debugColor);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        #endregion

        #region ISpellInfluenceable Member

        public virtual bool SpellInfluenceAction(Spell spell)
        {
            return false;
        }

        #endregion
    }
}
