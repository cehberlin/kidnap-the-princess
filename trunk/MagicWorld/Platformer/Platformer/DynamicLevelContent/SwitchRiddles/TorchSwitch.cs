using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    public class TorchSwitch : AbstractSwitch
    {

        /// <summary>
        /// true if the torch is lit
        /// </summary>
        public Boolean IsLit { get; set; }

       public TorchSwitch(String texture,  Level level, Vector2 position, String id, bool isLit)
            : base(texture, CollisionType.Passable , level, position, id)
        {
            this.IsLit = isLit;
            throw new NotImplementedException(); //TODO: implement class
        }
    }
}
