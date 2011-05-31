using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Controls
{
    /// <summary>
    /// singelton that provides the Game Control
    /// </summary>
    public class PlayerControlFactory
    {
        private IPlayerControl control;

        private PlayerControlFactory()
        {
            control = new DefaultControl();
        }

        public IPlayerControl getPlayerControl()
        {
            return control;
        }

        #region "singelton"

        private static PlayerControlFactory INSTANCE;
        public static PlayerControlFactory GET_INSTANCE()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new PlayerControlFactory();
            }
            return INSTANCE;
        }
        #endregion

    }
}
