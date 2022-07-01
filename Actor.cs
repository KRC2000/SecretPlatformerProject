using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace Project
{
    public abstract class Actor
    {
        public bool IsAirborne = true;
        public Vector2 Velocity = new Vector2();
        public Transform2 Transform = new Transform2();

        //public Texture2D Texture { get; protected set; }

        public Point Size { get; protected set; }

        public Vector2 GroundUpperSensor { get; protected set; } = new Vector2();
        public Vector2 GroundLowerSensor { get; protected set; } = new Vector2();
        public Vector2 LeftSensor { get; protected set; } = new Vector2();
        public Vector2 RightSensor { get; protected set; } = new Vector2();
        public bool ShouldFrictionBeApplyed = true;

        public virtual void Update(GameTime gameTime)
        {
        }

        public abstract void Draw(SpriteBatch batch, OrthographicCamera camera);

        public void UpdatePixelSensorsPos()
        {
            GroundUpperSensor = Transform.Position + new Vector2(Size.X / 2, Size.Y - 1);
            GroundLowerSensor = Transform.Position + new Vector2(Size.X / 2, Size.Y - 1) + new Vector2(0, 1);
            RightSensor = Transform.Position + new Vector2(Size.X, Size.Y - 10);
            LeftSensor = Transform.Position + new Vector2(0, Size.Y - 10);
        }
    }
}