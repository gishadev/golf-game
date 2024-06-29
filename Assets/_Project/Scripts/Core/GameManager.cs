using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using gishadev.golf.Gameplay;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace gishadev.golf.Core
{
    public class GameManager : IGameManager, IInitializable, IDisposable
    {
        [Inject] private GameDataSO _gameDataSO;
        
        public GolfPlayer[] Players { get; private set; }
        public GolfPlayer CurrentTurnPlayer { get; private set; }
        public GolfField CurrentGolfField { get; private set; }
        public static event Action Won;
        public static event Action<GolfPlayer> PlayerSwitched;
        
        private List<GolfPlayerContainer> _playerContainers = new();
        private readonly int _playersCount = 2;

        public void Initialize()
        {
            GolfHole.BallEnteredHole += OnBallEnteredHole;
            GolfClubController.ClubPunch += OnClubPunch;
            
            CurrentGolfField = Object.FindObjectOfType<GolfField>();
            CreatePlayerContainers(); 
            SetupGolfPlayers();
        }

        public void Dispose()
        {
            GolfHole.BallEnteredHole -= OnBallEnteredHole;
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
            Players = new GolfPlayer[_playerContainers.Count];
            for (var i = 0; i < _playerContainers.Count; i++)
                Players[i] = new GolfPlayer(_playerContainers[i]);

            SwitchPlayer(Players[0]);
        }

        private void CreatePlayerContainers()
        {
            for (int i = 0; i < _playersCount; i++)
            {
                var playerContainer = Object.Instantiate(_gameDataSO.PlayerContainerPrefab, CurrentGolfField.BallSpawnpoint.transform.position, Quaternion.identity)
                    .GetComponent<GolfPlayerContainer>();
                _playerContainers.Add(playerContainer);
                playerContainer.gameObject.SetActive(false);
            }
        }
        
        private async void SwitchPlayer(GolfPlayer player)
        {
            CurrentTurnPlayer = player;
            PlayerSwitched?.Invoke(CurrentTurnPlayer);

            await UniTask.Delay(2000);
            // Enable disabled golf player container.
            if (!CurrentTurnPlayer.GolfPlayerContainer.gameObject.activeSelf)
                CurrentTurnPlayer.GolfPlayerContainer.gameObject.SetActive(true);
        }

        private void OnBallEnteredHole(GolfBall ball)
        {
            Debug.Log("Ball entered hole");
            Won?.Invoke();
        }
    }
}