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
               Vector2 pos = player.getCurrentSpellPosition();

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
                   case SpellType.ElectricSpell:
                       player.CurrentSpell = new ElectricSpell("ElectricSpell", pos, level); break;
                   case SpellType.PushSpell:
                       player.CurrentSpell = new PushSpell("PushSpell", pos, level); break;
                   case SpellType.PullSpell:
                       player.CurrentSpell = new PullSpell("PullSpell", pos, level); break;
                   default:
                       throw new NotImplementedException();
               }
               Vector2 direction = pos - player.Position;
               direction.Normalize();
               player.CurrentSpell.Direction = direction;
               player.CurrentSpell.Rotation =  -(float)(level.Player.SpellAimAngle + Math.PI / 2);
               player.SpellSound.Play();               
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
            //update position and direction and drawing angle
            Vector2 pos = player.getCurrentSpellPosition();
            Vector2 direction = pos - player.Position;
            direction.Normalize();
            player.CurrentSpell.Direction = direction;
            player.CurrentSpell.Position = pos;
            player.CurrentSpell.Rotation = -(float)(level.Player.SpellAimAngle + Math.PI / 2);
            //add new particle effects
            player.CurrentSpell.AddOnCreationParticles();
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
