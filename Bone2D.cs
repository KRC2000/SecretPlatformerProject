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
        public bool InheritRotation { get; set; } = true;
        public Vector2 LocalPosition { get; private set; }
        public Vector2 InitialLocalPosition { get; private set; }
        public float Rotation { get; set; }
        public float InitialRotation { get; private set; }

        public Vector2 Vector { get; private set; }
        public float Lenght { get; private set; }
        public string Name { get; private set; }

        public Bone2D(string name, Vector2 position, float rotation, float lenght, Bone2D parentBone = null)
        {
            Name = name; LocalPosition = position; Rotation = rotation; Lenght = lenght;
            ParentBone = parentBone;
            InitialLocalPosition = LocalPosition;
            InitialRotation = Rotation;
            if (parentBone != null) parentBone.ChildBones.Add(this);
            UpdateVector();
        }

        public void Rotate(float degree)
        {
            Rotation += degree;
            UpdateVector();
            if (ParentBone != null)
            {
                LocalPosition = ParentBone.LocalPosition + ParentBone.Vector; 
            }

            foreach (Bone2D childBone in ChildBones)
            {
                if (childBone.InheritRotation) childBone.Rotate(degree);
            }
        }

        public float GetDrawRotation()
        {
            return MathHelper.ToRadians(Rotation - InitialRotation);
        }

        private void UpdateVector()
        {
            Vector = new Vector2(Lenght, 0).Rotate(MathHelper.ToRadians(Rotation));
        }

        public void Draw(SpriteBatch batch)
        {
            batch.DrawLine(LocalPosition, LocalPosition + Vector, Color.BlueViolet, 3, 0);
        }
    }
}