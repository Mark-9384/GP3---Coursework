using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GP3_Coursework
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        // graphics variables 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Image used to display the static background
        Texture2D mainBackground;

        // Font 
        SpriteFont scoreFont;

        //player score 
        public int score;

        // space Station health
        public int spaceStationHealth;

        //player 
        playerShip player;

        // Parallaxing Layers
        Background bgLayer1;
        Background bgLayer2;

        // The aspect ratio determines how to scale 3d to 2d projection.
        float aspectRatio;

        //Camera 1 position
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        // View 1  Matrix
        Matrix viewMatrix;
        // Projection 1 Matrix
        Matrix projectionMatrix;

        // camera 2 position
        Vector3 camera2Position = new Vector3(0.0f, 50.0f, 5000.0f);
        // View 2 Matrix
        Matrix view2Matrix;
        // Projection 2 Matrix
        Matrix projection2Matrix;


        // random number generator
        Random random;

        // basic Weapon Bullet 
        private PlayerBasicWeapon[] basicWeaponBulletList = new PlayerBasicWeapon[GameConstants.NumBasicBullets];
        // model for basic weapons (player)
        public Model basicWeaponBulletModel;
        // Matrix for storing basicweapon bullet transforms
        public Matrix[] basicWeaponbulletTransforms;

        // basic enemy model
        private Model basicEnemyModel;
        // matrix for basicEnemy transforms 
        private Matrix[] basicEnemyModelTransforms;
        // basicEnemyList
        private BasicEnemy[] basicEnemyList = new BasicEnemy[GameConstants.basicEnemyNumbers];

        // fast enemy model
        private Model fastEnemyModel;
        // matrix for basicEnemy transforms 
        private Matrix[] fastEnemyModelTransforms;
        // basicEnemyList
        private fastEnemy[] fastEnemyList = new fastEnemy[GameConstants.fastEnemyNumbers];

        // strong enemy model
        private Model strongEnemyModel;
        // matrix for strongEnemy transforms 
        private Matrix[] strongEnemyModelTransforms;
        // strongEnemyList
        private strongEnemy[] strongEnemyList = new strongEnemy[GameConstants.strongEnemyNumbers];

        // basic enemy bullet model 
        private Model basicEnemyWeaponBulletmodel;
        // matrix for basic enemy bullet transforms
        private Matrix[] basicEnemyWeaponbulletTransforms;
        // list for basic enemy bullets 
        private BasicEnemyWeapon[] basicEnemyWeaponBulletList = new BasicEnemyWeapon[GameConstants.NumBasicEnemyBullets];

        // strong enemy bullet model 
        private Model strongEnemyWeaponBulletmodel;
        // matrix for strong enemy bullet transforms
        private Matrix[] strongEnemyWeaponbulletTransforms;
        // list for basic enemy bullets 
        private BasicEnemyWeapon[] strongEnemyWeaponBulletList = new BasicEnemyWeapon[GameConstants.NumBasicEnemyBullets];


        // last state for keyboard
        private KeyboardState lastState;
        // old keyboard state 
        KeyboardState oldState;

        // hit count
        private int hitCount;
        
        // reload timer 
        public Timer playerReloadTime;
        // basic Enemy Timer 
        public Timer enemySpawnTime;
        // fast enemy Timer 
        public Timer fastEnemySpawnTime;
        // strong enemy timer 
        public Timer strongEnemySpawnTime;
        // enemy bullet timer 
        public Timer basicEnemyBulletTimer;
        // strong enemy bullet timer 
        public Timer strongEnemyBulletTimer;

        
        // Sprite explosion variables 
        Texture2D explosionTexture;
        List<Animation> explosions;

        // background song 
        private Song backgroundSong;
        // toggle sound effects boolean
        public Boolean musicToggle;
        // list for crash sound effects 
        static int iMaxCrashSounds = 5;
        private static SoundEffect[] CrashSounds = new SoundEffect[iMaxCrashSounds];
        // sound effects for lasers 
        static int iMaxLaserSounds = 3;
        private static SoundEffect[] LaserSounds = new SoundEffect[iMaxLaserSounds];

        

        //Initialize view and projection
        private void InitializeTransform()
        {
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10000.0f);
        }

        // Initialize second View and Projection 
        private void InitilizeSecondTransform()
        {
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10000.0f);
        }
    
        // shoot bullets
        public void shootbullets()
        {
            // get keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // if space bar is pressed 
            if (keyboardState.IsKeyDown(Keys.Space) || lastState.IsKeyDown(Keys.Space)) 
            {
                // for each bullet defined in gameConstants 
                for (int i = 0; i < GameConstants.NumBasicBullets; i++)
                {
                    // if the bullet isnt active and reload time is 0 
                    if (!basicWeaponBulletList[i].isActive && playerReloadTime.time == 0)
                    {
                        // set bullet direction 
                        basicWeaponBulletList[i].direction = new Vector3(5.0f, 0.0f, 0.0f);
                        // set bullet speed
                        basicWeaponBulletList[i].basicBulletspeed = GameConstants.basicWeaponSpeedAdjustment;
                        // bullet position 
                        basicWeaponBulletList[i].basicWeaponBulletposition = player.playerPosition;
                        // set bullet to active
                        basicWeaponBulletList[i].isActive = true;
                        
                        // play laser sound at [0]
                        LaserSounds[0].Play();

                        // set reload time to 2 
                        playerReloadTime.time = 2;
                        break;
                    }
                }
            }
            lastState = keyboardState;
        }
        // allow bullets to be shot by gamePad 
        public void gamePadShootBullets()
        {
            // input for controller 
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            // right trigger variable 
            float rightTriggerValue = gamePadState.Triggers.Right;

            // if gamepad is connected 
            if (gamePadState.IsConnected)
            {
                // if right trigger is pressed 
                if (rightTriggerValue == 1)
                {
                    // for each bullet defined in game constants 
                    for (int i = 0; i < GameConstants.NumBasicBullets; i++)
                    {
                        // is the bullet isnt active and reload time = 0
                        if (!basicWeaponBulletList[i].isActive && playerReloadTime.time == 0)
                        {
                            // set bullet direction 
                            basicWeaponBulletList[i].direction = new Vector3(5.0f, 0.0f, 0.0f);
                            // set bullet speed 
                            basicWeaponBulletList[i].basicBulletspeed = GameConstants.basicWeaponSpeedAdjustment;
                            // set bullet position 
                            basicWeaponBulletList[i].basicWeaponBulletposition = player.playerPosition;
                            // set bullet to active 
                            basicWeaponBulletList[i].isActive = true;

                            // play laser sound 0 
                            LaserSounds[0].Play();

                            // set reload time till 2 
                            playerReloadTime.time = 2;
                            break;
                        }
                    }

                }
            }
        }


        // spawning methods 
        public void spawnBasicEnemies()
        {
            // for each number defined in game constants 
            for (int i = 0; i < GameConstants.basicEnemyNumbers; i++)

                // if enemy isnt active and spawn time = 0 
                if (!basicEnemyList[i].isActive && enemySpawnTime.time == 0)
            {
                // position 
                basicEnemyList[i].basicEnemyposition = new Vector3(4000, random.Next(-1850, 1850), 0.0f);
                // direction 
                basicEnemyList[i].basicEnemydirection = new Vector3(-10.0f,0.0f,0.0f);
                // speed 
                basicEnemyList[i].basicEnemyspeed = GameConstants.basicEnemySpeedAdjustment;
                // health 
                basicEnemyList[i].health = 3;
                    // is active = true 
                basicEnemyList[i].isActive = true;
                // spawn time 
                enemySpawnTime.time = 7;
                break;

            }


        }
        public void spawnFastEnemies()
        {
            // for each number defined in game constants 
            for (int i = 0; i < GameConstants.fastEnemyNumbers; i++)
                // is enemy isnt active and spawn time = 0 
                if (!fastEnemyList[i].isActive && fastEnemySpawnTime.time == 0)
                {
                    // position 
                    fastEnemyList[i].fastEnemyposition = new Vector3(4000, random.Next(-1850, 1850), 0.0f);
                    // direction 
                    fastEnemyList[i].fastEnemydirection = new Vector3(-10.0f, 0.0f, 0.0f);
                    // speed 
                    fastEnemyList[i].fastEnemyspeed = GameConstants.fastEnemySpeedAdjustment;
                    // health 
                    fastEnemyList[i].health = 1;
                    // set to active 
                    fastEnemyList[i].isActive = true;

                    // respawn time 
                    fastEnemySpawnTime.time = 15;
                    break;

                }
        }
        public void spawnStrongEnemies()
        {
            // number of strong enemys defined in game constants 
            for (int i = 0; i < GameConstants.strongEnemyNumbers; i++)
                
                // if enemy isnt active and spawn time is 0 
                if (!strongEnemyList[i].isActive && strongEnemySpawnTime.time == 0)
                {
                    // position 
                    strongEnemyList[i].strongEnemyposition = new Vector3(4000, random.Next(-1850, 1850), 0.0f);
                    // direction 
                    strongEnemyList[i].strongEnemydirection = new Vector3(-10.0f, 0.0f, 0.0f);
                    // speed 
                    strongEnemyList[i].strongEnemyspeed = GameConstants.strongEnemySpeedAdjustment;
                    // health 
                    strongEnemyList[i].health = 10;
                    // set to active 
                    strongEnemyList[i].isActive = true;

                    // spawn time set to 60 seconds 
                    strongEnemySpawnTime.time = 60;
                    break;

                }


        }
        public void spawnBasicEnemybullets()
        {
            // number of basic bullets 
            for (int i = 0; i < GameConstants.NumBasicEnemyBullets; i++)
            
                // if bullet isnt active and spawn time is 0 
                if (!basicEnemyWeaponBulletList[i].isActive && basicEnemyBulletTimer.time == 0)
                {
                    // for each basic enemy 
                   for (int K = 0; K < GameConstants.basicEnemyNumbers; K++)
                      
                    {
                       // direction 
                        basicEnemyWeaponBulletList[i].basicEnemyWeaponBulletdirection = new Vector3(-10.0f, 0.0f, 0.0f);
                       // speed 
                        basicEnemyWeaponBulletList[i].basicEnemyWeaponBulletspeed = GameConstants.basicEnemyWeaponSpeedAdjustment;
                        // if basic enemy at K is active 
                       if (basicEnemyList[K].isActive)
                            {
                                basicEnemyWeaponBulletList[i].basicEnemyWeaponBulletposition = basicEnemyList[K].basicEnemyposition;
                            }
                       // bullet is set to true 
                        basicEnemyWeaponBulletList[i].isActive = true;
                       // play laser sounds 
                        LaserSounds[0].Play();
                       // set spawn time 
                        basicEnemyBulletTimer.time = 3;
                    }
                }
            
        }
        public void spawnStrongEnemybullets()
        {
            // number of bullets 
            for (int i = 0; i < GameConstants.NumBasicEnemyBullets; i++)

                // is bullet isnt active and spawn time = 0 
                if (!strongEnemyWeaponBulletList[i].isActive && strongEnemyBulletTimer.time == 0)
                {
                    // for each strong enemy 
                    for (int K = 0; K < GameConstants.strongEnemyNumbers; K++)
                    {
                        // bullet direction  
                        strongEnemyWeaponBulletList[i].basicEnemyWeaponBulletdirection = new Vector3(-10.0f, 0.0f, 0.0f);
                        // bullet speed 
                        strongEnemyWeaponBulletList[i].basicEnemyWeaponBulletspeed = GameConstants.basicEnemyWeaponSpeedAdjustment;
                        // if strong enemy is active 
                        if (strongEnemyList[K].isActive)
                        {
                            // bullet position set to enemy position 
                            strongEnemyWeaponBulletList[i].basicEnemyWeaponBulletposition = strongEnemyList[K].strongEnemyposition;
                        }
                        // bullet set to active 
                        strongEnemyWeaponBulletList[i].isActive = true;
                        // play laser sounds 
                        LaserSounds[0].Play();
                        // timer set to 3 seconds 
                        strongEnemyBulletTimer.time = 3;
                    }
                }

        }

        // explosion method 
        /*private void AddExplosion(Vector2 position)
        {
         
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 1340, 1340, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }*/

        // method for writing text 
        private void writeText(string msg,Vector2 msgPos, Color msgCol)
        {
            // start sprite batch 
            spriteBatch.Begin();

            // message to output 
            string output = msg;
            
            Vector2 FontOrigin = scoreFont.MeasureString(output) / 2;
            // position 
            Vector2 FontPos = msgPos;
            // draw message 
            spriteBatch.DrawString(scoreFont, output, FontPos,Color.Yellow);

            // end sprite batch 
            spriteBatch.End();
        }

        // set up model for drawing
        private Matrix[] SetupEffectTransformDefaults(Model myModel)
        {
            // matrix for each mesh count 
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            // foreach mesh in the model 
            foreach (ModelMesh mesh in myModel.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }
            return absoluteTransforms;
        }

        // draw model method
        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                    
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

        // ability to toggle audio 
        public void toggleAudio()
        {
            // get keyboard state 
            KeyboardState keyboardState = Keyboard.GetState();
            
            // keyboard state setup 
            if (keyboardState.GetPressedKeys() == oldState.GetPressedKeys())
            {
                return;
            }

            // if music toggle is true  
            if (musicToggle == true)
            {
                // if Q is pressed 
                if (oldState.IsKeyUp(Keys.Q) && keyboardState.IsKeyDown(Keys.Q))
                {
                    // music toggle is false 
                    musicToggle = false;
                    // music is muted 
                    MediaPlayer.IsMuted = true;
                }
            }
            // if music toggle is false 
            else if (musicToggle == false)
            {
                // if Q is pressed 
                if (oldState.IsKeyUp(Keys.Q) && keyboardState.IsKeyDown(Keys.Q))
                {
                    // music toggle = true
                    musicToggle = true;
                    // music is not muted 
                    MediaPlayer.IsMuted = false;
                    
                }
            }
            // old state = keyboard state 
            oldState = keyboardState;
        }

        // game1 code 
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize background layer 1 & 2
            bgLayer1 = new Background();
            bgLayer2 = new Background();

            // initialize transform
            InitializeTransform();
            
            // Initialize our random number generator
            random = new Random();

            // set music to on 
            musicToggle = true;

            // Initialize hit counter
            hitCount = 0;

            // explosion animation
            explosions = new List<Animation>();

            // Initialize Score 
            score = 0;

            // space Station 
            spaceStationHealth = 100;

            // Initialize Player
            player = new playerShip();

            // Initialize reload timer 
            playerReloadTime = new Timer();

            // Initialize enemy spawn timer
            enemySpawnTime = new Timer();

            // initialize fast spawn timer
            fastEnemySpawnTime = new Timer();

            // initialize strong enemy timer 
            strongEnemySpawnTime = new Timer();

            // initialize basic enemy bullet 
            basicEnemyBulletTimer = new Timer();

            // initialize strong enemy bullet 
            strongEnemyBulletTimer = new Timer();

            // Initialize
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            // Load player model
            player.playerModel = Content.Load<Model>("Models\\Feisar_Ship");
            player.playerTransforms = SetupEffectTransformDefaults(player.playerModel);
            // Load basicweapon bullets
            basicWeaponBulletModel = Content.Load<Model>("Models\\massiveorb");
            basicWeaponbulletTransforms = SetupEffectTransformDefaults(basicWeaponBulletModel);
            // load basic enemy 
            basicEnemyModel = Content.Load<Model>("Models\\cottus_elec");
            basicEnemyModelTransforms = SetupEffectTransformDefaults(basicEnemyModel);
            // load fast enemy 
            fastEnemyModel = Content.Load<Model>("Models\\Feisar_Ship");
            fastEnemyModelTransforms = SetupEffectTransformDefaults(fastEnemyModel);
            // load strong enemy 
            strongEnemyModel = Content.Load<Model>("Models\\H8SHIPscaled");
            strongEnemyModelTransforms = SetupEffectTransformDefaults(strongEnemyModel);
            // load basic enemy bullet
            basicEnemyWeaponBulletmodel = Content.Load<Model>("Models\\massiveorb");
            basicEnemyWeaponbulletTransforms = SetupEffectTransformDefaults(basicEnemyWeaponBulletmodel);
            // load strong enemy bullet
            strongEnemyWeaponBulletmodel = Content.Load<Model>("Models\\massiveorb");
            strongEnemyWeaponbulletTransforms = SetupEffectTransformDefaults(strongEnemyWeaponBulletmodel);
            // Load layer1 and 2 content
            bgLayer1.Initialize(Content, "bgLayer1", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "bgLayer2", GraphicsDevice.Viewport.Width, -2);
            // score Font 
            scoreFont = Content.Load<SpriteFont>("scoreFont");
            // explosion texture
            explosionTexture = Content.Load<Texture2D>("explosion");
            //Create background texture
            mainBackground = Content.Load<Texture2D>("sky-stars-background");
            
            // load sound effects for explosions and crashes
            CrashSounds[0] = Content.Load<SoundEffect>("sounds/explosion");
            CrashSounds[1] = Content.Load<SoundEffect>("sounds/explosion1");
            CrashSounds[2] = Content.Load<SoundEffect>("sounds/110115__ryansnook__small-explosion");
            CrashSounds[3] = Content.Load<SoundEffect>("sounds/137040__mateusboga__explosion1");
            CrashSounds[4] = Content.Load<SoundEffect>("sounds/explosion1");

            // load sound effects for lasers/bullets 
            LaserSounds[0] = Content.Load<SoundEffect>("sounds/151022__bubaproducer__laser-shot-silenced");
            LaserSounds[1] = Content.Load<SoundEffect>("sounds/42106__marcuslee__laser-wrath-4");
            LaserSounds[2] = Content.Load<SoundEffect>("sounds/laserFire");

            // background music
            backgroundSong = Content.Load<Song>("Audio/DRIVE for the SEGA MASTER SYSTEM_GENESIS");
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;



        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            // counts the time since the start of the game
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // spaceStation exit code
            if (spaceStationHealth == 0)
            {
                Exit();
            }

            if (player.lives == 0)
            {
                Exit();
            }


            
            // if music is false 
            if (musicToggle == false)
            {
                // mute music 
                MediaPlayer.IsMuted = true;
            }
            // if music is true 
            else if (musicToggle == true)
            {
                // un-mute music 
                MediaPlayer.IsMuted = false;
            }

            // toggle audio 
            toggleAudio();


            // player bullet timer update
            playerBulletTimer(gameTime);

            // basic enemies timer update
            spawnBasicEnemiesTimer(gameTime);

            //fast enemies timer update
            spawnFastEnemiesTimer(gameTime);

            // strong enemies timer update 
            spawnFastEnemiesTimer(gameTime);
   
            // basic enemy bullet timer 
            spawnBasicEnemyBulletsTimer(gameTime);

            // strong enemy bullet timer 
            spawnStrongEnemiesTimer(gameTime);

            // Update the parallaxing background
            bgLayer1.Update();
            bgLayer2.Update();

            // Clamps the player within the window (X coordinate)
            player.playerPosition.X = MathHelper.Clamp(player.playerPosition.X, -2700.0f, 2950.0f - player.playerBox.Radius);
            // Clamps the player within the window(Y coordinate)
            player.playerPosition.Y = MathHelper.Clamp(player.playerPosition.Y, -1950.0f, 2000.0f - player.playerBox.Radius);
            
            // update player
            player.Update();

            // update explosions 
            UpdateExplosions(gameTime);

            // shoot bullet code
            shootbullets();
            // gamepad controller bullet code 
            gamePadShootBullets();

            // shoot enemy bullets 
            spawnBasicEnemybullets();
            // shoot enemy bullets 
            spawnStrongEnemybullets();

            // bullet list update
            for(int i = 0; i < GameConstants.NumBasicBullets; i++)
            {
                if(basicWeaponBulletList[i].isActive)
                {
                    basicWeaponBulletList[i].Update(timeDelta);
                }
            }

            // spawn basic enemies 
            spawnBasicEnemies();
            // basic enemies update list 
            for (int i = 0; i < GameConstants.basicEnemyNumbers; i++)
            {
                if (basicEnemyList[i].isActive)
                {
                    basicEnemyList[i].Update(timeDelta);
                }

            }

            spawnFastEnemies();
            // fast enemies update list 
            for (int i = 0; i < GameConstants.fastEnemyNumbers; i++)
            {
                if (fastEnemyList[i].isActive)
                {
                    fastEnemyList[i].Update(timeDelta);
                }

            }

            spawnStrongEnemies();
            // fast enemies update list 
            for (int i = 0; i < GameConstants.strongEnemyNumbers; i++)
            {
                if (strongEnemyList[i].isActive)
                {
                    strongEnemyList[i].Update(timeDelta);
                }

            }
            // enemy bullets 
            for (int i = 0; i < GameConstants.NumBasicEnemyBullets; i++)
            {
                if (basicEnemyWeaponBulletList[i].isActive)
                {
                    basicEnemyWeaponBulletList[i].Update(timeDelta);
                }

            }

            // strong enemy bullets 
            for (int i = 0; i < GameConstants.NumBasicEnemyBullets; i++)
            {
                if (strongEnemyWeaponBulletList[i].isActive)
                {
                    strongEnemyWeaponBulletList[i].Update(timeDelta);
                }

            }


            // update player
            player.Update();

            // collision for basic enemies 
            for (int i = 0; i < basicEnemyList.Length; i++)
            {
                if (basicEnemyList[i].isActive)
                {
                    // create bounding 
                    BoundingSphere basicEnemyBounding = new BoundingSphere(basicEnemyList[i].basicEnemyposition,
                        basicEnemyModel.Meshes[0].BoundingSphere.Radius * GameConstants.basicEnemyBoundingSphereScale);
                    
                    // if player passed left hand side of screen 
                    if (basicEnemyList[i].basicEnemyposition.X < -3300 ||
                        basicEnemyList[i].basicEnemyposition.Z > GameConstants.PlayfieldSizeZ ||
                        basicEnemyList[i].basicEnemyposition.Z < -GameConstants.PlayfieldSizeZ)
                    {
                        // space station health decreases 
                        spaceStationHealth--;
                    }

                    // for each player bullet 
                    for (int K = 0; K < basicWeaponBulletList.Length; K++)
                    {
                        // if bullet is active 
                        if (basicWeaponBulletList[K].isActive)
                        {
                            // create bounding for bullet 
                            BoundingSphere basicWeaponBounding = new BoundingSphere(basicWeaponBulletList[K].basicWeaponBulletposition,
                        basicWeaponBulletModel.Meshes[0].BoundingSphere.Radius * GameConstants.basicWeaponBoundingSphereScale);

                            // is enemy intersects weapon bounding 
                            if (basicEnemyBounding.Intersects(basicWeaponBounding))
                            {
                                // enemy health decreases 
                                basicEnemyList[i].health--;
                                // if enemy health = 0 
                                if (basicEnemyList[i].health == 0)
                                {
                                    // play random sound effects 
                                    int crashIndex = random.Next(0, 4);
                                    CrashSounds[crashIndex].Play();
                                    //AddExplosion(new Vector2(basicEnemyList[i].basicEnemyposition.X,basicEnemyList[i].basicEnemyposition.Y));
                                }
                                // bullet is no longer active 
                                basicWeaponBulletList[K].isActive = false;
                                hitCount++;
                                // increase score 
                                score++;
                                break;
                            }
                        }

                        // if enemy intersects player 
                        if (basicEnemyBounding.Intersects(player.playerBox))
                        {
                            //AddExplosion(new Vector2(basicEnemyList[i].basicEnemyposition.X,basicEnemyList[i].basicEnemyposition.Y));
                            
                            // play random sound 
                            int crashIndex = random.Next(0,4);
                            CrashSounds[crashIndex].Play();
                            // enemy is set to false 
                            basicEnemyList[i].isActive = false;
                            // player health decreases 
                            player.Health--;
                            hitCount++;
                            
                            break;
                        }

                    }

                }
         }

            for (int i = 0; i < basicEnemyWeaponBulletList.Length; i++)
            {
                if (basicEnemyWeaponBulletList[i].isActive)
                {
                    BoundingSphere basicEnemyBulletBounding = new BoundingSphere(basicEnemyWeaponBulletList[i].basicEnemyWeaponBulletposition,
                        basicEnemyWeaponBulletmodel.Meshes[0].BoundingSphere.Radius * GameConstants.basicEnemyBoundingSphereScale);

                    if (basicEnemyBulletBounding.Intersects(player.playerBox))
                    {
                        basicEnemyWeaponBulletList[i].isActive = false;
                        player.Health--;
                        // random sound plays 
                        int crashIndex = random.Next(0, 4);
                        CrashSounds[crashIndex].Play();
                        break;
                    }

                }

            }          
                    
               



            // for each fast enemy 
            for (int i = 0; i < fastEnemyList.Length; i++)
            {
                // if enemy is active 
                if (fastEnemyList[i].isActive)
                {
                    // create bounding 
                    BoundingSphere fastEnemyBounding = new BoundingSphere(fastEnemyList[i].fastEnemyposition,
                        fastEnemyModel.Meshes[0].BoundingSphere.Radius * GameConstants.fastEnemyBoundingSphereScale);

                    // for each bullet 
                    for (int K = 0; K < basicWeaponBulletList.Length; K++)
                    {
                        // if bullet is active 
                        if (basicWeaponBulletList[K].isActive)
                        {
                            // create bounding 
                            BoundingSphere basicWeaponBounding = new BoundingSphere(basicWeaponBulletList[K].basicWeaponBulletposition,
                        basicWeaponBulletModel.Meshes[0].BoundingSphere.Radius * GameConstants.basicWeaponBoundingSphereScale);

                            // if fast enemy gets hit by bullet 
                            if (fastEnemyBounding.Intersects(basicWeaponBounding))
                            {
                                // health decreases 
                                fastEnemyList[i].health--;
                                // bullet is destroyed 
                                basicWeaponBulletList[K].isActive = false;
                                // random sound plays 
                                int crashIndex = random.Next(0, 4);
                                CrashSounds[crashIndex].Play();
                                hitCount++;
                                // score decreases 
                                score--;
                                break;
                            }
                        }
                        // if fast enemy hits player 
                        if (fastEnemyBounding.Intersects(player.playerBox))
                        {
                            // health decreases 
                            player.Health--;
                            // score decreases 
                            score--;
                            // random sound plays 
                            int crashIndex = random.Next(0, 4);
                            CrashSounds[crashIndex].Play();
                            // enemy is destroyed 
                            fastEnemyList[i].isActive = false;
                            hitCount++;
                            
                            break;
                        }

                    }

                }
            }

            // strong enemy collision
            for (int i = 0; i < strongEnemyList.Length; i++)
            {
                // if strong enemy is active 
                if (strongEnemyList[i].isActive)
                {
                    // create bounding 
                    BoundingSphere strongEnemyBounding = new BoundingSphere(strongEnemyList[i].strongEnemyposition,
                        strongEnemyModel.Meshes[0].BoundingSphere.Radius * GameConstants.strongEnemyBoundingSphereScale);

                    // strong enemy passed left of screen 
                    if (strongEnemyList[i].strongEnemyposition.X < -3300 ||
                        strongEnemyList[i].strongEnemyposition.Z > GameConstants.PlayfieldSizeZ ||
                        strongEnemyList[i].strongEnemyposition.Z < -GameConstants.PlayfieldSizeZ)
                    {
                        // space station health decreases 
                        spaceStationHealth--;
                    }

                    // create bullet bounding 
                    for (int K = 0; K < basicWeaponBulletList.Length; K++)
                    {
                        if (basicWeaponBulletList[K].isActive)
                        {
                            BoundingSphere basicWeaponBounding = new BoundingSphere(basicWeaponBulletList[K].basicWeaponBulletposition,
                        basicWeaponBulletModel.Meshes[0].BoundingSphere.Radius * GameConstants.basicWeaponBoundingSphereScale);

                            if (strongEnemyBounding.Intersects(basicWeaponBounding))
                            {
                                strongEnemyList[i].health--;
                                basicWeaponBulletList[K].isActive = false;
                                int crashIndex = random.Next(0, 4);
                                CrashSounds[crashIndex].Play();
                                hitCount++;
                                score++;
                                break;
                            }
                        }
                        // if strong enemy hits player 
                        if (strongEnemyBounding.Intersects(player.playerBox))
                        {
                            player.Health--;
                            int crashIndex = random.Next(0, 4);
                            CrashSounds[crashIndex].Play();
                            strongEnemyList[i].isActive = false;
                            hitCount++;
                            Debug.WriteLine("enemy hit");
                            break;
                        }

                    }

                }
            }

            

                // update game
             base.Update(gameTime);
        }

        // updates the explosions 
        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        // timer for player bullets 
        public void playerBulletTimer(GameTime gameTime)
        {
            // set current time to count seconds 
            playerReloadTime.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update()
            
            // reload counter
            if (playerReloadTime.time > 0)
            {
                // if current time is more than count duration 
                if (playerReloadTime.currentTime >= playerReloadTime.countDuration)
                {
                    // counter increases 
                    playerReloadTime.counter++;
                    playerReloadTime.currentTime -= playerReloadTime.countDuration;
                    Debug.WriteLine("In first counter");
                }
                // if counter is over the spawn time 
                if (playerReloadTime.counter >= playerReloadTime.time)
                {
                    // reset counter 
                    playerReloadTime.counter = 0;
                    // reset timer 
                    playerReloadTime.time = 0;
                    Debug.WriteLine("Reload reset");
                }
            }
        }

        // timer for spawning basic enemies 
        public void spawnBasicEnemiesTimer(GameTime gameTime)
        {
            // set timer for reload
            enemySpawnTime.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update()
            // reload counter
            if (enemySpawnTime.time > 0)
            {
                if (enemySpawnTime.currentTime >= enemySpawnTime.countDuration)
                {
                    enemySpawnTime.counter++;
                    enemySpawnTime.currentTime -= enemySpawnTime.countDuration;
                    Debug.WriteLine("In first  enemy counter");
                }
                if (enemySpawnTime.counter >= enemySpawnTime.time)
                {
                    enemySpawnTime.counter = 0;
                    enemySpawnTime.time = 0;
                    Debug.WriteLine("enemy spawned reset");
                }
            }
        }
        
        // spawn basic enemy bullets 
        public void spawnBasicEnemyBulletsTimer(GameTime gameTime)
        {
            // set timer for enemy bullets 
            basicEnemyBulletTimer.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update()
            // reload counter
            if (basicEnemyBulletTimer.time > 0)
            {
                if (basicEnemyBulletTimer.currentTime >= basicEnemyBulletTimer.countDuration)
                {
                    basicEnemyBulletTimer.counter++;
                    basicEnemyBulletTimer.currentTime -= basicEnemyBulletTimer.countDuration;
                    Debug.WriteLine("In enemy bullet counter");
                }
                if (basicEnemyBulletTimer.counter >= basicEnemyBulletTimer.time)
                {
                    basicEnemyBulletTimer.counter = 0;
                    basicEnemyBulletTimer.time = 0;
                    Debug.WriteLine("enemy bullet reset");
                }
            }
        }

        // spawn fast enemies 
        public void spawnFastEnemiesTimer(GameTime gameTime)
        {
            // set timer for fastenemy spawn
            fastEnemySpawnTime.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update()
            // reload counter
            if (fastEnemySpawnTime.time > 0)
            {
                if (fastEnemySpawnTime.currentTime >= fastEnemySpawnTime.countDuration)
                {
                    fastEnemySpawnTime.counter++;
                    fastEnemySpawnTime.currentTime -= fastEnemySpawnTime.countDuration;
                    Debug.WriteLine("In first  enemy counter");
                }
                if (fastEnemySpawnTime.counter >= fastEnemySpawnTime.time)
                {
                    fastEnemySpawnTime.counter = 0;
                    fastEnemySpawnTime.time = 0;
                    Debug.WriteLine("enemy spawned reset");
                }
            }
        }

        // spawn strong enemeis 
        public void spawnStrongEnemiesTimer(GameTime gameTime)
        {
            // set timer for strongenemy spawn
            strongEnemySpawnTime.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update()
            // reload counter
            if (strongEnemySpawnTime.time > 0)
            {
                if (strongEnemySpawnTime.currentTime >= strongEnemySpawnTime.countDuration)
                {
                    strongEnemySpawnTime.counter++;
                    strongEnemySpawnTime.currentTime -= strongEnemySpawnTime.countDuration;
                    Debug.WriteLine("In strong  enemy counter");
                }
                if (strongEnemySpawnTime.counter >= strongEnemySpawnTime.time)
                {
                    strongEnemySpawnTime.counter = 0;
                    strongEnemySpawnTime.time = 0;
                    Debug.WriteLine("enemy strong spawned reset");
                }
            }
        }

        // spawn strong enemies 
        public void spawnStrongEnemyBulletsTimer(GameTime gameTime)
        {
            // set timer for enemy bullets 
            strongEnemyBulletTimer.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update()
            // reload counter
            if (strongEnemyBulletTimer.time > 0)
            {
                if (strongEnemyBulletTimer.currentTime >= strongEnemyBulletTimer.countDuration)
                {
                    strongEnemyBulletTimer.counter++;
                    strongEnemyBulletTimer.currentTime -= strongEnemyBulletTimer.countDuration;
                    Debug.WriteLine("In enemy bullet counter");
                }
                if (strongEnemyBulletTimer.counter >= strongEnemyBulletTimer.time)
                {
                    strongEnemyBulletTimer.counter = 0;
                    strongEnemyBulletTimer.time = 0;
                    Debug.WriteLine("enemy bullet reset");
                }
            }
        }

        // draws player bullets
        public void Drawbullets(GameTime gameTime)
        {
            // draws bullets
            for (int i = 0; i < GameConstants.NumBasicBullets; i++)
            {
                if (basicWeaponBulletList[i].isActive)
                {
                    Matrix laserTransform = Matrix.CreateTranslation(basicWeaponBulletList[i].basicWeaponBulletposition);
                    DrawModel(basicWeaponBulletModel, laserTransform, basicWeaponbulletTransforms);
                }
            }
        }

        // Draw basic Enemy Models 
        public void DrawBasicEnemies(GameTime gameTime)
        {
            // draws basic enemies 
            for (int i = 0; i < GameConstants.basicEnemyNumbers; i++)
            {
                if (basicEnemyList[i].isActive)
                {
                    Matrix basicEnemyTransform =  Matrix.CreateTranslation(basicEnemyList[i].basicEnemyposition);
                    DrawModel(basicEnemyModel, basicEnemyTransform, basicEnemyModelTransforms);
                }
            }

        }

        // Draw basic Enemy Bullets 
        public void DrawBasicEnemybullets(GameTime gameTime)
        {
            // draw basic enemy bullets 
            for (int i = 0; i < GameConstants.NumBasicEnemyBullets; i++)
            {
                if (basicEnemyWeaponBulletList[i].isActive)
                {
                    Matrix enemyBulletTransform = Matrix.CreateTranslation(basicEnemyWeaponBulletList[i].basicEnemyWeaponBulletposition);
                    DrawModel(basicEnemyWeaponBulletmodel, enemyBulletTransform, basicEnemyWeaponbulletTransforms);
                }
            }
        }

        // Draw Fast Enemy Models 
        public void DrawFastEnemies(GameTime gameTime)
        {
            // draws fast enemies 
            for (int i = 0; i < GameConstants.fastEnemyNumbers; i++)
            {
                if (fastEnemyList[i].isActive)
                {
                    Matrix fastEnemyTransform = Matrix.CreateRotationY(300.0f) * Matrix.CreateTranslation(fastEnemyList[i].fastEnemyposition);
                    DrawModel(fastEnemyModel, fastEnemyTransform, fastEnemyModelTransforms);
                }
            }

        }

        // Draw Strong Enemy Models 
        public void DrawStrongEnemies(GameTime gameTime)
        {
            // draws strong enemies 
            for (int i = 0; i < GameConstants.strongEnemyNumbers; i++)
            {
                if (strongEnemyList[i].isActive)
                {
                    Matrix strongEnemyTransform = Matrix.CreateTranslation(strongEnemyList[i].strongEnemyposition);
                    DrawModel(strongEnemyModel, strongEnemyTransform, strongEnemyModelTransforms);
                }
            }
        }

        // Draw basic Enemy Bullets 
        public void DrawStrongEnemybullets(GameTime gameTime)
        {
            // draw basic enemy bullets 
            for (int i = 0; i < GameConstants.NumBasicEnemyBullets; i++)
            {
                if (strongEnemyWeaponBulletList[i].isActive)
                {
                    Matrix enemyBulletTransform = Matrix.CreateTranslation(basicEnemyWeaponBulletList[i].basicEnemyWeaponBulletposition);
                    DrawModel(strongEnemyWeaponBulletmodel, enemyBulletTransform, strongEnemyWeaponbulletTransforms);
                }
            }
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Begin Sprite drawing
            spriteBatch.Begin();
            // Draw Background 
            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
            // Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
                Debug.WriteLine("Explosion Drawn");
            }

            //End Sprite drawing
            spriteBatch.End();
            // stencil depth drawer
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // draw player bullets 
            Drawbullets(gameTime);

            // Draws the basic Enemies 
            DrawBasicEnemies(gameTime);

            // draw basic enemy bullets
            DrawBasicEnemybullets(gameTime);

            //Draws fast enemies
            DrawFastEnemies(gameTime);
          
            // draws strong enemies 
            DrawStrongEnemies(gameTime);

            // draw basic enemy bullets
            DrawStrongEnemybullets(gameTime);

            // draws player and transforms
            Matrix modelTransform = Matrix.CreateRotationY(player.modelRotation) * Matrix.CreateTranslation(player.playerPosition);
            DrawModel(player.playerModel, modelTransform, player.playerTransforms);

            // Draws the text for the Score
            writeText("Score:" + score.ToString()  , new Vector2(50,10), Color.Yellow);

            // Draws the text for the players health 
            writeText("Player Health/Lives:" + player.Health.ToString() + "/" + player.lives.ToString() , new Vector2(200, 10), Color.Yellow);

            // Draws the text for the space stations health
            writeText("Space Station Health:" + spaceStationHealth.ToString(), new Vector2(500, 10), Color.Yellow);

            // draw
            base.Draw(gameTime);
            
        }
        }
    
    }
