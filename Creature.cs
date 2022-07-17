using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGame.Extended;

namespace Project
{
    public abstract class Creature : Actor, IUpdatable, IContentOwner, IDrawable
    {
        public float MaxMovementSpeed { get; private set; } = 50;
        public float WalkAcceleration { get; private set; } = 400;

        public abstract void LoadContent(ContentManager content); 
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch batch, OrthographicCamera camera);
    }
}