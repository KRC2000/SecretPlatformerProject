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

        Skeleton2D skeleton = new Skeleton2D();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Vector2 vec = new Vector2(3, 0);
            Vector2 vec_rot = vec.Rotate(45f *  (float)Math.PI / 180f);
            Point p = vec_rot.ToPoint();
            // Vector2 vec_rot = new Vector2();;
            // vec_rot.X = Math.Cos(90 * PI/180);
            // vec_rot.Y = Math.Sin(90 * PI/180);
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
            _camera.LookAt(new Vector2(0, 0));
            pl = new Player();


            skeleton.AddBone("bone1", new Vector2(), -45, 20);
            skeleton.AddBone("bone2", "bone1", new Vector2(20, 0), 0, 20);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DebugUI.Font = Content.Load<SpriteFont>("Font1");
            DefaultFont = Content.Load<SpriteFont>("Font1");

            pl.LoadContent(Content);

            map.AddChunk(Content.Load<Texture2D>("map"), new Point(0, 0));
            map.AddChunk(Content.Load<Texture2D>("map1"), new Point(800, 0));
            map.AddChunk(Content.Load<Texture2D>("map2"), new Point(1600, 0));

            //anim.PlayFromStart();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Refresh();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            pl.Update(gameTime);
            map.Update();
            InteractionManager.ActorMap_collsion(pl, map, gameTime);

            //_camera.LookAt(pl.Transform.Position);
            //_camera.ZoomOut(0.01f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            map.Draw(_spriteBatch, _camera);
            pl.Draw(_spriteBatch, _camera);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            
            skeleton.Draw(_spriteBatch, DefaultFont);

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
