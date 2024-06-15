using System;
using gishadev.golf.Gameplay;
using UnityEngine;

namespace gishadev.golf.Core
{
    public class GameManager : MonoBehaviour
    {
        public static Action Won;
        
        private void Awake()
        {
            Hole.BallEnteredHole += OnBallEnteredHole;
        }

        private void OnDestroy()
        {
            Hole.BallEnteredHole -= OnBallEnteredHole;
        }
        
        private void OnBallEnteredHole()
        {
            Debug.Log("Ball entered hole");
        }
    }
}