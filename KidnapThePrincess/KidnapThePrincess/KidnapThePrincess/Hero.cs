using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KidnapThePrincess
{
    class Hero : Person
    {
        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        
        private Rectangle area;

        public Rectangle Area
        {
            get { return area; }
            set { area = value; }
        }

        private bool isActive;

        private int strength;

        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }
        

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private Vector2 dest;

        public Vector2 Destination
        {
            get { return dest; }
            set { dest = value; }
        }

        Boolean freezed = false;

        public Boolean Freezed
        {
            get { return freezed; }
            set { freezed = value; }
        }

        private TimeSpan coolDown;

        public TimeSpan CoolDown
        {
            get { return coolDown; }
            set { coolDown = value; }
        }
        

        //flag that says: hero can move during freezing
        protected Boolean canMoveFreezed = false;

        //must be stored to endfreeze at some time
        private TimeSpan freezeDuration;
        private TimeSpan freezeLifespan=TimeSpan.FromSeconds(1);

        //you could specify if a hero only could attack with some delay between
        public TimeSpan attackDelay;

        /// <summary>
        /// Constructor for a hero
        /// </summary>
        /// <param name="tex">The texture that will represent the hero in game.</param>
        /// <param name="area">For the goblin the rectangle equals the carriage, for the others it's the playArea</param>
        public Hero(Texture2D tex, Rectangle area)
            : base(tex)
        {
            this.area = area;
            isActive = false;
            coolDown = TimeSpan.Zero;
        }

        public override void Update(GameTime time)
        {
            if (freezed)
            {
                if (freezeLifespan.Subtract(freezeDuration) > TimeSpan.Zero)
                {
                    freezeDuration = freezeDuration.Add(time.ElapsedGameTime);
                }
                else
                {
                    freezed = false;
                    freezeDuration = TimeSpan.Zero;
                }
            }


            if (!freezed || canMoveFreezed) //if hero was hit by enemy he can move some time
            {
                if (isActive)
                {
                    if (area.X > Position.X) Position = new Vector2(area.X, Position.Y);
                    if (area.Right - sprite.Width < Position.X) Position = new Vector2(area.Right - sprite.Width, Position.Y);
                    if (area.Y > Position.Y) Position = new Vector2(Position.X, area.Y);
                    if (area.Bottom < Position.Y) Position = new Vector2(Position.X, area.Bottom);
                }
                else if (!isActive)
                {
                    Direction = Destination - Position;
                    Direction = Vector2.Normalize(Direction);
                }
            }
            coolDown = coolDown.Subtract(time.ElapsedGameTime);
            base.Update(time);
        }

        public bool CanAttack
        {
            get
            {
                return coolDown<=TimeSpan.Zero?true:false;
            }
        }

        public virtual void attack(Enemy e,Attack attack) { }

        public virtual void attack(GameObject e, Attack attack) { }

        public virtual void moveRight()
        {
            if (!freezed)
            {
                Direction = new Vector2(1, Direction.Y);
            }
        }


        public virtual void moveLeft()
        {
            if (!freezed)
            {
                Direction = new Vector2(-1, Direction.Y);
            }
        }

        public virtual void moveUp()
        {
            if (!freezed)
            {
                Direction = new Vector2(Direction.X, -1);
            }
        }

        public virtual void moveDown()
        {
            if (!freezed)
            {
                Direction = new Vector2(Direction.X, 1);
            }
        }

    }
}
