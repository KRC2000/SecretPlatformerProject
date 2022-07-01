using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGame.Extended;

namespace Project
{
    public class Map
    {
       

        public float Gravity { get; private set; } = 140;
        public float Friction { get; private set; } = 600f;
        public List<Chunk> Chunks;

        public Map(){
            Chunks = new List<Chunk>();
        }

        public void AddChunk(Texture2D levelTexture, Point position){
            Chunks.Add(new Chunk(levelTexture, position));
        }

        /// <summary>
        /// Gets state of pixel at the passed position. 
        /// </summary>
        /// <param name="pos">Position on the map, where pixel status needed</param>
        /// <param name="pixelStatus">Status of the pixel. Solid - if pixel is not white and fully transparent, 
        /// Empty - if it is. None - if there is no chunks with pixels at such position</param>
        /// <param name="solidPixelOwner">Chunk that owns Solid pixel. != null only if pixel status is Solid</param>
        /// <returns>true if there is any pixel at passed position and the status is successfully retrieved, false otherwise</returns>
        public bool GetPixelStatusAtPos(Vector2 pos, out MapPixelStatus pixelStatus, out Chunk solidPixelOwner){
            Point pos_p = new Point((int)Math.Floor(pos.X), (int)Math.Floor(pos.Y));
            solidPixelOwner = null;
            pixelStatus = MapPixelStatus.None;
            List<Chunk> matchingChunks = new List<Chunk>();

            // Get all chunks that contain requested point
            foreach (Chunk ch in Chunks){
                if (new Rectangle(ch.Position.X, ch.Position.Y, ch.CollisionMapTexture.Bounds.Width, ch.CollisionMapTexture.Bounds.Height).Contains(pos_p))
                    matchingChunks.Add(ch);
            }

            // For all all chunks that contain requested point:
            // Assign pixel status depending on the color of the pixel, if pixel is transparent white - state is Empty, check next chunk
            // If pixel is not transparent white - state is Solid, assign current chunk to solidPixelOwner, terminating by returning true
            if (matchingChunks.Count > 0){
                foreach (Chunk ch in matchingChunks)
                {
                    Point pos_p_relative = pos_p - ch.Position;
                    if (ch.CollisionMap[pos_p_relative.Y * ch.CollisionMapTexture.Width + pos_p_relative.X] != new Color(0, 0, 0, 0))
                    {
                        solidPixelOwner = ch;
                        pixelStatus = MapPixelStatus.Solid;
                        return true;
                    }
                    else pixelStatus = MapPixelStatus.Empty;
                }
                return true;
            }
            else return false;

        }

        // public Color GetColor(Point position)
        // {
        //     foreach (Chunk ch in Chunks)
        //     {
        //         if (new Rectangle(ch.Position.X, ch.Position.Y, ch.CollisionMapTexture.Bounds.Width, ch.CollisionMapTexture.Bounds.Height).Contains(position))
        //         {
        //             return ch.CollisionMap[position.Y * ch.CollisionMapTexture.Width + position.X];
        //         }
        //     }

        //     throw new System.Exception("Couldn't pick the texture at this point");
        // }

        // public Color GetColor(Point position, out Chunk chunk)
        // {
        //     foreach (Chunk ch in Chunks)
        //     {
        //         if (new Rectangle(ch.Position.X, ch.Position.Y, ch.CollisionMapTexture.Bounds.Width, ch.CollisionMapTexture.Bounds.Height).Contains(position))
        //         {
        //             chunk = ch;
        //             return ch.CollisionMap[position.Y * ch.CollisionMapTexture.Width + position.X];
        //         }
        //     }

        //     throw new System.Exception("Couldn't pick the texture at this point");
        // }

        public void Update()
        {

        }

        public void Draw(SpriteBatch batch, OrthographicCamera camera)
        {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());
            foreach (Chunk level in Chunks)
            {
                batch.Draw(level.CollisionMapTexture, level.Position.ToVector2(), Color.White);
            }
            batch.End();
        }
    }
}