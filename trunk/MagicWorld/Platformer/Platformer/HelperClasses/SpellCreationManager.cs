using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.Spells;
using MagicWorld.DynamicLevelContent.Player;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using MagicWorld.Constants;

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
            int basicCastingCost = SpellConstantsFactory.getSpellConstants(type).BasicCastingCost;
           if(player.Mana.CurrentMana > basicCastingCost)
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
               player.CurrentSpell.UsedMana = basicCastingCost;
               return true;
           }
           return false;
        }


        public static void morePower(Player player, Level level, GameTime gameTime )
        {
            double powerUp = SpellConstantsValues.spellPowerUpDownSpeed * gameTime.ElapsedGameTime.TotalSeconds;
            int currentSpellCost = (int)Math.Round(powerUp) + player.CurrentSpell.UsedMana;
            if (currentSpellCost > player.Mana.CurrentMana)
            {
                currentSpellCost = player.Mana.CurrentMana;
            }
            player.CurrentSpell.UsedMana = currentSpellCost;
        }

        public static void lessPower(Player player, Level level, GameTime gameTime)
        {
            int basicCastingCost = SpellConstantsFactory.getSpellConstants(player.CurrentSpell.SpellType).BasicCastingCost;
            double powerDown = SpellConstantsValues.spellPowerUpDownSpeed * gameTime.ElapsedGameTime.TotalSeconds;
            int currentSpellCost = - (int)Math.Round(powerDown) + player.CurrentSpell.UsedMana;
            if (currentSpellCost < basicCastingCost)
            {
                currentSpellCost = basicCastingCost;
            }
            player.CurrentSpell.UsedMana = currentSpellCost;
        }

        /// <summary>
        /// DEPRECATED
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
            Debug.WriteLine("SPELL: FIRED after button release [usedMana:" + player.CurrentSpell.UsedMana+"]");
            player.Mana.CurrentMana -= player.CurrentSpell.UsedMana;
            player.CurrentSpell.FireUp();
            player.CurrentSpell = null;
        }

    }
}
