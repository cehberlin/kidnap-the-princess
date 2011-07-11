#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld;
using MagicWorld.HelperClasses;
#endregion

namespace ParticleEffects
{
    /// <summary>
    /// ExplosionParticleSystem is a specialization of ParticleSystem which creates a
    /// fiery explosion. It should be combined with ExplosionSmokeParticleSystem for
    /// best effect.
    /// </summary>
    class LightningParticleSystem : ParticleSystem
    {
        public LightningParticleSystem(MagicWorldGame game, int howManyEffects)
            : base(game, howManyEffects)
        {
        }

        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "lightning";

            // high initial speed with lots of variance.  make the values closer
            // together to have more consistently circular explosions.
            minInitialSpeed = 0;
            maxInitialSpeed = 200;

            // doesn't matter what these values are set to, acceleration is tweaked in
            // the override of InitializeParticle.
            minAcceleration = 0;
            maxAcceleration = 0;

            // explosions should be relatively short lived
            minLifetime = .5f;
            maxLifetime = 1f;

            minScale = .005f;
            maxScale = .10f;

            baseOpacity = 1f;

            minNumParticles = 2;
            maxNumParticles = 5;

            minRotationSpeed =0;
            maxRotationSpeed =0;

            // additive blending is very good at creating fiery effects.
			blendState = BlendState.Additive;

        }

        protected override void InitializeParticle(Particle p, ParticleSetting particleSetting)
        {
            base.InitializeParticle(p, particleSetting);
            
            // The base works fine except for acceleration. Explosions move outwards,
            // then slow down and stop because of air resistance. Let's change
            // acceleration so that when the particle is at max lifetime, the velocity
            // will be zero.

            // We'll use the equation vt = v0 + (a0 * t). (If you're not familar with
            // this, it's one of the basic kinematics equations for constant
            // acceleration, and basically says:
            // velocity at time t = initial velocity + acceleration * t)
            // We'll solve the equation for a0, using t = p.Lifetime and vt = 0.
            p.Acceleration = -p.Velocity / p.Lifetime;
        }


        protected override Vector2 PicDirection(ParticleSetting particleSetting, Vector2 startPosition)
        {
            float angle = RandomBetween(0, MathHelper.PiOver4/2);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        protected override Vector2 getStartPositionRelativeToCenter(ParticleSetting particleSetting)
        {
            return GeometryCalculationHelper.getRandomPositionOnCycleBow(particleSetting.position, random.Next(0,(int)particleSetting.distance));
        }

    }
}
