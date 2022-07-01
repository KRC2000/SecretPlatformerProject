using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace Project
{
    class Bone2D
    {
        public Bone2D ParentBone { get; private set; }
        public List<Bone2D> ChildBones { get; private set; }
        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public Vector2 Vector { get; private set; }
        public float Lenght { get; private set; }
        public string Name { get; private set; }

        public Bone2D(string name, Vector2 position, float rotation, float lenght, Bone2D parentBone = null)
        {
            Name = name; Position = position; Rotation = rotation; Lenght = lenght;
            ParentBone = parentBone;
            UpdateVector();
        }

        private void UpdateVector()
        {
            Vector = new Vector2(Lenght, 0).Rotate(MathHelper.ToRadians(Rotation));
        }

        public void Draw(SpriteBatch batch)
        {
            batch.DrawLine(Position, Position + Vector, Color.BlueViolet, 3, 0);
        }
    }
}