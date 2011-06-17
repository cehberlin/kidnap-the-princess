using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.DynamicLevelContent;

namespace MagicWorld.Spells
{
    /// <summary>
    /// this class handles a push or pull impulse in a arbitary direction for a given time
    /// also available is the use of accelaration
    /// </summary>
    class PushPullHandler
    {

        protected float currentAccelarationX = 1;
        protected float accelarationChangeFactorX = 0; //will be multiplyed be elaspes seconds
        protected float accelarationMaxX = 2.0f;
        protected float accelarationMinX = 0f;

        protected float currentAccelarationY = 1;
        protected float accelarationChangeFactorY = 0; //will be multiplyed be elaspes seconds
        protected float accelarationMaxY = 2.0f;
        protected float accelarationMinY = 0f;

        float influenceTimeInMs;

        protected BasicGameElement element;

        protected Vector2 influenceVelocity;

        public PushPullHandler(BasicGameElement elem,float influenceTimeInMs,Vector2 startVelocity)
        {
           start(elem,influenceTimeInMs,startVelocity);
        }

        public PushPullHandler()
        {
            start(null, 0, Vector2.Zero);
        }

        public void start(BasicGameElement elem, float influenceTimeInMs, Vector2 startVelocity)
        {
            this.influenceTimeInMs = influenceTimeInMs;
            this.element = elem;
            influenceVelocity = startVelocity;
        }

        public void setYAcceleration(float current, float min, float max, float changeFactor)
        {
            currentAccelarationY = current;
            accelarationChangeFactorY = changeFactor;
            accelarationMaxY = max;
            accelarationMinY = min;
        }

        public void setXAcceleration(float current, float min, float max, float changeFactor)
        {
            currentAccelarationX = current;
            accelarationChangeFactorX = changeFactor;
            accelarationMaxX = max;
            accelarationMinX = min;
        }

        public void Update(GameTime gameTime)
        {
            if (element != null)
            {
                if (influenceTimeInMs > 0)
                {
                    float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    //accelaration
                    currentAccelarationX += (float)elapsed/1000 * accelarationChangeFactorX;
                    if (currentAccelarationX < accelarationMinX)
                        currentAccelarationX = accelarationMinX;
                    if (currentAccelarationX > accelarationMaxX)
                        currentAccelarationX = accelarationMaxX;

                    currentAccelarationY += (float)elapsed/1000 * accelarationChangeFactorY;
                    if (currentAccelarationY < accelarationMinY)
                        currentAccelarationY = accelarationMinY;
                    if (currentAccelarationY > accelarationMaxY)
                        currentAccelarationY = accelarationMaxY;

                    // Move in the current direction.
                    influenceVelocity = new Vector2((float)influenceVelocity.X * currentAccelarationX, (float)influenceVelocity.Y * currentAccelarationY);

                    element.Position += influenceVelocity;

   
                    influenceTimeInMs -= elapsed;
                }
                else
                {
                    //remove current handled element
                    element = null;
                }
            }
        }
    }
}
