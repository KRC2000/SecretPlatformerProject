using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;


namespace Project
{
    class Skeleton2D
    {
        public List<Bone2D> Bones { get; private set; }

        public Skeleton2D()
        {
            Bones = new List<Bone2D>();
        }

        public void AddBone(string name, Vector2 pos, float rot, float lenght)
        {
            Bones.Add(new Bone2D(name, pos, rot, lenght));
        }

        public void AddBone(string name, string parentBoneName, Vector2 pos, float rot, float lenght)
        {
            Bones.Add(new Bone2D(name, pos, rot, lenght, FindBoneByName(parentBoneName)));
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            foreach (Bone2D bone in Bones)
            {
                if (bone.ParentBone == null) batch.DrawCircle(new CircleF(bone.Position, 4), 4, Color.Blue);
                else batch.DrawCircle(new CircleF(bone.Position, 4), 10, Color.AliceBlue);
                batch.DrawLine(bone.Position, bone.Position + bone.Vector, Color.BlueViolet, 3, 0);
                batch.DrawString(font, bone.Name, bone.Position, Color.Black, 0f, new Vector2(), 0.5f, SpriteEffects.None, 0);
            }
        }

        private Bone2D FindBoneByName(string name)
        {
            foreach (Bone2D bone in Bones)
            {
                if (bone.Name == name) return bone;
            }

            return null;
        }
    }
}