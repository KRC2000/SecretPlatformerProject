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
        private float frameCountDown = 0;

        public Animation(FlipBook flipBook, float speed = 1f)
        {
            FlipBook = flipBook;
            Speed = speed;
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
            FlipBook.ToStart();
            frameCountDown = 0;
        }

        public void PlayFromStart()
        {
            Playing = true;
            FlipBook.ToStart();
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
            }
        }
    }
}