using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Input;

namespace Project
{
    public class Player : Actor
    {
        public float MaxMovementSpeed { get; private set; } = 50;
        public float WalkAcceleration { get; private set; } = 400;
        //public Texture2D Texture;
        
        public Player()
        {
        }

        public void LoadContent(ContentManager content)
        {
            //Texture = content.Load<Texture2D>("character");
            Size = new Point(32, 64);

            body_skel = new Skeleton2D();
            
            body_skel.AddBone("torso", new Vector2(), -90, 18);
            body_skel.AddBone("head", "torso", -90, 7);
            body_skel.AddBone("heands", "torso", 0, 10);
            bone_torso = body_skel.GetBoneByName("torso");
            bone_head = body_skel.GetBoneByName("head");
            bone_heands = body_skel.GetBoneByName("heands");


            fb_legs = new FlipBook(content.Load<Texture2D>("legs_walk"), new Point(32, 32), 6, new Vector2());
            fb_torso = new FlipBook(content.Load<Texture2D>("torso_idle"), new Point(32, 32), 2, new Vector2(32 / 2, 25));
            anim_legs_walk = new Animation(fb_legs);
            anim_torso_idle = new Animation(fb_torso);
            anim_legs_walk.Speed = 10;
            anim_torso_idle.Speed = 0.5f;

            anim_torso_idle.Play();
        }

        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
            //body_skel.Position = Transform.Position;
            body_skel.Position = Transform.Position + new Vector2((float)Size.X/2, 47f);
            
            if (Keyboard.GetState().IsKeyDown(Keys.D)){
                WalkRight(gameTime);
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.A)){
                WalkLeft(gameTime);
            }

            UpdateFrictionApplianceStatus(gameTime);

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch batch, OrthographicCamera camera)
        {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());

            batch.Draw(fb_torso.Texture, body_skel.Position + bone_torso.LocalPosition, fb_torso.GetSourceRectangle(), Color.White, 0f, fb_torso.FrameOrigin, new Vector2(1, 1), anim_torso_idle.FlipBook.GetSpriteEffects(), 0);
            batch.Draw(fb_legs.Texture, Transform.Position + legs_offset.ToVector2(), fb_legs.GetSourceRectangle(), Color.White, 0f, new Vector2(), new Vector2(1, 1), anim_legs_walk.FlipBook.GetSpriteEffects(), 0);
            batch.DrawRectangle(new RectangleF(Transform.Position.X, Transform.Position.Y, Size.X, Size.Y), new Color(100, 100, 100, 100), 1f, 0);
            body_skel.Draw(batch);


            batch.End();

            DebugUI.DrawDebug<bool>(batch, "Airborne: ", IsAirborne, 0);
            DebugUI.DrawDebug<float>(batch, "Position.X: ", Transform.Position.X, 1);
            DebugUI.DrawDebug<float>(batch, "Position.Y: ", Transform.Position.Y, 2);
            DebugUI.DrawDebug<float>(batch, "Velocity.X: ", Velocity.X, 3);
            DebugUI.DrawDebug<float>(batch, "Velocity.Y: ", Velocity.Y, 4);
            DebugUI.DrawDebug<bool>(batch, "ShouldFrictionBeApplyed: ", ShouldFrictionBeApplyed, 6);
        }

        private Skeleton2D body_skel;
        private Bone2D bone_torso, bone_head, bone_heands;
        private FlipBook fb_legs;
        private FlipBook fb_torso;
        private Animation anim_legs_walk;
        private Animation anim_torso_idle;

        private Point legs_offset = new Point(0, 39);
        private Point torso_offset = new Point(0, 23);

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

        private void UpdateFrictionApplianceStatus(GameTime gameTime)
        {
            if ((Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.D))
            && Math.Abs(Velocity.X) * gameTime.GetElapsedSeconds() <= MaxMovementSpeed * gameTime.GetElapsedSeconds())
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