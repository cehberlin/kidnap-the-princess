using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MagicWorld.Controls
{
    public static class GameOptionsControls
    {
        public const Keys FullscreenToggleKey = Keys.F11;
        public const Keys ExitGameKey = Keys.Escape;
        public const Keys ToggleSound = Keys.F12;


        #region "Debug"#

        public const Keys DEBUG_NO_MANA_COST = Keys.F2;
        public const Keys DEBUG_SHOW_BOUNDINGS = Keys.F3;
        public const Keys DEBUG_TOGGLE_GRAVITY_INFLUECE_ON_PLAYER = Keys.F8; //not resolved at the moment

        
        public const Keys DEBUG_PREV_LEVEL = Keys.F4;
        public const Keys DEBUG_NEXT_LEVEL = Keys.F5;
        public const Keys DEBUG_RELOAD_LEVEL = Keys.F7;

        public const Keys DEBUG_CONSTANT_NEXTGROUP = Keys.LeftShift;
        public const Keys DEBUG_CONSTANT_NEXTGROUPITEM = Keys.Tab;
        public const Keys DEBUG_CONSTANT_ITEM_INCREASE = Keys.D2;
        public const Keys DEBUG_CONSTANT_ITEM_DECREASE = Keys.D1;
        public const Keys DEBUG_CONSTANT_ITEM_SWITCH_INTERNAL = Keys.LeftAlt;

        #endregion
    }
}
