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
            /// <summary>
            /// Please add constants from the bottom if you want to change them online
            /// </summary>
            /// <returns></returns>
            public static List<ConstantGroup> getChangeableConstants()
            {
                List<ConstantGroup> Constants = new List<ConstantGroup>();

                //group vertical player movement
                List<IConstantChangerItem> groupVerticalPlayerMovement = new List<IConstantChangerItem>();
                groupVerticalPlayerMovement.Add(player_max_jump_time);
                groupVerticalPlayerMovement.Add(player_jump_launch_velocity);
                groupVerticalPlayerMovement.Add(player_gravity_acceleratiopm);
                groupVerticalPlayerMovement.Add(player_max_fall_speed);
                groupVerticalPlayerMovement.Add(player_jump_control_power);                
                Constants.Add(new ConstantGroup("VerticalPlayerMovement", groupVerticalPlayerMovement));

                //group horizontal player movement
                List<IConstantChangerItem> groupHorizontalPlayerMovement = new List<IConstantChangerItem>();
                groupHorizontalPlayerMovement.Add(player_move_acceleration);
                groupHorizontalPlayerMovement.Add(player_max_move_speed);
                groupHorizontalPlayerMovement.Add(player_ground_drag_factor);
                groupHorizontalPlayerMovement.Add(player_air_drag_factor);   
                Constants.Add(new ConstantGroup("HorizontalPlayerMovement", groupHorizontalPlayerMovement));

                //group general physics
                List<IConstantChangerItem> groupgeneralPhysics = new List<IConstantChangerItem>();
                groupgeneralPhysics.Add(new ConstantChangerItemVector("DEFAULT_GRAVITY",ref DEFAULT_GRAVITY,0.01f));
                groupgeneralPhysics.Add(new ConstantChangerItemVector("PUSHPULL_ELEMENT_GRAVITY", ref PUSHPULL_ELEMENT_GRAVITY, 0.01f));
                groupgeneralPhysics.Add(slow_motion_factor);
                Constants.Add(new ConstantGroup("generalPhysics", groupgeneralPhysics));              

                
                return Constants;
            }

            #region general Physics

            public static Vector2 DEFAULT_GRAVITY = new Vector2(0f,1f);
            public static Vector2 PUSHPULL_ELEMENT_GRAVITY = new Vector2(0f, 0.5f);


            public static float SLOW_MOTION_FACTOR
            {
                get { return slow_motion_factor.value; }
            }
            static ConstantChangerItemFloat slow_motion_factor = new ConstantChangerItemFloat("SLOW_MOTION_FACTOR", 0.13f, 0.001f);

            #endregion

            #region Constants for controling vertical movement

            public static float PLAYER_MAX_JUMP_TIME
            {
                get { return player_max_jump_time.value; }
            }
            static ConstantChangerItemFloat player_max_jump_time = new ConstantChangerItemFloat("PLAYER_MAX_JUMP_TIME", 0.3f, 0.01f);

            public static float PLAYER_JUMP_LAUNCH_VELOCITY
            {
                get { return player_jump_launch_velocity.value; }
            }
            static ConstantChangerItemFloat player_jump_launch_velocity = new ConstantChangerItemFloat("PLAYER_JUMP_LAUNCH_VELOCITY", -3500.0f, 50f);

            public static float PLAYER_GRAVITY_ACCELERATIOPM
            {
                get { return player_gravity_acceleratiopm.value; }
            }
            static ConstantChangerItemFloat player_gravity_acceleratiopm = new ConstantChangerItemFloat("PLAYER_GRAVITY_ACCELERATIOPM", 3400.0f, 50f);

            public static float PLAYER_MAX_FALL_SPEED
            {
                get { return player_max_fall_speed.value; }
            }
            static ConstantChangerItemFloat player_max_fall_speed = new ConstantChangerItemFloat("PLAYER_MAX_FALL_SPEED", 550.0f, 10f);

            public static float PLAYER_JUMP_CONTROL_POWER
            {
                get { return player_jump_control_power.value; }
            }
            static ConstantChangerItemFloat player_jump_control_power = new ConstantChangerItemFloat("PLAYER_JUMP_CONTROL_POWER", 0.1f, 0.01f);

            #endregion

            #region Constants for controling horizontal movement
            public static float PLAYER_MOVE_ACCELERATION
            {
                get { return player_move_acceleration.value; }
            }
            static ConstantChangerItemFloat player_move_acceleration = new ConstantChangerItemFloat("PLAYER_MOVE_ACCELERATION", 18000.0f, 100f);

            public static float PLAYER_MAX_MOVE_SPEED
            {
                get { return player_max_move_speed.value; }
            }
            static ConstantChangerItemFloat player_max_move_speed = new ConstantChangerItemFloat("PLAYER_MAX_MOVE_SPEED", 1750.0f, 50f);


            public static float PLAYER_GROUND_DRAG_FACTOR
            {
                get { return player_ground_drag_factor.value; }
            }
            static ConstantChangerItemFloat player_ground_drag_factor = new ConstantChangerItemFloat("PLAYER_GROUND_DRAG_FACTOR", 0.48f, 0.001f);

            public static float PLAYER_AIR_DRAG_FACTOR
            {
                get { return player_air_drag_factor.value; }
            }
            static ConstantChangerItemFloat player_air_drag_factor = new ConstantChangerItemFloat("PLAYER_AIR_DRAG_FACTOR", 0.58f, 0.001f);
            
            #endregion

            public static double PLAYER_MAX_NO_GRAVITY_TIME = 1000; //not activated at the moment


        }
    }
}
