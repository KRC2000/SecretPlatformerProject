using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace Project
{
    public class Player : Creature
    {
        public Vector2 LookUnitVector { get; private set; }
        public float LookAngle { get; private set; }
        
        public Player(OrthographicCamera playerSceneCamera)
        {
            SceneCamera = playerSceneCamera;
        }

        public override void LoadContent(ContentManager content)
        {
            Size = new Point(32, 50);
            skel_main = new Skeleton2D();
            
            skel_main.AddBone("torso", new Vector2(), -90, 16);
            skel_main.AddBone("neck", "torso", -90, 2);
            skel_main.AddBone("head", "neck", -90, 7);
            skel_main.AddBone("hands", "torso", 0, 10);
            skel_main.AddBone("legs", new Vector2(), 90, 17);
            skel_main.AddBone("gun", "hands", 0, 5, new Vector2(0, -20));
            bone_torso = skel_main.GetBoneByName("torso");
            bone_neck = skel_main.GetBoneByName("neck");
            bone_head = skel_main.GetBoneByName("head");
            bone_hands = skel_main.GetBoneByName("hands");
            bone_legs = skel_main.GetBoneByName("legs");
            bone_gun = skel_main.GetBoneByName("gun");


            fb_head = new FlipBook(content.Load<Texture2D>("head"), new Point(32, 32), new Vector2(32 / 2, 20));
            fb_torso = new FlipBook(content.Load<Texture2D>("torso_idle"), new Point(32, 32), new Vector2(32 / 2, 25));
            fb_legs = new FlipBook(content.Load<Texture2D>("legs_walk"), new Point(32, 32), new Vector2(32 / 2 , 8));
            fb_leftHand = new FlipBook(content.Load<Texture2D>("hand_left"), new Point(32, 32), new Vector2(32 / 2 - 2, 32 / 2));
            fb_rightHand = new FlipBook(content.Load<Texture2D>("hand_right"), new Point(32, 32), new Vector2(32 / 2 - 2 , 32 / 2));
            fb_pistol = new FlipBook(content.Load<Texture2D>("pistol"), new Point(32, 32), new Vector2(32 / 2 - 5, 32 / 2));
            
            anim_legs_walk = new Animation(fb_legs, 6, 10, 0);
            anim_torso_idle = new Animation(fb_torso, 2, 0.5f);


            anim_torso_idle.Play();
        }

        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
            skel_main.Position = Transform.Position + new Vector2((float)Size.X/2, Size.Y - bone_legs.Lenght);
            
            Vector2 mousePos = Vector2.Transform(Mouse.GetState().Position.ToVector2(), SceneCamera.GetInverseViewMatrix());

            LookUnitVector = Vector2.Normalize(mousePos - (skel_main.Position + bone_head.LocalPosition));
            LookAngle = MathHelper.ToDegrees((float)Math.CopySign(Math.Acos(LookUnitVector.X), LookUnitVector.Y));

            if (Math.Abs(LookAngle) < 90) skel_main.SetBoneRotation(bone_head, LookAngle-90);
            else skel_main.SetBoneRotation(bone_head, LookAngle-90 + 180);

            skel_main.SetBoneRotation(bone_hands, LookAngle);

            if (Keyboard.GetState().IsKeyDown(Keys.D)){
                WalkRight(gameTime);
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.A)){
                WalkLeft(gameTime);
            }

            UpdateInputStatus();
            UpdateFrictionApplianceStatus(gameTime);

        }
        public override void Draw(SpriteBatch batch, OrthographicCamera camera)
        {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());

            batch.Draw(fb_leftHand.Texture, skel_main.GetBoneWorldPos(bone_neck), fb_leftHand.GetSourceRectangle(), Color.White, bone_hands.GetDrawRotation(), fb_leftHand.FrameOrigin , new Vector2(1, 1), fb_leftHand.GetSpriteEffects(), 0);
            batch.Draw(fb_torso.Texture, skel_main.GetBoneWorldPos(bone_torso), fb_torso.GetSourceRectangle(), Color.White, bone_torso.GetDrawRotation(), fb_torso.FrameOrigin, new Vector2(1, 1), anim_torso_idle.FlipBook.GetSpriteEffects(), 0);
            batch.Draw(fb_pistol.Texture, skel_main.GetBoneWorldPos(bone_gun), fb_pistol.GetSourceRectangle(), Color.White, bone_gun.GetDrawRotation(), fb_pistol.FrameOrigin, new Vector2(1, 1), fb_pistol.GetSpriteEffects(), 0);
            batch.Draw(fb_legs.Texture, skel_main.GetBoneWorldPos(bone_torso), fb_legs.GetSourceRectangle(), Color.White, 0f, fb_legs.FrameOrigin, new Vector2(1, 1), fb_legs.GetSpriteEffects(), 0);
            batch.Draw(fb_head.Texture, skel_main.GetBoneWorldPos(bone_head), fb_head.GetSourceRectangle(), Color.White, bone_head.GetDrawRotation(), fb_head.FrameOrigin , new Vector2(1, 1), fb_head.GetSpriteEffects(), 0);
            batch.Draw(fb_rightHand.Texture, skel_main.GetBoneWorldPos(bone_neck), fb_rightHand.GetSourceRectangle(), Color.White, bone_hands.GetDrawRotation(), fb_rightHand.FrameOrigin , new Vector2(1, 1), fb_rightHand.GetSpriteEffects(), 0);
            batch.DrawRectangle(new RectangleF(Transform.Position.X, Transform.Position.Y, Size.X, Size.Y), new Color(100, 100, 100, 100), 1f, 0);
            skel_main.Draw(batch);

            batch.End();

            DebugUI.DrawDebug<bool>(batch, "Airborne: ", IsAirborne, 0);
            DebugUI.DrawDebug<float>(batch, "Position.X: ", Transform.Position.X, 1);
            DebugUI.DrawDebug<float>(batch, "Position.Y: ", Transform.Position.Y, 2);
            DebugUI.DrawDebug<float>(batch, "Velocity.X: ", Velocity.X, 3);
            DebugUI.DrawDebug<float>(batch, "Velocity.Y: ", Velocity.Y, 4);
            DebugUI.DrawDebug<bool>(batch, "ShouldFrictionBeApplyed: ", ShouldFrictionBeApplyed, 6);
        }

        private Skeleton2D skel_main;
        private Bone2D bone_torso, bone_head, bone_hands, bone_legs, bone_gun, bone_neck;
        private FlipBook fb_legs, fb_torso, fb_head, fb_leftHand, fb_rightHand, fb_pistol;
        private Animation anim_legs_walk, anim_torso_idle;

        private OrthographicCamera SceneCamera;

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
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A)) IsReceivingInput = true;
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
        
        private void Animate(GameTime gameTime)
        {
            anim_legs_walk.Update(gameTime);
            anim_torso_idle.Update(gameTime);

            if (Math.Abs(LookAngle) > 90) 
            {
                fb_head.FlipHorizontally = true;
                fb_torso.FlipHorizontally = true;
                fb_leftHand.FlipVertically = true;
                fb_rightHand.FlipVertically = true;
                fb_pistol.FlipVertically = true;
            }
            else 
            {
                fb_head.FlipHorizontally = false;
                fb_torso.FlipHorizontally = false;
                fb_leftHand.FlipVertically = false;
                fb_rightHand.FlipVertically = false;
                fb_pistol.FlipVertically = false;
            }

            if (!IsAirborne)
            {
                if (Math.Abs(Velocity.X) > 0.01)
                {
                    if (!anim_legs_walk.Playing) anim_legs_walk.Play();
                }
                else
                {
                    if (anim_legs_walk.Playing) anim_legs_walk.Stop();
                }
            }
            else
            {
                if (anim_legs_walk.Playing) anim_legs_walk.Pause();
            }

            if (Velocity.X > 0) anim_legs_walk.FlipBook.FlipHorizontally = false;
            else if (Velocity.X < 0) anim_legs_walk.FlipBook.FlipHorizontally = true;
        }
    }
}