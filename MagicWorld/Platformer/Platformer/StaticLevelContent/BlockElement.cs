#region File Description
//-----------------------------------------------------------------------------
// Tile.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;

namespace MagicWorld
{
    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    enum BlockCollision
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

    /// <summary>
    /// Stores the appearance and collision behavior of a tile.
    /// </summary>
    class BlockElement:BasicGameElement
    {
        public Texture2D Texture;
        public BlockCollision Collision;

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
        public BlockElement(String texture, BlockCollision collision,Level level,Vector2 position):base(level)
        {
            this.level = level;            
            Collision = collision;
            this.position = position;
            if (texture != null)
            {
                LoadContent(texture);
            }
        }

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public BlockElement(String texture, BlockCollision collision, Level level, Vector2 position,int width,int height)
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
