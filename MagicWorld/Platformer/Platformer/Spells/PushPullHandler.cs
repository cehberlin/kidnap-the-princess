using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;
using MagicWorld.Constants;
using System.Diagnostics;

namespace MagicWorld.Spells
{
    /// <summary>
    /// this class handles a push or pull impulse in a arbitary direction for a given time
    /// also available is the use of accelaration
    /// </summary>
    class PushPullHandler
    {

        protected double currentAccelarationX = 1;
        protected float accelarationChangeFactorX = 0; //will be multiplyed be elaspes seconds
        protected float accelarationMaxX = 2.0f;
        protected float accelarationMinX = 0f;

        protected double currentAccelarationY = 1;
        protected float accelarationChangeFactorY = 0; //will be multiplyed be elaspes seconds
        protected float accelarationMaxY = 2.0f;
        protected float accelarationMinY = 0f;

        double influenceTimeInMs;

        protected BasicGameElement element;

        public Vector2 influenceVelocity;

        public PushPullHandler(BasicGameElement elem, float influenceTimeInMs, Vector2 startVelocity)
        {
            start(elem, influenceTimeInMs, startVelocity);
        }

        public PushPullHandler()
        {
            start(null, 0, Vector2.Zero);
        }

        public void start(BasicGameElement elem, double influenceTimeInMs, Vector2 startVelocity)
        {
            this.influenceTimeInMs = influenceTimeInMs;
            this.element = elem;
            influenceVelocity = startVelocity*320;
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
                    double elapsed = gameTime.ElapsedGameTime.TotalSeconds;

                    //only change acceleration if we are over a some elapsed time
                    //if we do not consider this special case the accelartion change factor gets to much influence
                    // in casting time
                    if (elapsed > 0.01) //no movement
                    {
                        //accelaration
                        currentAccelarationX += accelarationChangeFactorX * elapsed;
                        if (currentAccelarationX < accelarationMinX)
                            currentAccelarationX = accelarationMinX;
                        if (currentAccelarationX > accelarationMaxX)
                            currentAccelarationX = accelarationMaxX;

                        currentAccelarationY += accelarationChangeFactorY * elapsed;
                        if (currentAccelarationY < accelarationMinY)
                            currentAccelarationY = accelarationMinY;
                        if (currentAccelarationY > accelarationMaxY)
                            currentAccelarationY = accelarationMaxY;
                    
                    
                        influenceVelocity = new Vector2((float)(influenceVelocity.X * currentAccelarationX), (float)(influenceVelocity.Y * currentAccelarationY));

                        element.Position += (float)elapsed * influenceVelocity;

                    }

                    influenceTimeInMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    //remove current handled element
                    element = null;
                }
            }
        }

        public void Update(GameTime gameTime, Vector2 currentPath, Vector2 nextPath, Bounds bounds)
        {
            if (element != null)
            {
                if (influenceTimeInMs > 0)
                {
                    double elapsed = gameTime.ElapsedGameTime.TotalSeconds;

                    //only change acceleration if we are over a some elapsed time
                    //if we do not consider this special case the accelartion change factor gets to much influence
                    // in casting time
                    if (elapsed > 0.01)
                    {
                        //accelaration
                        currentAccelarationX += accelarationChangeFactorX * elapsed;
                        if (currentAccelarationX < accelarationMinX)
                            currentAccelarationX = accelarationMinX;
                        if (currentAccelarationX > accelarationMaxX)
                            currentAccelarationX = accelarationMaxX;

                        currentAccelarationY += accelarationChangeFactorY * elapsed;
                        if (currentAccelarationY < accelarationMinY)
                            currentAccelarationY = accelarationMinY;
                        if (currentAccelarationY > accelarationMaxY)
                            currentAccelarationY = accelarationMaxY;
                    }
                    
                    influenceVelocity = new Vector2((float)(influenceVelocity.X * currentAccelarationX), (float)(influenceVelocity.Y * currentAccelarationY));

                    //pushing X Axis
                    if (influenceVelocity.X > 0)
                    {
                        if (currentPath.X < nextPath.X)
                        {
                            if (element.Position.X < nextPath.X - bounds.Width / 2)
                            {
                                element.Position += influenceVelocity *(float) elapsed;
                            }
                        }
                        else
                        {
                            if (element.Position.X < currentPath.X - bounds.Width / 2)
                            {
                                element.Position += influenceVelocity * (float)elapsed;
                            }
                        }
                    }
                    //pulling X Axis
                    else if (influenceVelocity.X < 0)
                    {
                        if (currentPath.X < nextPath.X)
                        {
                            if (element.Position.X > currentPath.X - bounds.Width / 2)
                            {
                                element.Position += influenceVelocity * (float)elapsed;
                            }
                        }
                        else
                        {
                            if (element.Position.X > nextPath.X - bounds.Width / 2)
                            {
                                element.Position += influenceVelocity * (float)elapsed;
                            }
                        }
                    }
                    //pull Y Axis
                    else if (influenceVelocity.X == 0 && influenceVelocity.Y > 0)
                    {
                        if (currentPath.Y > nextPath.Y)
                        {
                            if (element.Position.Y < currentPath.Y - bounds.Height / 2)
                            {
                                element.Position += influenceVelocity * (float)elapsed; ;
                            }
                        }
                        else
                        {
                            if (element.Position.Y <= nextPath.Y - bounds.Height / 2)
                            {
                                element.Position += influenceVelocity * (float)elapsed; ;
                            }
                        }
                    }//push y Axis
                    else if (influenceVelocity.X == 0 && influenceVelocity.Y < 0)
                    {
                        if (currentPath.Y > nextPath.Y)
                        {
                            if (element.Position.Y > nextPath.Y - bounds.Height / 2)
                            {
                                element.Position += influenceVelocity * (float)elapsed; ;
                            }
                        }
                        else
                        {
                            if (element.Position.Y > currentPath.Y - bounds.Height / 2)
                            {
                                element.Position += influenceVelocity * (float)elapsed; ;
                            }
                        }
                    }

                    influenceTimeInMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
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
