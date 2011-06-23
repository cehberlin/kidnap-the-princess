using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    abstract class AbstractSwitch : BlockElement
    {

        /// <summary>
        /// array with all switchable Objects that are activated if switch is this activated
        /// </summary>
        public LinkedList<IActivation> SwitchableObjects { get; set; }


        /// <summary>
        /// true if switch is activated
        /// </summary>
        public bool Activated{get; protected set;}

        /// <summary>
        /// identification of button (for level editor)
        /// </summary>
        public string ID { get; set; }

        public AbstractSwitch(String texture, CollisionType collision, Level level, Vector2 position)
            : base(texture, collision, level, position) 
        {
            SwitchableObjects = new LinkedList<IActivation>();
        }

        /// <summary>
        /// activated all switchable Objects
        /// </summary>
        public virtual void Activate()
        {
            Activated = true;
            foreach(IActivation switchable in SwitchableObjects)
            {
                switchable.activate();
            }
        }

        /// <summary>
        /// Dectivate all switchable Objects
        /// </summary>
        public virtual void Deactivate()
        {
            Activated = false;
            foreach (IActivation switchable in SwitchableObjects)
            {
                switchable.deactivate();
            }
        }
    }
}
