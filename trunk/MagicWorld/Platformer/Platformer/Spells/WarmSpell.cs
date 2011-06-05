using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.Spells;

namespace MagicWorld
{
    class WarmSpell:Spell 
    {
        private const int manaBasicCost = 150;
        private const float manaCastingCost = 1f;
        public WarmSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, manaBasicCost, manaCastingCost, SpellType.WarmingSpell)
        {            
            Force = 1;
            survivalTimeMs = 3000;
            MoveSpeed = 64;
            LoadContent(spriteSet);
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = 5000;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/WarmSpell/";
            runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.5f, true,3);

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void OnRemove()
        {
            level.ExplosionParticleSystem.AddParticles(position);
            level.ExplosionSmokeParticleSystem.AddParticles(position);
            base.OnRemove();
        }
    }
}
