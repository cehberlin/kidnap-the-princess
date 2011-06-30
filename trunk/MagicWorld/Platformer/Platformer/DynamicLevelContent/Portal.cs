using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses;

namespace MagicWorld.DynamicLevelContent
{
    /// <summary>
    /// one portal only works for a specific object type,
    /// you could specify it here
    /// </summary>
    enum PortalHandlingType
    {
        PLAYER,
        PUSHPULL,
        SHADOW_CREATURE
    }
    class Portal:BlockElement
    {
        PortalHandlingType handleType;

        Vector2 portalDestination;

        CollisionManager.OnCollisionWithCallback collisionCallback;

        public Portal(String texture, Level level, Vector2 position, Vector2 portalDestination,PortalHandlingType handleType=PortalHandlingType.PLAYER)
            : base(texture, CollisionType.Passable, level, position)
        {
            this.portalDestination = portalDestination;
            this.handleType = handleType;
            collisionCallback = HandleCollisionWithSingleObject;
        }


        public override void Update(GameTime gameTime)
        {
            level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
            base.Update(gameTime);
        }

        protected void HandleCollisionWithSingleObject(BasicGameElement element, bool xAxisCollision, bool yAxisCollision)
        {
            switch (handleType)
            {
                case PortalHandlingType.PLAYER:
                    if (element.GetType() == typeof(MagicWorld.Player))
                    {
                        element.Position = portalDestination;
                    }
                    break;
                case PortalHandlingType.PUSHPULL:
                    if (element.GetType() == typeof(PushPullElement))
                    {
                        element.Position = portalDestination;
                    }
                    break;
                case PortalHandlingType.SHADOW_CREATURE:
                    if (element.GetType() == typeof(ShadowCreature))
                    {
                        element.Position = portalDestination;
                    }
                    break;
            }
           
        }
    }
}
