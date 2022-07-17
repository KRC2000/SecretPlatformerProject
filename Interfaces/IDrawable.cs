using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace Project
{
    interface IDrawable
    {
        void Draw(SpriteBatch batch, OrthographicCamera camera);
    }
}