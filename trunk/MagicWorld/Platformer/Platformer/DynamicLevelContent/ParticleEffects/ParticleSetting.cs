using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ParticleEffects
{
    public class ParticleSetting
    {
        private static Random rnd = new Random(1415648);

        public Color color;

        public Vector2 position;

        /// <summary>
        /// could be used for distance or length information
        /// </summary>
        public float distance;

        public ParticleSetting(Vector2 position, Color color, float distance = 0f)
        {
            this.position = position;
            this.color = color;
            this.distance = distance;
        }

        public ParticleSetting(Vector2 position, float distance = 0f)
            : this(position, Color.White, distance)
        {
        }

        public static ParticleSetting getWithRandomColor(Vector2 position, float distance = 0f)
        {
            return new ParticleSetting(position, getRandomParticleTextureColor(), distance);
        }

        protected static Color getRandomParticleTextureColor()
        {

            Color color;

            switch (rnd.Next(0, 10))
            {
                case 0:
                    color = Color.White;
                    break;
                case 1:
                    color = Color.Red;
                    break;
                case 2:
                    color = Color.Blue;
                    break;
                case 3:
                    color = Color.Yellow;
                    break;
                case 4:
                    color = Color.Green;
                    break;
                case 5:
                    color = Color.Pink;
                    break;
                case 6:
                    color = Color.Violet;
                    break;
                case 7:
                    color = Color.LightCyan;
                    break;
                case 8:
                    color = Color.LightBlue;
                    break;
                default:
                    color = Color.White;
                    break;
            }


            return color;
        }

    }
}
