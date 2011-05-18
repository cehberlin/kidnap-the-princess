using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Platformer.DynamicLevelContent;
using System.Diagnostics;

namespace Platformer
{
    class MatterSpell:Spell 
    {
        private const int manaBasicCost = 200;
        private const float manaCastingCost = 1f;

        private const int MATTER_EXISTENCE_TIME = 1200; // time that created Matter exist

        /// <summary>
        /// Created Tile lifetime depends on Force (spell creation time) also the life time of the spell itself(so it flies a shorter time)
        /// </summary>
        /// <param name="spriteSet"></param>
        /// <param name="_origin"></param>
        /// <param name="level"></param>
        public MatterSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, manaBasicCost, manaCastingCost)
        {            
            Force = 1;
            survivalTimeMs = 10;
            MoveSpeed = 40.0f;
            LoadContent(spriteSet);
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = MATTER_EXISTENCE_TIME;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/" + spriteSet + "/";
            runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.15f, true,3);

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void OnWorkingStart()
        {
            survivalTimeMs *= Force;
            Debug.WriteLine("Matter starts working TIme:" +survivalTimeMs);
            base.OnWorkingStart();
        }

        protected override void OnRemove()
        {
            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + BoundingRectangle.Width / 2 * (int)direction.X;
            int x = (int)Math.Floor(posX / Tile.Width) - (int)direction.X;
            int y = (int)Math.Floor(Position.Y / Tile.Height);

            if (x > 0 && x < level.Width && y > 0 && y < level.Height)
            {
                if (level.GetTile(x, y).Texture == null && level.GetTile(x+1, y).Texture == null&& level.GetTile(x-1, y).Texture == null)//empty tile
                {
                    double matterTileLifeTime = durationOfActionMs * Force;
                    Debug.WriteLine("Matter Tile LifeTime " + matterTileLifeTime);
                    level.Tiles[x, y] = new MatterTile("Tiles/BlockA1", level, x, y,matterTileLifeTime );
                }
            }

            base.OnRemove();
        }
    }
}
