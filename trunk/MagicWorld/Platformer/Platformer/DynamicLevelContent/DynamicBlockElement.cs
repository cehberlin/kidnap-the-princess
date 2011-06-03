using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.Spells;
using System.Diagnostics;

namespace MagicWorld.DynamicLevelContent
{
    /// <summary>
    /// Base class for special tiles with some self dynamic properties
    /// </summary>
    class DynamicBlockElement:BlockElement
    {
        Boolean isGravity = true;

        public Boolean IsGravity
        {
            get { return isGravity; }
            set { isGravity = value; }
        }

        const double FALLINGINTERVALL = 1000;

        /// <summary>
        /// defines in which time intervalls this tile falls down one tile
        /// </summary>
        double fallingTimeMS = FALLINGINTERVALL;


        protected Boolean gravityIsSetOffBySpell = false;

        public DynamicBlockElement(String texture, BlockCollision collision, Level level, Vector2 position) :
            base(texture, collision, level, position)
        {
        }

        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(NoGravitySpell))
            {
                gravityIsSetOffBySpell = true;
                return false; //do not remove spell
            }

            return base.SpellInfluenceAction(spell);
        }


        public override void Update(GameTime gameTime)
        {
            //TODO
            ////every update cycle in the game the spells where updated before the tiles, so if a no gravity colliates with this tile
            ////it resets the flag
            //#region pseudogravity
            //if (isGravity && !gravityIsSetOffBySpell)
            //{
            //    //falling is not smove it goes in tile steps because of problematic tile layout of game
            //    if (fallingTimeMS <= 0)
            //    {
            //        fallingTimeMS = FALLINGINTERVALL;

            //        //switch current tile with underlying one
            //        Tile tmp = level.GetTile(X, Y + 1);
            //        if (tmp.Texture == null)//empty tile
            //        {
            //            level.Tiles[X, Y + 1] = this;
            //            level.Tiles[X, Y] = tmp;
            //            this.Y++;
            //            tmp.Y--;
            //        }
            //    }
            //    else
            //    {
            //        fallingTimeMS -= gameTime.ElapsedGameTime.TotalMilliseconds;                    
            //    }
            //}
            //else
            //{
            //    Debug.WriteLine("No Gravity in this update cycle");
            //    gravityIsSetOffBySpell = false;
            //}
            //#endregion pseudogravity
        }
    }
}
