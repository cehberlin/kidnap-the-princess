#region File Description
//-----------------------------------------------------------------------------
// SmokePlumeParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld;
#endregion

namespace ParticleEffects
{
    /// <summary>
    /// SmokePlumeParticleSystem is a specialization of ParticleSystem which sends up a
    /// plume of smoke. The smoke is blown to the right by the wind.
    /// </summary>
    class FireParticleSystem : ParticleSystem
    {
        public FireParticleSystem(MagicWorldGame game, int howManyEffects)
            : base(game,howManyEffects)
        {
        }

        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "fire";

            minInitialSpeed = 20;
            maxInitialSpeed = 50;

            // we don't want the particles to accelerate at all, aside from what we
            // do in our overriden InitializeParticle.
            minAcceleration = 0;
            maxAcceleration = 0;

            // long lifetime, this can be changed to create thinner or thicker smoke.
            // tweak minNumParticles and maxNumParticles to complement the effect.
            minLifetime = 1f;
            maxLifetime = 2.0f;

            minScale = .05f;
            maxScale = 0.2f;

            // we need to reduce the number of particles on Windows Phone in order to keep
            // a good framerate

            minNumParticles = 5;
            maxNumParticles = 10;

            // rotate slowly, we want a fairly relaxed effect
            minRotationSpeed = 0;
            maxRotationSpeed = 0;

			blendState = BlendState.AlphaBlend;

            //DrawOrder = AlphaBlendDrawOrder;
        }

        /// <summary>
        /// PickRandomDirection is overriden so that we can make the particles always 
        /// move have an initial velocity pointing up.
        /// </summary>
        /// <returns>a random direction which points basically up.</returns>
        protected override Vector2 PickRandomDirection(Vector2 pos_center, Vector2 startPosition)
        {
            // Point the particles somewhere between 80 and 100 degrees.
            // tweak this to make the smoke have more or less spread.
            float radians = RandomBetween(
                MathHelper.ToRadians(85), MathHelper.ToRadians(95));

            Vector2 direction = Vector2.Zero;
            // from the unit circle, cosine is the x coordinate and sine is the
            // y coordinate. We're negating y because on the screen increasing y moves
            // down the monitor.
            direction.X = (float)Math.Cos(radians);
            direction.Y = -(float)Math.Sin(radians);
            return direction;
        }

        /// <summary>
        /// InitializeParticle is overridden to add the appearance of wind.
        /// </summary>
        /// <param name="p">the particle to set up</param>
        /// <param name="where">where the particle should be placed</param>
        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);

            // the base is mostly good, but we want to simulate a little bit of wind
            // heading to the right.
            p.Acceleration.X += RandomBetween(10, 50);
        }

       protected override float getOpacityFactor(Particle p){
           float normalizedLifetime = getNormalizedLifetime(p);
           return 2*normalizedLifetime * (1 - normalizedLifetime);
       }
    }
}
