using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggScramble
{


    class Dog : Sprite
    {
        double timeSinceStateChange;
        double stateDuration;
        public double DropDelay;

        Vector2 startPosition;
        Vector2 targetPosition;
        Random rand = new Random();
        public event EventHandler<EventArgs> DropEgg;

        public Dog()
        {
            Size = new Vector2(75, 117);
        }

        enum DogState
        {
            Stopped,
            Moving,
            WaitingToDrop,
            WaitingToMove
        }
        DogState state = DogState.Stopped;

        public void Start()
        {
            state = DogState.WaitingToMove;
            SetNextState();
        }

        private void SetNextTargetPosition()
        {
            float pos = Position.X;
            while (Math.Abs(pos - Position.X) < 100)
            {
                pos = rand.Next(50, 750 - (int)Size.X);
            }
            targetPosition = new Vector2(pos, Position.Y);
            startPosition = Position;
            stateDuration = (targetPosition - startPosition).Length() / Speed;
            state = DogState.Moving;
        }

        void SetNextState()
        {
            switch (state)
            {
                case DogState.WaitingToMove:
                    SetNextTargetPosition();
                    break;
                case DogState.Moving:
                    state = DogState.WaitingToDrop;
                    stateDuration = DropDelay;
                    break;
                case DogState.WaitingToDrop:
                    if (DropEgg != null) DropEgg(this, null);
                    state = DogState.WaitingToMove;
                    stateDuration = 0;
                    break;
            }
            timeSinceStateChange = 0;
        }

        public override void Update(double elapsed)
        {
            base.Update(elapsed);
            if (state == DogState.Stopped) return;
            timeSinceStateChange += elapsed;
            if (timeSinceStateChange > stateDuration)
            {
                SetNextState();
            }

            if (state == DogState.Moving)
            {
                Position.X = MathHelper.Lerp(startPosition.X, targetPosition.X, (float)(timeSinceStateChange / stateDuration));
            }
        }
    }
}
