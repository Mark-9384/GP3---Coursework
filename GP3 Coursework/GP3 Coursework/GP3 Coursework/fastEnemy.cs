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
    struct fastEnemy
    {
        // position 
        public Vector3 fastEnemyposition;
        // direction 
        public Vector3 fastEnemydirection;
        // speed 
        public float fastEnemyspeed;
        // active 
        public bool isActive;
        // health 
        public int health;

        public void Update(float delta)
        {
            // update position 
            fastEnemyposition += fastEnemydirection * fastEnemyspeed * GameConstants.fastEnemySpeedAdjustment * delta;

            // if leaves play field size 
            if (fastEnemyposition.X < -GameConstants.PlayfieldSizeX ||
                fastEnemyposition.Z > GameConstants.PlayfieldSizeZ ||
                fastEnemyposition.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;

            // health = 0 
            if (health == 0)
            {
                isActive = false;
            }
        }
    }
}
