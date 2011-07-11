using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using N00bGame.Components;

namespace N00bGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class N00bGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Components.GridComponent grid;
        Components.CameraComponent camera;

        public N00bGame()
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
            // TODO: Add your initialization logic here
            grid = new Components.GridComponent(this);
            grid.MajorColor = Color.Red;

            camera = new Components.CameraComponent(this);

            this.Components.Add(grid);
            this.Components.Add(camera);
            Mouse.SetPosition(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();
            currentMouseState = Mouse.GetState();
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
        /// 
        MouseState oldMouseState;
        MouseState currentMouseState;

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float amount = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.005f;
            currentMouseState = Mouse.GetState();
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                camera.Move(Actions.MoveForward);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                camera.Move(Actions.MoveBackward);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                camera.Rotate(0f, 0f, amount * 10);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                camera.Rotate(0f, 0f, -amount * 10);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                camera.Move(Actions.StrafeLeft);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camera.Move(Actions.StrafeRight);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                camera.Move(Actions.TurnLeft);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                camera.Move(Actions.TurnRight);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                camera.Move(Actions.LookUp);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                camera.Move(Actions.LookDown);
            }

            Vector2 mouseDelta = new Vector2(currentMouseState.X - oldMouseState.X, currentMouseState.Y - oldMouseState.Y);
            camera.Rotate(-mouseDelta.X * amount, -mouseDelta.Y * amount, 0.0f);
            //oldMouseState = currentMouseState;
            Mouse.SetPosition(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();

            grid.World = Matrix.Identity;
            grid.View = camera.View;
            grid.Projection = camera.Projection;
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
