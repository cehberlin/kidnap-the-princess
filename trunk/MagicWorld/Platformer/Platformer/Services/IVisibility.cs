using System;
using Microsoft.Xna.Framework;
using MagicWorld.BlendInClasses;

namespace MagicWorld.Services
{
    public interface IVisibility:IServiceProvider
    {
        void Add(IIcedVisibility pos);
        void Remove(IIcedVisibility pos);
        void Clear();
    }
}
