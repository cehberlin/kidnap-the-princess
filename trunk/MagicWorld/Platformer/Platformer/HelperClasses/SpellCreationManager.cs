using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.Spells;
using MagicWorld.DynamicLevelContent.Player;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace MagicWorld.HelperClasses
{
    public class SpellCreationManager
    {
        
        /// <summary>
        /// called if the player create a spell
        /// </summary>
        /// <param name="type"></param>
        /// <returns>true if casting started successfull</returns>
        public static bool tryStartCasting(Player player, SpellType type, Level level)
        {
           if(player.Mana.CurrentMana > SpellConstantsFactory.getSpellConstants(type).BasicCastingCost)
           {
               Vector2 pos;
               pos.X = player.Position.X + 20 * player.Direction.X;
               pos.Y = player.Position.Y + player.Bounds.Height / 2;

               switch (type)
               {
                   case SpellType.ColdSpell:
                       player.CurrentSpell = new ColdSpell("ColdSpell", pos, level);break;
                   case SpellType.CreateMatterSpell:
                       player.CurrentSpell = new MatterSpell("MatterSpell", pos, level);break;
                   case SpellType.NoGravitySpell:
                       player.CurrentSpell = new NoGravitySpell("NoGravitySpell", pos, level); break;
                   case SpellType.WarmingSpell:
                       player.CurrentSpell = new WarmSpell("WarmingSpell", pos, level); break;

               }
               

               player.SpellSound.Play();                   
               player.CurrentSpell.Direction = player.Direction;
               level.addSpell(player.CurrentSpell);
               return true;
           }
           return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="level"></param>
        /// <param name="gameTime"></param>
        /// <returns>true if the spell is still casted</returns>
        public static bool furtherSpellCasting(Player player, Level level, GameTime gameTime)
        {
            if (!player.Mana.castingSpell(gameTime))
            {
                releaseSpell(player);
                return false;
            }
            return true;
        }

        public static void releaseSpell(Player player)
        { 
            Debug.WriteLine("SPELL:FIRED after button release");
            player.CurrentSpell.FireUp();
            player.CurrentSpell = null;
        }

    }
}
