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
    struct strongEnemy
    {
        // position 
        public Vector3 strongEnemyposition;
        // direction 
        public Vector3 strongEnemydirection;
        // speed 
        public float strongEnemyspeed;
        // is active 
        public bool isActive;
        // health 
        public int health;


        // update 
        public void Update(float delta)
        {
            // update position 
            strongEnemyposition += strongEnemydirection * strongEnemyspeed * GameConstants.strongEnemySpeedAdjustment * delta;
            // if enemy moves outside of screen 
            if (strongEnemyposition.X < -3400 ||
                strongEnemyposition.Z > GameConstants.PlayfieldSizeZ ||
                strongEnemyposition.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;

            // if health is zero, destroy 
            if (health == 0)
            {
                isActive = false;
            }
        }




    }

}
