using System;
using Microsoft.Xna.Framework;

namespace MagicWorld.Services
{
    public interface IVisibility:IServiceProvider
    {
        void Add(Rectangle pos);
        void Remove(Rectangle pos);
    }
}
