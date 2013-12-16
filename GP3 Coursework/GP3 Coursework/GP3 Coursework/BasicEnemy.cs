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
    struct BasicEnemy
    {
        // position 
        public Vector3 basicEnemyposition;
        // direction 
        public Vector3 basicEnemydirection;
        // speed 
        public float basicEnemyspeed;
        // is active 
        public bool isActive;
        // health 
        public int health;

       
        // update 
        public void Update(float delta)
        {
            // posisition 
            basicEnemyposition += basicEnemydirection * basicEnemyspeed * GameConstants.basicEnemySpeedAdjustment * delta;

            // if outside play field, set to false 
            if (basicEnemyposition.X < -GameConstants.PlayfieldSizeX ||
                basicEnemyposition.Z > GameConstants.PlayfieldSizeZ ||
                basicEnemyposition.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;

            // if health = 0, destroy 
            if (health == 0)
            {
                isActive = false;
            }

        }

    }
}
