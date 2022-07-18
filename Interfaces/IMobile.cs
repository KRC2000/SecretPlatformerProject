using Microsoft.Xna.Framework;

namespace Project
{
    interface IMobile
    {
        public void GoLeft();
        public void GoRight();
        public void StopMoving();
    }
}