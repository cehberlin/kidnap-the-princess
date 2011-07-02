using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;

namespace MagicWorld
{
    /// <summary>
    /// a very genereal interactive level element, has collision bounds ...
    /// </summary>
    class BlockElement:BasicGameElement
    {
        public Texture2D Texture;

        public int Width = 0;
        public int Height = 0;

        private bool positionChanged = true; // true if the position was changed but no new bounding box was calculated

        public override Vector2 Position
        {
            get { return position; }
            set { position = value; positionChanged = true; }
        }

        protected Color drawColor = Color.White;

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
        /// Constructor with color
        /// </summary>
        public BlockElement(String texture, CollisionType collision,Level level,Vector2 position,Color drawColor):base(level)
        {
            this.drawColor = drawColor;
            this.level = level;            
            Collision = collision;
            this.position = position;
            if (texture != null)
            {
                LoadContent(texture);
            }
            Width= Texture.Bounds.Width;
            Height = Texture.Bounds.Height;
            bounds = new HelperClasses.Bounds(this.position, Width, Height);
        }

        /// <summary>
        ///Constructor
        /// </summary>
        public BlockElement(String texture, CollisionType collision, Level level, Vector2 position)
            : this(texture,collision,level,position,Color.White)
        {           
        }

        /// <summary>
        /// Constructor with width and height
        /// </summary>
        public BlockElement(String texture, CollisionType collision, Level level, Vector2 position,int width,int height)
            : this(texture,collision,level,position,Color.White)
        {
            this.Width = width;
            this.Height = height;
            bounds = new HelperClasses.Bounds(this.position, Width, Height);
        }

        public override Bounds Bounds
        {
            get
            {
                if (positionChanged)
                {
                    int left = (int)Math.Round(position.X);
                    int top = (int)Math.Round(position.Y);
                    bounds = new Bounds(left, top, Width, Height);
                }
                return bounds;
            }
        }

        public override Boolean SpellInfluenceAction(Spell spell)
        {
            if (Texture != null)
            {
                if (spell.GetType() == typeof(WarmSpell)||spell.GetType() == typeof(ColdSpell))
                {                      
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
                spriteBatch.Draw(Texture,Bounds.getRectangle(), drawColor);
               
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
