using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;

namespace MagicWorld
{
    /// <summary>
    /// Stores the appearance and collision behavior of a tile.
    /// </summary>
    class BlockElement:BasicGameElement
    {
        public Texture2D Texture;

        public int Width = 40;
        public int Height = 32;

        public override Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// some variables for spell reaction
        /// </summary>
        public enum SpellState { NORMAL, BURNED, FROZEN , DESTROYED};
        protected SpellState spellState = SpellState.NORMAL;

        public SpellState State
        {
            get { return spellState; }
        }
        protected double spellDurationOfActionMs = 0;

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public BlockElement(String texture, CollisionType collision,Level level,Vector2 position):base(level)
        {
            this.level = level;            
            Collision = collision;
            this.position = position;
            if (texture != null)
            {
                LoadContent(texture);
            }
            Width= Texture.Bounds.Width;
            Height = Texture.Bounds.Height;
        }

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public BlockElement(String texture, CollisionType collision, Level level, Vector2 position,int width,int height)
            : this(texture,collision,level,position)
        {
            this.Width = width;
            this.Height = height;
        }

        public override Bounds Bounds
        {
            get
            {
                int left = (int)Math.Round(position.X);
                int top = (int)Math.Round(position.Y);

                return new Bounds(left, top, Width,Height);
            }
        }

        public override Boolean SpellInfluenceAction(Spell spell)
        {
            if (Texture != null)
            {
                if (spell.GetType() == typeof(WarmSpell))
                {                       
                    spellState = SpellState.BURNED;
                    spellDurationOfActionMs = spell.DurationOfActionMs;
                    
                    return true;
                }
                if (spell.GetType() == typeof(ColdSpell))
                {
                    spellState = SpellState.FROZEN;
                    spellDurationOfActionMs = spell.DurationOfActionMs;
                    return true;
                }
            }
            return false;
        }

        #region IAutonomusGameObject Member

        public override void LoadContent(string spriteSet)
        {
            Texture = level.Content.Load<Texture2D>(spriteSet);
            Texture.Name = spriteSet;
            base.LoadContent("");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                // Draw it in screen space.                
                if (spellState == SpellState.BURNED) {
                    spriteBatch.Draw(Texture, Bounds.getRectangle(),Color.Red);
                }
                else if (spellState == SpellState.FROZEN)
                {
                    spriteBatch.Draw(Texture, Bounds.getRectangle(), Color.Blue);
                }
                else
                {
                    spriteBatch.Draw(Texture,Bounds.getRectangle(), Color.White);
                }
                base.Draw(gameTime, spriteBatch);
            }            
        }

        public override void Update(GameTime gameTime)
        {
            if (spellState != SpellState.NORMAL)
            {
                if (spellDurationOfActionMs>0)
                {
                    spellDurationOfActionMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    spellDurationOfActionMs = 0;
                    spellState = SpellState.NORMAL;
                }                
            }
        }
        #endregion

    }
}
