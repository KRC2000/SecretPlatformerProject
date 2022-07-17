using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace Project
{
    class Animation
    {
        public FlipBook FlipBook { get; private set; }
        public float Speed { get; set; }
        public bool Playing { get; private set; } = false;


        public Animation(FlipBook flipBook, int frameCount, float speed = 1f, int rowNumber = 0)
        {
            FlipBook = flipBook;
            Speed = speed;

            StartFBookPage = (FlipBook.Texture.Width / FlipBook.FrameSize.X) * rowNumber + 1;
            EndFBookPage = StartFBookPage + frameCount - 1;
        }

        public void Play()
        {
            Playing = true;
        }

        public void Pause()
        {
            Playing = false;
        }

        public void Stop()
        {
            Playing = false;
            FlipBook.CurrentPage = StartFBookPage;
            frameCountDown = 0;
        }

        public void PlayFromStart()
        {
            Playing = true;
            FlipBook.CurrentPage = StartFBookPage;
            frameCountDown = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (Playing){
                frameCountDown += Speed * gameTime.GetElapsedSeconds();
                if (frameCountDown >= 1){
                    FlipBook.NextPage();
                    frameCountDown -= 1;
                }

                if (FlipBook.CurrentPage < StartFBookPage || FlipBook.CurrentPage > EndFBookPage) FlipBook.CurrentPage = StartFBookPage;
            }
        }
        private int StartFBookPage;
        private int EndFBookPage;
        private float frameCountDown = 0;
    }
}