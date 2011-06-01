using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.GameConstants
{
    class DefaultPlayerConstants : IPlayerConstants
    {
        private float MaxJumpTime = 0.18f;//0.15f//0.25f; //original 0.35f
        private float JumpLaunchVelocity = -3500.0f;
        private float GravityAcceleration = 3400.0f;
        private float MaxFallSpeed = 550.0f;
        private float JumpControlPower = 0.14f;
        private double MaxNoGravityTime = 1000;
        private float MoveAcceleration = 13000.0f;
        private float MaxMoveSpeed = 1750.0f;
        private float GroundDragFactor = 0.48f;
        private float AirDragFactor = 0.58f;


        #region IPlayerConstants Member

        public float MAXJUMPTIME
        {
            get
            {
                return MaxJumpTime;
            }
            set
            {
                MaxJumpTime = value;
            }
        }

        public float JUMPLAUNCHVELOCITY
        {
            get
            {
                return JumpLaunchVelocity;
            }
            set
            {
                JumpLaunchVelocity = value;
            }
        }

        public float GRAVITYACCELARATION
        {
            get
            {
                return GravityAcceleration;
            }
            set
            {
                GravityAcceleration = value;
            }
        }

        public float MAXFALLSPEED
        {
            get
            {
                return MaxFallSpeed;
            }
            set
            {
                MaxFallSpeed = value;
            }
        }

        public float JUMPCONTROLPOWER
        {
            get
            {
                return JumpControlPower;
            }
            set
            {
                JumpControlPower = value;
            }
        }

        public double MAXNOGRAVITYTIME
        {
            get
            {
                return MaxNoGravityTime;
            }
            set
            {
                MaxNoGravityTime = value;
            }

        #endregion
        }

        #region IPlayerConstants Member


        public float MOVEACCELERATION
        {
            get
            {
                return MoveAcceleration;
            }
            set
            {
                MoveAcceleration = value;
            }
        }

        public float MAXMOVESPEED
        {
            get
            {
                return MaxMoveSpeed;
            }
            set
            {
                MaxMoveSpeed = value;
            }
        }

        public float GROUNDDRAGFACTOR
        {
            get
            {
                return GroundDragFactor;
            }
            set
            {
                GroundDragFactor = value;
            }
        }

        public float AIRDRAGFACTOR
        {
            get
            {
                return AirDragFactor;
            }
            set
            {
                AirDragFactor = value;
            }
        }

        #endregion
    }
}
