using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MagicWorld.Controls
{
    /// <summary>
    /// Defines the player Controls
    /// </summary>
    public interface IPlayerControl
    {
        /// <summary>
        /// GamePad Button for decreasing the power of the spell
        /// </summary>
        Buttons GamePad_DecreaseSpell { get; }

        /// <summary>
        /// GamePad Button for increasing the power of the spell
        /// </summary>
        Buttons GamePad_IncreaseSpell { get; }

        /// <summary>
        /// GamePad Button for Casting Selected Spell A
        /// </summary>
        Buttons GamePad_CastSelectedSpellA { get; }

        /// <summary>
        /// Keyboard Button for Casting Selected Spell A
        /// </summary>
        Keys Keys_CastSelectedSpellA { get; }

        /// <summary>
        /// GamePad Button for Casting Selected Spell B
        /// </summary>
        Buttons GamePad_CastSelectedSpellB { get; }

        /// <summary>
        /// Keyboard Button for Casting Selected Spell B
        /// </summary>
        Keys Keys_CastSelectedSpellB { get; }

        /// <summary>
        /// GamePad Button for Selecting Next Spell A
        /// </summary>
        Buttons GamePad_SelectedSpellA { get; }

        /// <summary>
        /// GamePad Button for Selecting Next Spell A
        /// </summary>
        Keys Keys_SelectedSpellA { get; }

        /// <summary>
        /// GamePad Button for Selecting Next Spell B
        /// </summary>
        Buttons GamePad_SelectedSpellB { get; }

        /// <summary>
        /// Keyboard Button for Selecting Next Spell B
        /// </summary>
        Keys Keys_SelectedSpellB { get; }

        /// <summary>
        /// GamePad Button for jumping
        /// </summary>
        Buttons GamePad_Jump { get; }

        /// <summary>
        /// Keyboard Button for jumping
        /// </summary>
        Keys Keys_Jump { get; }

        /// <summary>
        /// GamePad Button for pausing the Game
        /// </summary>
        Buttons GamePad_Pause { get; }

        /// <summary>
        /// Keyboard Button for pausing the game
        /// </summary>
        Keys Keys_Pause { get; }

        /// <summary>
        /// game pad button for moving up
        /// </summary>
        Buttons GamePad_Up { get; }

        /// <summary>
        /// Keys button for moving up
        /// </summary>
        Keys Keys_Up { get; }

        /// <summary>
        /// game pad button for moving  down
        /// </summary>
        Buttons GamePad_Down { get; }

        /// <summary>
        /// Keys button for moving down
        /// </summary>
        Keys Keys_Down { get; }

        /// <summary>
        /// game pad button for moving left
        /// </summary>
        Buttons GamePad_Left { get; }

        /// <summary>
        /// Keys button for moving left
        /// </summary>
        Keys Keys_Left { get; }

        /// <summary>
        /// game pad button for moving right
        /// </summary>
        Buttons GamePad_Right { get; }

        /// <summary>
        /// Keys button for moving right 
        /// </summary>
        Keys Keys_Right { get; }
    }
}
