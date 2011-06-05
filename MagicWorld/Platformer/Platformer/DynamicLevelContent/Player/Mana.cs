using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Constants;

namespace MagicWorld.DynamicLevelContent.Player
{
    class Mana
    {
        public const int MAX_MANA = 1000;
        private const float MANA_REGENERATION_RATE = 0.25f;

        private MagicWorld.Player player;

        private int _currentMana;

        public int CurrentMana {
            get { return _currentMana; }
            set {
                _currentMana = value;
            }
        }

        /// <summary>
        /// Constructor
        /// inits Mana with MAX_MANA
        /// </summary>
        public Mana(MagicWorld.Player player)
        {
            _currentMana = MAX_MANA;
            this.player = player;
        }

        /// <summary>
        /// update to change mana if no spell is casted
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spell"></param>
        public void update(GameTime gameTime) {
            if (!player.IsCasting)
            {
                _currentMana += (int)(gameTime.ElapsedGameTime.Milliseconds * MANA_REGENERATION_RATE);
                if (_currentMana > MAX_MANA)
                {
                    _currentMana = MAX_MANA;
                }
                
            }
        }


        /// <summary>
        /// use while casting Spell to check if enough mana is available
        /// if enoug mana is available the costs are substracted
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>false if not enough mana is available to cast</returns>
        public bool castingSpell(GameTime gameTime)
        {
            if (!DebugValues.DEBUG_NO_MANA_COST)
            {
                if (player.CurrentSpell != null)
                {
                    int cost = (int)(gameTime.ElapsedGameTime.Milliseconds * player.CurrentSpell.ManaCastingCost);
                    if (cost < _currentMana)
                    {
                        _currentMana -= cost;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// use to try cast a spell
        /// if enough mana is availiabe the mana costs are substracted
        /// </summary>
        /// <param name="spell">spell to be casted</param>
        /// <returns>true if enough mana is availabele, false if not</returns>
        public bool startCastingSpell(Spell spell)
        {
            if (!DebugValues.DEBUG_NO_MANA_COST)
            {
                if (spell.ManaBasicCost < _currentMana)
                {
                    _currentMana -= spell.ManaBasicCost;
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public void drawHud(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            Color color = Color.Blue;
            String str = "Mana: " + CurrentMana;

            spriteBatch.DrawString(font, str, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, str, position, color);
        }
    }
}
