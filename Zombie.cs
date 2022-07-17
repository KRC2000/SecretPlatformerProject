using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGame.Extended;

namespace Project
{
    public class Zombie : Creature
    {
        public Vector2 LookUnitVector { get; private set; }
        public float LookAngle { get; private set; }

        public override void LoadContent(ContentManager content)
        {
            Size = new Point(32, 50);
            skel_main = new Skeleton2D();
            
            skel_main.AddBone("torso", new Vector2(), -90, 16);
            skel_main.AddBone("neck", "torso", -90, 2);
            skel_main.AddBone("head", "neck", -90, 7);
            skel_main.AddBone("hands", "torso", 0, 10);
            skel_main.AddBone("legs", new Vector2(), 90, 17);

            bone_torso = skel_main.GetBoneByName("torso");
            bone_neck = skel_main.GetBoneByName("neck");
            bone_head = skel_main.GetBoneByName("head");
            bone_hands = skel_main.GetBoneByName("hands");
            bone_legs = skel_main.GetBoneByName("legs");

            fb_head = new FlipBook(content.Load<Texture2D>("head"), new Point(32, 32), new Vector2(32 / 2, 20));
            fb_torso = new FlipBook(content.Load<Texture2D>("torso_idle"), new Point(32, 32), new Vector2(32 / 2, 25));
            fb_legs = new FlipBook(content.Load<Texture2D>("legs_walk"), new Point(32, 32), new Vector2(32 / 2 , 8));
            fb_hands = new FlipBook(content.Load<Texture2D>("hand_left"), new Point(32, 32), new Vector2(32 / 2 - 2, 32 / 2));
        } 
        public override void Update(GameTime gameTime)
        {}
        public override void Draw(SpriteBatch batch, OrthographicCamera camera)
        {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());
            
            skel_main.Draw(batch);
            batch.End();

        }

        private Skeleton2D skel_main;
        private Bone2D bone_torso, bone_head, bone_hands, bone_legs, bone_neck;
        private FlipBook fb_legs, fb_torso, fb_head, fb_hands;
        private Animation anim_legs_walk, anim_torso_idle, anim_hands_attack;
    }
}