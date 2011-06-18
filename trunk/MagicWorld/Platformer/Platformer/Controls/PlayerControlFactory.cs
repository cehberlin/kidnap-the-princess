using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Controls
{
    public enum ControlType
    {
        defaultControl,
        laptopControl
    }

    /// <summary>
    /// singelton that provides the Game Control
    /// </summary>
    public class PlayerControlFactory
    {

        private IPlayerControl control;

        private PlayerControlFactory()
        {
            control = new LaptopControl();
        }

        public IPlayerControl getPlayerControl()
        {
            return control;
        }

        public void ChangeControl(ControlType type)
        {
            switch (type)
            {
                case ControlType.laptopControl:
                    this.control = new LaptopControl(); break;
                default:
                    this.control = new DefaultControl(); break;
            }
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
