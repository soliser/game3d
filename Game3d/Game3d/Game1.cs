using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game3d
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicObject plane, skybg; 
        Vector3 cameraPosition; //position of the camera
        Vector3 cameraTarget; //camera target
        private List<BulletObject> bulletList = new List<BulletObject>();
        private List<TargetObject> targetList = new List<TargetObject>();
        float farPlaneDistance = 2000000f; //plane distance
        //to store how far the plane model rotates
        float rotateUpDown = 0f;
        float rotate = 0f;
        int numBullets = 40;
        int numTargets = 20;
        int x = 0;//bulet we are on
        KeyboardState prevKeyState;
        Random rnd = new Random();
        int score = 0;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 900;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            Model currentModel; //Temporary storage for your new model

            Content.RootDirectory = "Content";
            currentModel = Content.Load<Model>("plane4"); //Loads your new model, point it towards your model
            plane = new GraphicObject(currentModel);
            //plane.Rotation.Z -= 1.57f;
            plane.Rotation.X = .1f;
            //plane.Rotation.Y = 3.14f;
            plane.Scale = 0.2f;
            currentModel = Content.Load<Model>("sky");
            skybg = new GraphicObject(currentModel);
            skybg.Scale = 5f;
            skybg.Translation = skybg.Translation + new Vector3(0,0,-750f);
            currentModel = Content.Load<Model>("Bullet");
            for (int i = 0; i < numBullets; i++)
            {
                BulletObject bullet = new BulletObject(currentModel);
                bulletList.Add(bullet);
            }
            currentModel = Content.Load<Model>("target");
            for (int i = 0; i < numTargets; i++)
            {
                //each target randomly placed moving in a random direction
                TargetObject target = new TargetObject(currentModel, 
                    new Vector3(rnd.Next(-60, 60), rnd.Next(-20, 90), rnd.Next(-250,-150)), 
                    new Vector3(rnd.Next(-1, 1), rnd.Next(-1, 1), rnd.Next(-1,1)));
                targetList.Add(target);
            }
            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            Window.Title = "Score : " + score;
            HandleInput(gameTime);
            
            cameraPosition = cameraTarget = plane.Translation;
            cameraPosition.Z += 50.8f;
            cameraTarget.Z -= 100f;
            cameraPosition.Y += 20f;
            cameraTarget.Y += 10f;

            foreach (BulletObject g in bulletList) g.update(gameTime, plane.Translation);
            foreach (TargetObject g in targetList) g.update(gameTime);
            CheckCollision();
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            plane.ModelDraw(device, cameraPosition, cameraTarget, farPlaneDistance);
            skybg.ModelDraw(device, cameraPosition, cameraTarget, farPlaneDistance);
            foreach (BulletObject g in bulletList)
            {
                if (g.active)
                {
                    g.ModelDraw(device, cameraPosition, cameraTarget, farPlaneDistance);
                }
            }
            foreach (TargetObject g in targetList)
            {
                if (g.active)
                {
                    g.ModelDraw(device, cameraPosition, cameraTarget, farPlaneDistance);
                }
            }
            

            base.Draw(gameTime);
        }
        private void HandleInput(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            // Check for exit.
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            float speed = 0.05f; //Dictates the speed
            float turnBoundry = .5f;// max rotation
            float turnSpeed = .001f;//rotation speed

            //pitch plane Left
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {               
                if (rotate < turnBoundry)
                {
                    rotate += turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
                    plane.Rotation.Y += turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    skybg.Rotation.Y += turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (rotate > 0.0f)
            {
                rotate -= turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
                plane.Rotation.Y -= turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                skybg.Rotation.Y -= turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            //Pitch plane right
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {                
                if (rotate > -turnBoundry)
                {
                    rotate -= turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
                    plane.Rotation.Y -= turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    skybg.Rotation.Y -= turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (rotate < 0.0f)
            {
                rotate += turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
                plane.Rotation.Y += turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                skybg.Rotation.Y += turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            //move forward
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                skybg.Rotation.X -= speed/100 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            //picth down
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (rotateUpDown > -turnBoundry/20)
                {
                    rotateUpDown -= turnSpeed;
                    plane.Rotation.X -= turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (rotateUpDown < 0.0f)
            {
                rotateUpDown += turnSpeed;
                plane.Rotation.X += turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            // picth Up
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
               
                if (rotateUpDown < turnBoundry/20)
                {
                    rotateUpDown += turnSpeed;
                    plane.Rotation.X += turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (rotateUpDown > 0.0f)
            {
                rotateUpDown -= turnSpeed;
                plane.Rotation.X -= turnSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            //fire missle
            if (currentKeyboardState.IsKeyDown(Keys.M) == true && prevKeyState.IsKeyDown(Keys.M) == false)
            {
                if (!bulletList.ElementAt(x).active)
                {
                    //set up direction vector, also used for rotation
                    Vector3 direction = new Vector3(-rotate, rotateUpDown * 15, -1);
                    bulletList.ElementAt(x).fire(direction);
                    x++;
                    x %= numBullets;
                }
            }
            //store prev state for bullet firing
            prevKeyState = currentKeyboardState;
            
        }


        private void CheckCollision()
        {
            for (int i = 0; i < numBullets; i++)
            {
                if (bulletList[i].active)
                {
                    //create shere for bullet
                    BoundingSphere sphere = new BoundingSphere(bulletList[i].Translation, 1f);
                    for (int j = 0; j < numTargets; j++)
                    {

                        if (targetList[j].active)
                        {
                            //create shere for the targets
                            BoundingSphere sphere2 = new BoundingSphere(targetList[j].Translation, 4.0f);
                            if (sphere2.Contains(sphere) != ContainmentType.Disjoint)
                            {
                                // make target and bullet colliding disappear
                                targetList[j].active = false;
                                bulletList[i].active = false;
                                //score for hitting target
                                score += 10;
                            }
                        }
                    }
                }
            }

        }
    }
}
