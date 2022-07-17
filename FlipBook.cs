using System;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project
{
    struct FrameSequence
    {
        Vector2 firstFrameOffset; 
    }

    class FlipBook
    {
        public Texture2D Texture { get; private set; }
        public Point FrameSize { get; private set; }
        public Vector2 FrameOrigin { get; private set; }
        public bool FlipHorizontally { get; set; } = false;
        public bool FlipVertically { get; set; } = false;

        public int FrameCount { get; private set; }
        
        private int currentPage = 1;
        public int CurrentPage { 
            get { return currentPage;} 
            set { 
                if (value > FrameCount){
                    int times = value / FrameCount;
                    currentPage = value - (times * FrameCount);
                } 
                else if (value < 1) 
                    currentPage = 1; 
                else 
                    currentPage = value;
            } 
        }


        public FlipBook(Texture2D texture, Point frameSize, Vector2 frameOrigin, bool flipHorizontally = false)
        {
            Texture = texture;
            FrameSize = frameSize;
            FrameCount = (texture.Width / frameSize.X) * (texture.Height / frameSize.Y);
            FlipHorizontally = flipHorizontally;
            FrameOrigin = frameOrigin;
        }

        public Rectangle GetSourceRectangle()
        {
            if (!FlipHorizontally){}
            int columns = Texture.Width / FrameSize.X;
            int frameRow = (CurrentPage-1) / columns;
            int sourceX = (CurrentPage-1) - (frameRow * columns);
            int sourceY = frameRow;
            
            return new Rectangle(sourceX * FrameSize.X, sourceY * FrameSize.Y, FrameSize.X, FrameSize.Y);
        }

        public SpriteEffects GetSpriteEffects()
        {
            return ((FlipHorizontally) ? SpriteEffects.FlipHorizontally : SpriteEffects.None) 
                    | ((FlipVertically) ? SpriteEffects.FlipVertically : SpriteEffects.None);
        }

        public void NextPage() => CurrentPage++;

        public void ToStart() => currentPage = 1;

    }

}