using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GP3_Coursework
{
    struct BasicEnemyWeapon
    {
        // position 
        public Vector3 basicEnemyWeaponBulletposition;
        // direction 
        public Vector3 basicEnemyWeaponBulletdirection;
        // speed 
        public float basicEnemyWeaponBulletspeed;
        // active 
        public bool isActive;


        public void Update(float delta)
        {
            
            // position update 
            basicEnemyWeaponBulletposition += basicEnemyWeaponBulletdirection * basicEnemyWeaponBulletspeed * GameConstants.basicEnemyWeaponSpeedAdjustment * delta;
            
            // if outside of screen destroy 
            if (basicEnemyWeaponBulletposition.X > GameConstants.PlayfieldSizeX ||
                basicEnemyWeaponBulletposition.X < -GameConstants.PlayfieldSizeX ||
                basicEnemyWeaponBulletposition.Z > GameConstants.PlayfieldSizeZ ||
                basicEnemyWeaponBulletposition.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;
        }
    }
    }

