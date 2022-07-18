using System;
using Microsoft.Xna.Framework;

using MonoGame.Extended;

namespace Project
{
    class ActorToMapInteractionManager
    {
        public static void ActorToMap_collision(Actor actor, Map map, GameTime gameTime)
        {
            // apply gravity
            actor.Velocity.Y += map.Gravity * gameTime.GetElapsedSeconds();


            // update airborne status
            MapPixelStatus pixelStatus;
            Chunk solidPxOwner;
            map.GetPixelStatusAtPos(actor.GroundLowerSensor, out pixelStatus, out solidPxOwner);
            if (pixelStatus == MapPixelStatus.Solid)
            {
                actor.IsAirborne = false;
                actor.Velocity.Y = 0;
            }
            else actor.IsAirborne = true;

            // apply friction

            if (!actor.IsAirborne && actor.ShouldFrictionBeApplyed)
            {
                if (Math.Abs(actor.Velocity.X) < map.Friction * gameTime.GetElapsedSeconds()) actor.Velocity.X = 0;
                else
                {
                    float friction_velX = (Math.Sign(actor.Velocity.X) * -1) * (map.Friction * gameTime.GetElapsedSeconds()); 
                    actor.Velocity.X += friction_velX;
                }
            }

            ApplyActorVelocity(actor, map, gameTime);
        }

        // apply player's velocity by a little(normal), checking for collisions with bit map every time. 
        // such check needed to prevent character going through thin walls and floors when speed is high enough
        private static void ApplyActorVelocity(Actor actor, Map map, GameTime gameTime)
        {
            Vector2 vel_norm = actor.Velocity; vel_norm.Normalize();
            float vel_len = actor.Velocity.Length();


            while (vel_len > 0)
            {
                if (vel_len > 1)
                {
                    actor.Transform.Position += vel_norm * gameTime.GetElapsedSeconds();
                    actor.UpdatePixelSensorsPos();
                    vel_len -= 1;
                }
                else
                {
                    actor.Transform.Position += (vel_norm * vel_len) * gameTime.GetElapsedSeconds();
                    actor.UpdatePixelSensorsPos();
                    vel_len = 0;
                }

                MapPixelStatus pixelStatus;
                Chunk solidPxOwner;
                // if in the ground - lift up until on the surface
                do
                {
                    map.GetPixelStatusAtPos(actor.GroundUpperSensor, out pixelStatus, out solidPxOwner);
                    if (pixelStatus == MapPixelStatus.Solid)
                    {
                        actor.Transform.Position -= new Vector2(0, 1f);
                        actor.UpdatePixelSensorsPos();
                    }

                } while (pixelStatus == MapPixelStatus.Solid);

                // if in the wall on the left - move right until its not
                do
                {
                    map.GetPixelStatusAtPos(actor.LeftSensor, out pixelStatus, out solidPxOwner);
                    if (pixelStatus == MapPixelStatus.Solid)
                    {
                        actor.Transform.Position += new Vector2(1f, 0);
                        actor.UpdatePixelSensorsPos();
                    }

                } while (pixelStatus == MapPixelStatus.Solid);

                // if in the wall on the right - move left until its not
                do
                {
                    map.GetPixelStatusAtPos(actor.RightSensor, out pixelStatus, out solidPxOwner);
                    if (pixelStatus == MapPixelStatus.Solid)
                    {
                        actor.Transform.Position -= new Vector2(1f, 0);
                        actor.UpdatePixelSensorsPos();
                    }

                } while (pixelStatus == MapPixelStatus.Solid);
            }
        }
    }
}