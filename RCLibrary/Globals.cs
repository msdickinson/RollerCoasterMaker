using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLibrary
{
    static public class Globals
    {
        #region Constants

        //Start Postion
        public const float START_X = 500;
        public const float START_Y = -150;
        public const float START_Z = 0;

        public const float START_YAW = 0;
        public const float START_PITCH = 0;
        public const float START_ROLL = 0;

        //Start Build Area (Note: Build area only allows postive values. so size 100. means 0-100)
        public const float BUILD_AREA_SIZE_X = 1000;
        public const float BUILD_AREA_SIZE_Y = 1000;
        public const float BUILD_AREA_SIZE_Z = 2500;

        public const float FINSH_AREA_X = 260;
        public const float FINSH_AREA_Y = 5;
        public const float FINSH_AREA_Z = 150;

        public const float FINSH_AREA_X_RANGE = 150;
        public const float FINSH_AREA_Y_RANGE = 150;
        public const float FINSH_AREA_Z_RANGE = 200;


      //  public const float REGION_LENGTH = 4;
        public const float TRACK_HALF_LENGH = TRACK_LENGTH / 2;
        public const float TRACK_HALF_LENGH_SQUARED = TRACK_HALF_LENGH * TRACK_HALF_LENGH;
        public const int REGIONS = 25;
        public const float X_REGION_LENGTH = BUILD_AREA_SIZE_X / REGIONS;
        public const float Y_REGION_LENGTH = BUILD_AREA_SIZE_Y / REGIONS;
        public const float Z_REGION_LENGTH = BUILD_AREA_SIZE_Z / REGIONS;

        //Track
        public const int MAX_TRACKS = 5000;
        public const float TRACK_LENGTH_2X = (float)7.7 * 2;
        public const float TRACK_LENGTH = (float)7.7;
        public const float HALF_TRACK_LENGTH = (float)7.7/2;
        public const float TRACK_HIGHT = (float)1.2;
        public const float TRACK_WIDTH = (float)3;

        //Cart
        public const float CART_HEIGHT = (float)5;
        public const float CART_SCALE = .05f;

        //Angle Amount
        public const float STANDARD_ANGLE_CHANGE = 7.5f;

        //Camera
        public const float CAMERA_HEIGHT = 7f;

        //Physics
        public const float GRAVITY = 9.8f;

        //MinSpeed
        public const float CART_MIN_SPEED = 2.0f;
        #endregion

    }
}
