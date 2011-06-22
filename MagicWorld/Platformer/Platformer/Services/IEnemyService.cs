using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses;

namespace MagicWorld.Services
{
    interface IEnemyService : IServiceProvider
    {
        Vector2 Position { get; }
        Bounds Bounds { get; }
        Boolean IsBurning { get; set; }
        Boolean IsFroozen { get; set; }
        Boolean IsElectrified { get; set; }
    }
}
