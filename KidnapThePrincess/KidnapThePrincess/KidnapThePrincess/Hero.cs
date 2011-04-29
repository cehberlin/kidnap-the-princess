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
        private Rectangle area;

        public Rectangle Area
        {
            get { return area; }
            set { area = value; }
        }

        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private bool isActive;

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


        /// <summary>
        /// Constructor for a hero
        /// </summary>
        /// <param name="tex">The texture that will represent the hero in game.</param>
        /// <param name="type">0 = brute, 1 = goblin , 2 = knight, 3 = widow</param>
        /// <param name="area">For the goblin the rectangle equals the carriage, for the others it's the playArea</param>
        public Hero(Texture2D tex, int type, Rectangle area)
            : base(tex)
        {
            switch (type)
            {
                case 0: //goblin
                    Speed = 0.5f;
                    break;
                case 1://brute
                    Speed = 0.8f;
                    break;
                case 2: //Knight
                    Speed = 1.5f;
                    break;
                case 3: //widow
                    Speed = 1f;
                    break;
            }
            this.type = type;
            this.area = area;
            isActive = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        public override void Update()
        {
            
            if (type != 0 && isActive)
            {
                if (area.X > Position.X) Position=new Vector2(area.X,Position.Y);
                if (area.Right-sprite.Width < Position.X) Position = new Vector2(area.Right-sprite.Width, Position.Y);
                if (area.Y > Position.Y) Position = new Vector2(Position.X, area.Y);
                if (area.Bottom < Position.Y) Position = new Vector2(Position.X, area.Bottom);
            }
            else if (type != 0 && !isActive)
            {
                if (dest.X > Position.X) X += 1;
                if (dest.X < Position.X) X -= 1;
                if (dest.Y > Position.Y) Y += 1;
                if (dest.Y < Position.Y) Y -= 1;
            }
            else if (type == 0)
            {
                //are we at the carriage yet?
                if (area.Contains((int)Position.X, (int)Position.Y))
                {
                    isActive = true;
                    return;
                }
            }
            base.Update();
        }
    }
}
