using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// interface which coul be used by doors,machines and something like this 
    /// Example: Open and close...
    /// </summary>
    interface IActivation
    {
        void activate();
        void deactivate();
    }
}
