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
    struct PlayerBasicWeapon
    {
        // position 
        public Vector3 basicWeaponBulletposition;
        // direction 
        public Vector3 direction;
        // bullet speed 
        public float basicBulletspeed;
        // is active 
        public bool isActive;

        public void Update(float delta)
        {
            
            // update position 
            basicWeaponBulletposition += direction * basicBulletspeed * GameConstants.basicWeaponSpeedAdjustment * delta;
            
            // play field size 
            if (basicWeaponBulletposition.X > GameConstants.PlayfieldSizeX ||
                basicWeaponBulletposition.X < -GameConstants.PlayfieldSizeX ||
                basicWeaponBulletposition.Z > GameConstants.PlayfieldSizeZ ||
                basicWeaponBulletposition.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;
        }
    }
}
    

