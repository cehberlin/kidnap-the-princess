using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.Services
{
    interface ISimpleAnimator:IServiceProvider
    {
        void AddItem(int type, Vector2 pos);
        void InitCamera();
        void Clear();
    }
}
