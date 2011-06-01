using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using MagicWorld.AbstractGameElements.BasicShapes;
using Microsoft.Xna.Framework;

namespace MagicWorld.AbstractGameElements
{
    /// <summary>
    /// superclass of spell
    /// </summary>
    public abstract class Spell : IUpdateable
    {

        public abstract SpellType SpellType { get; } 

        public SpellConstants spellConstants{get;set;}

        public Spell(SpellConstants spellConstants)
        {
            this.spellConstants = spellConstants;
        }

#region "IUpdateable"

        public bool Enabled
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public int UpdateOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;

#endregion
    }
}
