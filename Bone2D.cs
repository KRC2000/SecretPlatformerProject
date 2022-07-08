using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace Project
{
    class Bone2D
    {
        public Bone2D ParentBone { get; private set; }
        public List<Bone2D> ChildBones { get; private set; } = new List<Bone2D>();
        public Vector2 LocalPosition { get; private set; }
        public Vector2 InitialLocalPosition { get; private set; }
        public float Rotation { get; private set; }
        public float InitialRotation { get; private set; }
        public Vector2 Offset { get; private set; }

        public Vector2 Vector { get; private set; }
        public float Lenght { get; private set; }
        public string Name { get; private set; }

        public Bone2D(string name, Vector2 position, float rotation, float lenght, Bone2D parentBone = null, Vector2 offset = new Vector2())
        {
            Name = name; LocalPosition = position; Rotation = rotation; Lenght = lenght;
            ParentBone = parentBone;
            InitialLocalPosition = LocalPosition;
            InitialRotation = Rotation;
            Offset = offset;

            if (parentBone != null) parentBone.ChildBones.Add(this);
            UpdateVector();
        }

        public void Rotate(float degree)
        {
            Rotation += degree;
            UpdateVector();
            UpdatePosition();

            foreach (Bone2D childBone in ChildBones)
            {
                childBone.Rotate(degree);
            }
        }

        public void SetOffset(Vector2 offset)
        {
            Offset = offset;
            UpdatePosition();

            foreach (Bone2D childBone in ChildBones)
            {
                childBone.UpdatePosition();
            }
        }

        public float GetDrawRotation()
        {
            return MathHelper.ToRadians(Rotation - InitialRotation);
        }

        private void UpdatePosition()
        {
            if (ParentBone != null)
            {
                LocalPosition = ParentBone.LocalPosition + ParentBone.Vector;
            }
            else
            {
                LocalPosition = ParentBone.LocalPosition;
            }
        }

        private void UpdateVector()
        {
            Vector = new Vector2(Lenght, 0).Rotate(MathHelper.ToRadians(Rotation));
        }

        public void Draw(SpriteBatch batch)
        {
            batch.DrawLine(LocalPosition, LocalPosition + Vector, Color.BlueViolet, 3, 0);
            batch.DrawLine(LocalPosition - Offset, LocalPosition, Color.Yellow, 1, 0);
        }
    }
}