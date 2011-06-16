using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagicWorld.Services
{
    interface IPlayerService : IServiceProvider
    {
        Vector2 Position { get; }
        bool IsCasting { get; }
        bool IsAlive { get; }

        Spells.SpellType selectedSpell_A { get; }
        Spells.SpellType selectedSpell_B { get; }
        double SpellAimAngle { get; }
    }
}
