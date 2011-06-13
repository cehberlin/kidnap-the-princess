using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld
{
    namespace Constants
    {
        class PhysicValues
        {
            public static Vector2 DEFAULT_GRAVITY = new Vector2(0f,0.1f);

            // Constants for controling vertical movement
            public static float PLAYER_MAX_JUMP_TIME = 0.3f;//0.15f//0.25f; //original 0.35f
            public static float PLAYER_JUMP_LAUNCH_VELOCITY = -3500.0f;
            public static float PLAYER_GRAVITY_ACCELERATIOPM = 3400.0f;
            public static float PLAYER_MAX_FALL_SPEED = 550.0f;
            public static float PLAYER_JUMP_CONTROL_POWER = 0.1f;

            // Constants for controling horizontal movement
            public static float PLAYER_MOVE_ACCELERATION = 18000.0f;
            public static float PLAYER_MAX_MOVE_SPEED = 1750.0f;
            public static float PLAYER_GROUND_DRAG_FACTOR = 0.48f;
            public static float PLAYER_AIR_DRAG_FACTOR = 0.58f;

            public static double PLAYER_MAX_NO_GRAVITY_TIME = 1000;
            public static float SLOW_MOTION_FACTOR = 0.13f;
        }
    }
}
