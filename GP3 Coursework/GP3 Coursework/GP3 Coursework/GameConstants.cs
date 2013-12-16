using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GP3_Coursework
{
     public static class GameConstants
    {
         // boundings 
        public const float ShipBoundingSphereScale = 1.2f;//50% size
        public const float basicEnemyBoundingSphereScale = 5.5f;
        public const float fastEnemyBoundingSphereScale = 1.0f;
        public const float strongEnemyBoundingSphereScale = 20.0f;

         // play field size 
        public const float PlayfieldSizeX = 3310f;
        public const float PlayfieldSizeZ = 3000f;

         // enemy play field 
        public const float enemyPlayfieldSizeX = 3300f;
        public const float enemyPlayfieldSizeZ = 300f;

         // speeds 
        public const float basicEnemySpeedAdjustment = 6.5f;
        public const float fastEnemySpeedAdjustment = 11.5f;
        public const float strongEnemySpeedAdjustment = 3.5f;

         // enemy numbers 
        public const int basicEnemyNumbers = 10;
        public const int fastEnemyNumbers = 10;
        public const int strongEnemyNumbers = 10;
        // player basic weapon constants
        public const int NumBasicBullets = 30;
        public const float basicWeaponBoundingSphereScale = 2.5f;
        public const float basicWeaponSpeedAdjustment = 15.5f;
         // enemy basic weapon constants
        public const int NumBasicEnemyBullets = 30;
        public const float basicEnemyWeaponBoundingSphereScale = 1.5f;
        public const float basicEnemyWeaponSpeedAdjustment = 15.5f;




    }
}
