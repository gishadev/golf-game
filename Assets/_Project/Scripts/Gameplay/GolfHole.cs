using System;
using DG.Tweening;
using UnityEngine;

namespace gishadev.golf.Gameplay
{
    public class GolfHole : MonoBehaviour
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
            
            var sequence = DOTween.Sequence();
            sequence.Append(ball.transform.DOMove(transform.position, 0.5f));
            sequence.Append(ball.transform.DOScale(Vector3.zero, 0.5f));
            
            sequence.OnComplete(() => ball.gameObject.SetActive(false));
        }
    }
}