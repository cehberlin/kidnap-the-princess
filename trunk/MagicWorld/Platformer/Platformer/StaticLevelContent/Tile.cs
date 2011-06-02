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
using Platformer.DynamicLevelContent;
using Platformer.HelperClasses;

namespace Platformer
{
    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    enum TileCollision
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
    class Tile:BasicGameElement
    {
        public Texture2D Texture;
        public TileCollision Collision;

        public const int Width = 40;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        int x;

        public int X
        {
            get { return x; }
            set { 
                x = value;
                position = new Vector2(x, y) * Tile.Size; //update position if index changed
            }
        }
        int y;

        public int Y
        {
            get { return y; }
            set {                
                y = value;
                position = new Vector2(x, y)*Tile.Size; //update position if index changed
            }
        }


        public override Vector2 Position
        {
            get { return position; }
            set { position = value * Tile.Size; }
        }

        /// <summary>
        /// some variables for spell reaction
        /// </summary>
        enum SpellState { NORMAL, BURNED, FROZEN , DESTROYED};
        SpellState spellState = SpellState.NORMAL;
        double spellDurationOfActionMs = 0;

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public Tile(String texture, TileCollision collision,Level level,Vector2 position):base(level)
        {
            this.level = level;            
            Collision = collision;
            this.position = position * Tile.Size;
            if (texture != null)
            {
                LoadContent(texture);
            }
        }

        /// <summary>
        /// overloaded constructor
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="collision"></param>
        /// <param name="level"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Tile(String texture, TileCollision collision, Level level, int x, int y):
            this(texture,collision,level,new Vector2(x,y))
        {
            this.x = x;
            this.y = y;
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
                    if (Texture.Name.Equals("Tiles/Ice_Tile"))
                    {
                        spellState = SpellState.DESTROYED;
                        spellDurationOfActionMs = spell.DurationOfActionMs;
                    }
                    else
                    {
                        spellState = SpellState.BURNED;
                        spellDurationOfActionMs = spell.DurationOfActionMs;
                    }
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
                    spriteBatch.Draw(Texture, position, Color.Red);
                }
                else if (spellState == SpellState.FROZEN)
                {
                    spriteBatch.Draw(Texture, position, Color.Blue);
                }
                else if (spellState == SpellState.DESTROYED)
                {
                    level.ClearTile(this.X, this.Y);
                }
                else
                {
                    spriteBatch.Draw(Texture, position, Color.White);
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
