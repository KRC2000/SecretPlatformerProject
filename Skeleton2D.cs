using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;


namespace Project
{
    class Skeleton2D
    {
        public List<Bone2D> Bones { get; private set; }

        public Vector2 Position { get; set; }

        public Skeleton2D()
        {
            Bones = new List<Bone2D>();
        }

        public void AddBone(string name, Vector2 pos, float rot, float lenght)
        {
            Bones.Add(new Bone2D(name, pos, rot, lenght));
        }

        public void AddBone(string name, string parentBoneName, float rot, float lenght)
        {
            Bone2D parentBone = GetBoneByName(parentBoneName);
            Bone2D newBone = new Bone2D(name, parentBone.LocalPosition + parentBone.Vector, rot, lenght, parentBone);
            Bones.Add(newBone);
        }

        public void RotateBone(string boneName, float degrees)
        {
            Bone2D targetBone = GetBoneByName(boneName);
            if (targetBone != null) targetBone.Rotate(degrees);
        }

        public void Draw(SpriteBatch batch, SpriteFont font = null)
        {
            foreach (Bone2D bone in Bones)
            {
                batch.DrawLine(Position + bone.LocalPosition, Position + bone.LocalPosition + bone.Vector, Color.BlueViolet, 1, 0);
                if (bone.ParentBone == null) batch.DrawCircle(new CircleF(Position + bone.LocalPosition, 4), 4, Color.Blue);
                else batch.DrawCircle(new CircleF(Position + bone.LocalPosition, 4), 10, Color.AliceBlue);
                if (font != null) batch.DrawString(font, bone.Name, Position + bone.LocalPosition + bone.Vector / 2, Color.Black, 0f, new Vector2(), 0.5f, SpriteEffects.None, 0);
            }
        }

        public Bone2D GetBoneByName(string name)
        {
            foreach (Bone2D bone in Bones)
            {
                if (bone.Name == name) return bone;
            }

            return null;
        }
    }
}