using System;
using gishadev.golf.Gameplay;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace gishadev.golf.Core
{
    public class GameManager : IGameManager, IInitializable, IDisposable
    {
        public GolfPlayer[] Players { get; private set; }
        public GolfPlayer CurrentTurnPlayer { get; private set; }

        public static event Action Won;
        public static event Action<GolfPlayer> PlayerSwitched;

        public void Initialize()
        {
            Hole.BallEnteredHole += OnBallEnteredHole;
            GolfClubController.ClubPunch += OnClubPunch;
            SetupGolfPlayers();
        }

        public void Dispose()
        {
            Hole.BallEnteredHole -= OnBallEnteredHole;
            GolfClubController.ClubPunch -= OnClubPunch;
        }

        private void OnClubPunch()
        {
            // Switch to the next player.
            var currentPlayerIndex = Array.IndexOf(Players, CurrentTurnPlayer);
            var nextPlayerIndex = currentPlayerIndex + 1;
            if (nextPlayerIndex >= Players.Length)
                nextPlayerIndex = 0;
            
            SwitchPlayer(Players[nextPlayerIndex]);
        }

        private void SetupGolfPlayers()
        {
            var golfPlayerContainers = Object.FindObjectsOfType<GolfPlayerContainer>();
            Players = new GolfPlayer[golfPlayerContainers.Length];
            for (var i = 0; i < golfPlayerContainers.Length; i++)
                Players[i] = new GolfPlayer(golfPlayerContainers[i]);

            SwitchPlayer(Players[0]);
        }

        private void SwitchPlayer(GolfPlayer player)
        {
            CurrentTurnPlayer = player;
            PlayerSwitched?.Invoke(CurrentTurnPlayer);
        }
        
        private void OnBallEnteredHole(GolfBall ball)
        {
            Debug.Log("Ball entered hole");
            Won?.Invoke();
        }
    }
}