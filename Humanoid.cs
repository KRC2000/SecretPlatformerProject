using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGame.Extended;

namespace Project
{
    abstract class Humanoid : Actor
    {
        public float MaxMovementSpeed { get; private set; } = 50;
        public float WalkAcceleration { get; private set; } = 400;
        public Vector2 LookVector { get; private set; }
        public float LookAngle { get; private set; }

        public abstract void LoadContent(ContentManager content);
        public abstract void Draw(SpriteBatch batch, OrthographicCamera camera);
    }
}