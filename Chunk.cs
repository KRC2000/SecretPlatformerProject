using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Project
{
    public class Chunk
    {
        public Point Position;
        public Texture2D CollisionMapTexture { get; private set; }
        public Color[] CollisionMap { get; set; }

        public Chunk(Texture2D texture, Point position)
        {
            Position = position;
            CollisionMapTexture = texture;
            CollisionMap = new Color[CollisionMapTexture.Width * CollisionMapTexture.Height];
            CollisionMapTexture.GetData<Color>(CollisionMap);

            Console.WriteLine($"Chunk {this.CollisionMapTexture.Name} loaded at position {position}");
        }

        public Color GetColor(Point position)
        {
            return CollisionMap[position.Y * CollisionMapTexture.Width + position.X];
        }
    }
}