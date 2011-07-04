#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using MagicWorld.Controls;
#endregion

namespace MagicWorld
{
    /// <summary>
    /// Helper for reading input from keyboard, gamepad, and touch input. This class 
    /// tracks both the current and previous state of the input devices, and implements 
    /// query methods for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    public class InputState
    {
        #region Fields

        public const int MaxInputs = 1;

        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        public TouchCollection TouchState;

        public readonly List<GestureSample> Gestures = new List<GestureSample>();
        IPlayerControl controls;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];
            
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            controls = PlayerControlFactory.GET_INSTANCE().getPlayerControl();
            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }

            
        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                        LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex));
            }
        }


        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex));
            }
        }


        /// <summary>
        /// Check if any button was pressed
        /// </summary>
        public bool IsAnyButtonPress(PlayerIndex? controllingPlayer)
        {
            int i;
            bool result;
            PlayerIndex playerIndex;
            

            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;

                i = (int)playerIndex;
            }
            else
            {
                i = (int)PlayerIndex.One;
            }


            result = IsNewButtonPress(controls.GamePad_CastSelectedSpellA, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_CastSelectedSpellB, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_DecreaseSpell, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Down, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_IncreaseSpell, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Jump, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Left, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Pause, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Right, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_SelectedSpellA, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_SelectedSpellB, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Up, controllingPlayer, out playerIndex);

            return result;
                       
                        
           
        }
        /// <summary>
        /// Check if any key was pressed
        /// </summary>
        public bool IsContinuePress(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            bool result;
            bool result1;
            
            result= IsNewKeyPress(controls.Keys_Down, controllingPlayer, out playerIndex) ||
                    IsNewKeyPress(controls.Keys_Jump, controllingPlayer, out playerIndex) ||
                    IsNewKeyPress(controls.Keys_Left, controllingPlayer, out playerIndex) ||
                    IsNewKeyPress(controls.Keys_Right, controllingPlayer, out playerIndex) ||
                    IsNewKeyPress(controls.Keys_SelectedSpellA, controllingPlayer, out playerIndex) ||
                    IsNewKeyPress(controls.Keys_SelectedSpellB, controllingPlayer, out playerIndex) ||
                    IsNewKeyPress(controls.Keys_Up, controllingPlayer, out playerIndex) ||
                    IsNewKeyPress(controls.Keys_CastSelectedSpellA, controllingPlayer, out playerIndex) ||
                    IsNewKeyPress(controls.Keys_CastSelectedSpellB, controllingPlayer, out playerIndex);

            result1 = IsNewButtonPress(controls.GamePad_CastSelectedSpellA, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_CastSelectedSpellB, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_DecreaseSpell, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Down, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_IncreaseSpell, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Jump, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Left, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Pause, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Right, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_SelectedSpellA, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_SelectedSpellB, controllingPlayer, out playerIndex) ||
                    IsNewButtonPress(controls.GamePad_Up, controllingPlayer, out playerIndex);

            return (result||result1);
        }

        /// <summary>
        /// Checks for a "menu select" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(controls.Keys_CastSelectedSpellA, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(controls.Keys_CastSelectedSpellB, controllingPlayer, out playerIndex) ||                   
                   IsNewKeyPress(controls.Keys_SelectedSpellA, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(controls.Keys_SelectedSpellB, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_CastSelectedSpellA, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_CastSelectedSpellB, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_Jump, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_SelectedSpellA, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_SelectedSpellB, controllingPlayer, out playerIndex);
            //IsNewKeyPress(controls.Keys_Jump, controllingPlayer, out playerIndex) ||
                   
        }


        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(controls.Keys_Pause, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_Pause, controllingPlayer, out playerIndex);            
        }


        /// <summary>
        /// Checks for a "menu up" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(controls.Keys_Up, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_Up, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu down" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(controls.Keys_Down, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_Down, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Check if Right was selected when in menu screen
        /// </summary>
        /// <param name="controllingPlayer"></param>
        /// <returns></returns>
        public bool IsMenuRight(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(controls.Keys_Right, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_Right, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Check if Left was selected when in menu screen
        /// </summary>
        /// <param name="controllingPlayer"></param>
        /// <returns></returns>
        public bool IsMenuLeft(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(controls.Keys_Left, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_Left, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Checks for a "pause the game" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(controls.Keys_Pause, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(controls.GamePad_Pause, controllingPlayer, out playerIndex);
                   
        }


        #endregion
    }
}
