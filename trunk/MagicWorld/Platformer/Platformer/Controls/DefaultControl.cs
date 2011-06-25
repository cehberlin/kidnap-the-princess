using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Controls
{
    class DefaultControl : IPlayerControl
    {
        public Microsoft.Xna.Framework.Input.Keys Keys_CastSelectedSpellA
        {
            get { return Microsoft.Xna.Framework.Input.Keys.NumPad1; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_CastSelectedSpellB
        {
            get { return Microsoft.Xna.Framework.Input.Keys.NumPad2; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_SelectedSpellA
        {
            get { return Microsoft.Xna.Framework.Input.Keys.NumPad4; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_SelectedSpellB
        {
            get { return Microsoft.Xna.Framework.Input.Keys.NumPad5; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_Jump
        {
            get { return Microsoft.Xna.Framework.Input.Keys.W; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_Pause
        {
            get { return Microsoft.Xna.Framework.Input.Keys.Space; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_Up
        {
            get { return Microsoft.Xna.Framework.Input.Keys.W; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_Down
        {
            get { return Microsoft.Xna.Framework.Input.Keys.S; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_Left
        {
            get { return Microsoft.Xna.Framework.Input.Keys.A; }
        }

        public Microsoft.Xna.Framework.Input.Keys Keys_Right
        {
            get { return Microsoft.Xna.Framework.Input.Keys.D; }
        }

        //Gamepad

        public Microsoft.Xna.Framework.Input.Buttons GamePad_CastSelectedSpellA
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.RightTrigger; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_CastSelectedSpellB
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.LeftTrigger; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_SelectedSpellA
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.RightShoulder; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_SelectedSpellB
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.LeftShoulder; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_Jump
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.A; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_Pause
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.Start; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_Up
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.DPadUp; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_Down
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.DPadDown; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_Left
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.DPadLeft; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_Right
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.DPadRight; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_IncreaseSpell
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.X; }
        }

        public Microsoft.Xna.Framework.Input.Buttons GamePad_DecreaseSpell
        {
            get { return Microsoft.Xna.Framework.Input.Buttons.Y; }
        }
    }
}
