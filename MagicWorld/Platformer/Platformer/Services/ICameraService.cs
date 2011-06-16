using System;
using Microsoft.Xna.Framework;

namespace MagicWorld.Services
{
    public interface ICameraService : IServiceProvider
    {
        Matrix TransformationMatrix { get; }
        Vector2 Position { get; set; }
        float Zoom { get; set; }
        float Rotation { get; set; }
    }
}
