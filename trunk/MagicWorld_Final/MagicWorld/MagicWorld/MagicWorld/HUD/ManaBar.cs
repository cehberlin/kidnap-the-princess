using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.HUD
{
    /// <summary>
    /// shows the amount of mana the player has
    /// </summary>
    public class ManaBar : DrawableGameComponent
    {
        public ManaBar(Game game) :
            base(game)
        {
        }
    }
}
