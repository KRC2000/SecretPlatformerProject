using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace Project
{
    public class Zombie : Creature, IMobile
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
        {
            skel_main.Position = Transform.Position + new Vector2((float)Size.X/2, Size.Y - bone_legs.Lenght);

            if (input_TravelLeft) WalkLeft(gameTime);
            if (input_TravelRight) WalkRight(gameTime);

            UpdateInputStatus();
            UpdateFrictionApplianceStatus(gameTime);
        }
        public override void Draw(SpriteBatch batch, OrthographicCamera camera)
        {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());
            
            skel_main.Draw(batch);

            batch.End();

        }

        public void GoLeft()
        {
            input_TravelLeft = true;
        }

        public void GoRight()
        {
            input_TravelRight = true;
        }

        public void StopMoving()
        {
            input_TravelLeft = false;
            input_TravelRight = false;
        }

        private bool input_TravelLeft, input_TravelRight = true;

        private Skeleton2D skel_main;
        private Bone2D bone_torso, bone_head, bone_hands, bone_legs, bone_neck;
        private FlipBook fb_legs, fb_torso, fb_head, fb_hands;
        private Animation anim_legs_walk, anim_torso_idle, anim_hands_attack;

        private void WalkRight(GameTime gameTime)
        {
            if (Velocity.X * gameTime.GetElapsedSeconds() < MaxMovementSpeed * gameTime.GetElapsedSeconds())
            {
                Velocity.X += WalkAcceleration * gameTime.GetElapsedSeconds();

                // cut off possible speed overflow made in previous line
                if (Velocity.X * gameTime.GetElapsedSeconds() > MaxMovementSpeed * gameTime.GetElapsedSeconds())
                    Velocity.X = MaxMovementSpeed;
            }
        }

        private void WalkLeft(GameTime gameTime)
        {
            if (Velocity.X * gameTime.GetElapsedSeconds() > -MaxMovementSpeed * gameTime.GetElapsedSeconds())
            {
                Velocity.X -= WalkAcceleration * gameTime.GetElapsedSeconds();
                
                // cut off possible speed overflow made in previous line
                if (Velocity.X * gameTime.GetElapsedSeconds() < -MaxMovementSpeed * gameTime.GetElapsedSeconds())
                    Velocity.X = -MaxMovementSpeed;
            }
        }

        private void UpdateInputStatus()
        {
            if (input_TravelLeft || input_TravelRight) IsReceivingInput = true;
            else IsReceivingInput = false;
        }

        private void UpdateFrictionApplianceStatus(GameTime gameTime)
        {
            if (IsReceivingInput && Math.Abs(Velocity.X) * gameTime.GetElapsedSeconds() <= MaxMovementSpeed * gameTime.GetElapsedSeconds())
            {
                ShouldFrictionBeApplyed = false;
            }
            else
            {
                ShouldFrictionBeApplyed = true;
            }
        }
    }
}