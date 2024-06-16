using System;
using DG.Tweening;
using UnityEngine;

namespace gishadev.golf.Gameplay
{
    public class Hole : MonoBehaviour
    {
        public static Action<GolfBall> BallEnteredHole;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out GolfBall golfBall))
            {
                InitBallInHoleAnimation(golfBall);
                BallEnteredHole?.Invoke(golfBall);
            }
        }
        
        private void InitBallInHoleAnimation(GolfBall ball)
        {
            ball.ChangeBodyType(RigidbodyType2D.Kinematic);
            ball.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
            {
                ball.gameObject.SetActive(false);
            });
        }
    }
}