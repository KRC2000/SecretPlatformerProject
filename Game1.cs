using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Input;



namespace Project
{
    public class Game1 : Game
    {
        private static SpriteFont DefaultFont;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;


        private Map map = new Map();

        private Player pl;
        private Creature z;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //SetResolution(800, 600);
            //SetFrameLimit(5);
            //SetFrameLimit(15);

            //SetFrameLimit(30);
            //SetFrameLimit(60);
            //SetFrameLimit(140);

            SetFrameLimit(0);

            _camera = new OrthographicCamera(GraphicsDevice);
            _camera.Zoom = 3;

            pl = new Player(_camera);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DebugUI.Font = Content.Load<SpriteFont>("Font1");
            DefaultFont = Content.Load<SpriteFont>("Font1");

            z = new Zombie();

            pl.LoadContent(Content);
            z.LoadContent(Content);

            map.AddChunk(Content.Load<Texture2D>("map"), new Point(0, 0));
            map.AddChunk(Content.Load<Texture2D>("map1"), new Point(800, 0));
            map.AddChunk(Content.Load<Texture2D>("map2"), new Point(1600, 0));

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Refresh();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            


            pl.Update(gameTime);
            z.Update(gameTime);
            map.Update();

            InteractionManager.ActorMap_collsion(pl, map, gameTime);
            InteractionManager.ActorMap_collsion(z, map, gameTime);

            _camera.LookAt(pl.Transform.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            map.Draw(_spriteBatch, _camera);
            pl.Draw(_spriteBatch, _camera);
            z.Draw(_spriteBatch, _camera);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void SetResolution(int width, int height)
		{
			_graphics.PreferredBackBufferWidth = width;
			_graphics.PreferredBackBufferHeight = height;
			_graphics.ApplyChanges();
		}
        protected void SetFrameLimit(int targetFps)
		{
			if (targetFps != 0)
			{
				_graphics.SynchronizeWithVerticalRetrace = true;
				IsFixedTimeStep = true;
				TargetElapsedTime = TimeSpan.FromSeconds(1d / (double)targetFps);
			}
			else
			{
				_graphics.SynchronizeWithVerticalRetrace = false;
				IsFixedTimeStep = false;
			}
			_graphics.ApplyChanges();
		}

		protected void SetFullScreen(bool value)
		{
			_graphics.IsFullScreen = value;
			_graphics.ApplyChanges();
		}
    }
}
