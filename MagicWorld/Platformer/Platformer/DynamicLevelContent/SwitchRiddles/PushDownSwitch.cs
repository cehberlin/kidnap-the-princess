using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.StaticLevelContent;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// this switch is on as long an enemy,the player or a matter collidates with it
    /// </summary>
    class PushDownSwitch : AbstractSwitch
    {
        
        CollisionManager.OnCollisionWithCallback collisionCallback;

        Vector2 pushDownPosition;
        Vector2 pushUpPosition;

        public PushDownSwitch(String texture, Level level, Vector2 position, string id)
            :base(texture,CollisionType.Passable,level,position, id)
        {

            //calculate bounds
            int left = (int)Math.Round(position.X);
            int top = (int)Math.Round(position.Y);

            //the bounds are only a small rectangle at bottom middle of the texture
            int boundsWith = 30;
            int boundsHeight = 30;
            this.bounds = new Bounds(left + Width / 2 - boundsWith / 2, top + Height - boundsHeight, boundsWith, boundsHeight);

            //set callback for collision
            collisionCallback = HandleCollisionWithSingleObject;
            pushUpPosition = position;
            pushDownPosition = position + new Vector2(0, 15);
        }


        public override void Update(GameTime gameTime)
        {
            CheckForPushDown();
            base.Update(gameTime);
        }

        bool collisionOccured = false;

        protected void HandleCollisionWithSingleObject(BasicGameElement element, bool xAxisCollision, bool yAxisCollision)
        {
            if (element.GetType() == typeof(MatterSpell) || element.GetType() == typeof(ShadowCreature) || element.GetType() == typeof(PushPullElement))
                {
                    if (!Activated)
                    {
                        Activate();  
                    }
                    collisionOccured = true;
                }           
        }

        public override void Activate()
        {
            Position = pushDownPosition;
            base.Activate();
        }

        public override void Deactivate()
        {
            Position = pushUpPosition;
            base.Deactivate();
        }

        /// <summary>
        /// check if switch is pushed down
        /// </summary>
        void CheckForPushDown()
        {
            if (SwitchableObjects != null && SwitchableObjects.Count > 0)
            {
                if (level.CollisionManager.CollidateWithPlayer(this))
                {
                    if (!Activated)
                    {
                        Activate();
                    }
                }
                else
                {
                    collisionOccured = false;
                    level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
                    if (Activated && !collisionOccured)
                    {
                        Deactivate();
                    }
                }
            }
        }

    }
}
