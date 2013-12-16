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
    class playerShip
    {

        // player model 
         public Model playerModel;
        // Position of the Player relative to the upper left side of the screen
         public Vector3 playerPosition;
        // State of the player
        public bool Active;
        // Amount of hit points that player has
        public float Health;
        // Boost Amount 
        public float boostEnergy;
        // player lives 
        public int lives = 5;
        // player speed 
        public Vector3 playerSpeed;
        //ship bounding
        public BoundingSphere playerBox;
        // Model transforms
        public Matrix[] playerTransforms;
        // rotation 
        public float modelRotation = 20.5f;
        public float rollAngle;

        
        
        

        public void Initialize()
        {
            playerPosition = Vector3.Zero;
            Active = true;
            Health = 10;
            boostEnergy = 10;
            playerSpeed = Vector3.Zero;
            
            
        }

        public void MovePlayer()
        {


            // input for controller 
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            KeyboardState keyboardState = Keyboard.GetState();
            // Create some velocity if the right trigger is down.
            Vector3 mdlVelocityLeftRight = Vector3.Zero;
            mdlVelocityLeftRight.X = 1.0f;
            // Velocity for veritcal movement 
            Vector3 mdlVelocityUpDown = Vector3.Zero;
            mdlVelocityUpDown.Y = 1.0f;
            // Boost Velocity 
            Vector3 mdlVelocityBoostRight = Vector3.Zero;
            mdlVelocityBoostRight.X = 2.5f;



            if (gamePadState.IsConnected)
            {


                if (gamePadState.Buttons.X == ButtonState.Pressed)
                {
                    mdlVelocityLeftRight *= -3.00f;
                    playerSpeed += mdlVelocityLeftRight;
                }
                if (gamePadState.Buttons.Y == ButtonState.Pressed)
                {
                    mdlVelocityUpDown *= 2.00f;
                    playerSpeed += mdlVelocityUpDown;
                }
                if (gamePadState.Buttons.A == ButtonState.Pressed)
                {
                    mdlVelocityUpDown *= -2.00f;
                    playerSpeed += mdlVelocityUpDown;
                }
                if (gamePadState.Buttons.B == ButtonState.Pressed)
                {
                    mdlVelocityLeftRight *= 3.00f;
                    playerSpeed += mdlVelocityLeftRight;
                }
                if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    mdlVelocityUpDown *= 8.00f;
                    playerSpeed += mdlVelocityUpDown;
                }
                if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    mdlVelocityUpDown *= -8.00f;
                    playerSpeed += mdlVelocityUpDown;
                }

            }


            if (keyboardState.IsKeyDown(Keys.Right))
            {
                //Move Up.
                // Create some velocity if the right trigger is down.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityLeftRight *= 3.00f;
                playerSpeed += mdlVelocityLeftRight;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                // Move Down.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityLeftRight *= -3.00f;
                playerSpeed += mdlVelocityLeftRight;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                //Move Up.
                // Create some velocity if the right trigger is down.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityUpDown *= 2.00f;
                playerSpeed += mdlVelocityUpDown;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                // Move Down.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityUpDown *= -2.00f;
                playerSpeed += mdlVelocityUpDown;
            }
            // boost Right
            if (keyboardState.IsKeyDown(Keys.X))
            {
                // Boost Right.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityUpDown *= -8.00f;
                playerSpeed += mdlVelocityUpDown;
            }
            if (keyboardState.IsKeyDown(Keys.Z))
            {
                // Roll Up.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityUpDown *= 8.00f;
                playerSpeed += mdlVelocityUpDown;
                rollAngle = 180;
                
            }
            if (keyboardState.IsKeyUp(Keys.Z))
            {
                // Roll Up.
                // Now scale our direction by how hard the trigger is down.
                rollAngle = 0;

            }
            
            


        }


        public void Update()
        {
            MovePlayer();
            playerPosition += playerSpeed;
            // Bleed off velocity over time.
            playerSpeed *= 0.95f;
            playerBox = new BoundingSphere(playerPosition, playerModel.Meshes[0].BoundingSphere.Radius * GameConstants.ShipBoundingSphereScale);

            if (Health == 0)
            {
                lives--;
                playerPosition = Vector3.Zero;
                Health = 5;
            }

            

        }

        public void Draw()
        {
        }
    }
}
