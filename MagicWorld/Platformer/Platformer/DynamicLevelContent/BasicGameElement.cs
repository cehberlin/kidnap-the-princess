using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Constants;
using MagicWorld.Audio;

namespace MagicWorld.DynamicLevelContent
{
    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    public enum CollisionType
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2
    }

    public class BasicGameElement : IAutonomusGameObject,ISpellInfluenceable
    {
        /// <summary>
        /// how should collision be handled
        /// </summary>
        public CollisionType Collision=CollisionType.Passable;

        protected Vector2 position = Vector2.Zero;

        private Vector2 oldPosition = Vector2.Zero;

        protected bool positionChanged = true; // true if the position was changed but no new bounding box was calculated

        public Vector2 OldPosition
        {
            get { return oldPosition; }
            set { oldPosition = value; }
        }

        public virtual Vector2 Position
        {
            get { return position; }
            set {
                oldPosition = position;
                position = new Vector2((float)Math.Round(value.X), (float)Math.Round(value.Y));
                positionChanged = true;

            }
        }

        protected Vector2 velocity = Vector2.Zero;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        //you need to set a Bound
        protected Bounds bounds=new Bounds();
        

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

        protected Texture2D debugTextureCycle;

        protected Color debugColor = Color.White;

        /// <summary>
        /// enables and disables debug only for this objects
        /// </summary>
        public Boolean PrivateDebug = true;

        #endregion

        protected Level level;

        public BasicGameElement(Level level)
        {
            this.level = level;
        }

        protected bool isRemovable = false;

        public bool IsRemovable
        {
            get { return isRemovable; }
            set { isRemovable = value; }
        }

        public IAudioService audioService;

        #region IAutonomusGameObject Member

        public virtual void LoadContent(string spriteSet)
        {
            debugTexture = level.Content.Load<Texture2D>("Sprites\\white");
            debugTextureCycle = level.Content.Load<Texture2D>("Sprites\\Circle");
            audioService = (IAudioService)level.Game.Services.GetService(typeof(IAudioService));
        }

        public virtual void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (DebugValues.DEBUG && PrivateDebug)
            {
                if (Bounds.Type == Bounds.BoundType.BOX)
                {
                    spriteBatch.Draw(debugTexture, Bounds.getRectangle(), debugColor);
                }
                else
                {
                    spriteBatch.Draw(debugTextureCycle, Bounds.getRectangle(), debugColor);
                }
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
