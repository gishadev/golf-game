using System;
using UnityEngine;

namespace gishadev.golf.Gameplay
{
    public class Hole : MonoBehaviour
    {
        public static Action BallEnteredHole;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out GolfBall golfBall))
                BallEnteredHole?.Invoke();
        }
    }
}