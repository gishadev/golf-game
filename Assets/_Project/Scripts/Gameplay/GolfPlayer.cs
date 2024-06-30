using UnityEngine;

namespace gishadev.golf.Gameplay
{
    public class GolfPlayer
    {
        public int Index { get; private set; }
        public int HitsCount { get; private set; }
        public GolfPlayerContainer GolfPlayerContainer { get; private set; }

        public GolfPlayer(int index, GolfPlayerContainer golfPlayerContainer)
        {
            Index = index;
            GolfPlayerContainer = golfPlayerContainer;
            HitsCount = 0;
        }

        public void SubscribeEvents()
        {
            GolfClubController.ClubPunch += OnClubPunch;
        }

        public void UnsubscribeEvents()
        {
            GolfClubController.ClubPunch -= OnClubPunch;

        }
        
        private void OnClubPunch(GolfBall golfBall)
        {
            if (golfBall != GolfPlayerContainer.GolfBall)
                return;
            
            HitsCount++;
            Debug.Log($"Player {Index} hits count: {HitsCount}");
        }
        
    }
}