using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace KidnapThePrincess
{
    class GameObject
    {
        protected Texture2D spriteFullHitPoints;
        protected Texture2D spriteMediumHitPoints;
        protected Texture2D spriteLowHitPoints;
        protected SpriteFont defaultTextFont;

        private Vector2 pos;

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        private Rectangle area;

        public Rectangle Area
        {
            get { return area; }
            set { area = value; }
        }

        public Rectangle CollisionArea
        {
            get { return new Rectangle(
                (int)(area.X + area.Width * 0.125),
                (int)(area.Y + area.Height * 0.25),
                (int)(area.Width * 0.75),
                (int)(area.Height * 0.75));
            }
            set { area = value; }
        }

        private int hitPoints=Int32.MinValue;
        public int Hitpoints
        {
            get { return hitPoints; }
            set
            {
                hitPoints = value;
                if (hitPoints == Int32.MinValue)
                {
                    hitpoitsStore = hitPoints;
                }
            }
        }

        
        private int hitpoitsStore;
        /// <summary>
        /// store highest hitpoints
        /// </summary>
        public int HitpoitsMax
        {
            get { return hitpoitsStore; }
        }

        public readonly int START_HIT_POINTS; 
        private readonly int HALF_HIT_POINTS;



        public GameObject(int startHitPoints, SpriteFont defaultTextFont, Texture2D textureFullHitPoints, Texture2D textureMediumHitPoints, Texture2D textureLowHitPoints)
        {
            this.spriteFullHitPoints = textureFullHitPoints;
            this.spriteMediumHitPoints = textureMediumHitPoints;
            this.spriteLowHitPoints = textureLowHitPoints;
            Hitpoints = startHitPoints;
            this.START_HIT_POINTS = startHitPoints;
            this.HALF_HIT_POINTS = (int)((2.0 / 3.0) * HitpoitsMax);
            this.defaultTextFont = defaultTextFont;
        }
        public virtual void Draw(SpriteBatch sb) 
        {
            if (hitPoints == START_HIT_POINTS)
            {
                Area = new Rectangle((int)Position.X, (int)Position.Y, spriteFullHitPoints.Width, spriteFullHitPoints.Height);
                sb.Draw(spriteFullHitPoints, Area, Color.White);
            }
            else if (hitPoints > this.HALF_HIT_POINTS)
            {
                Area = new Rectangle((int)Position.X, (int)Position.Y, spriteMediumHitPoints.Width, spriteMediumHitPoints.Height);
                sb.Draw(spriteMediumHitPoints, Area, Color.White);               
            }
            else
            {
                Area = new Rectangle((int)Position.X, (int)Position.Y, spriteLowHitPoints.Width, spriteLowHitPoints.Height);
                sb.Draw(spriteLowHitPoints, Area, Color.White);
            }
            if (GameState.DEBUG)
            {
                sb.DrawString(defaultTextFont, hitPoints + "HP", new Vector2(area.Center.X, area.Center.Y), Color.Red);
            }
        }

        public virtual void Update(GameTime time) { }
    }
}
