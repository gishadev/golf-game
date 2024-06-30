using System;
using System.Collections.Generic;
using System.Linq;
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
        public static event Action<GolfPlayer> PlayerSwitched;

        private readonly List<GolfPlayerContainer> _playerContainers = new();
        private readonly int _playersCount = 2;

        public void Initialize()
        {
            GolfHole.BallScoredHole += OnBallEnteredHole;
            GolfClubController.ClubPunch += OnClubPunch;

            CurrentGolfField = Object.FindObjectOfType<GolfField>();
            CreatePlayerContainers();
            SetupGolfPlayers();
        }

        public void Dispose()
        {
            GolfHole.BallScoredHole -= OnBallEnteredHole;
            GolfClubController.ClubPunch -= OnClubPunch;
            
            foreach (var p in Players)
                p.UnsubscribeEvents();
        }

        private void SetupGolfPlayers()
        {
            Players = new GolfPlayer[_playerContainers.Count];
            for (var i = 0; i < Players.Length; i++)
            {
                Players[i] = new GolfPlayer(i, _playerContainers[i]);
                Players[i].SubscribeEvents();
            }

            SwitchPlayer(Players[0]);
        }

        private void CreatePlayerContainers()
        {
            for (int i = 0; i < _playersCount; i++)
            {
                var playerContainer = Object.Instantiate(_gameDataSO.PlayerContainerPrefab,
                        CurrentGolfField.BallSpawnpoint.transform.position, Quaternion.identity)
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

        private void OnClubPunch(GolfBall golfBall) =>
            CurrentTurnPlayer.GolfPlayerContainer.GolfBall.OnBallStopped += OnBallStopped;

        private void OnBallStopped()
        {
            var notScoredPlayers = Players
                .Where(x => !x.GolfPlayerContainer.GolfBall.IsScored)
                .ToArray();

            if (notScoredPlayers.Length == 0)
                return;

            var currentPlayerIndex = Array.IndexOf(notScoredPlayers, CurrentTurnPlayer);
            var nextPlayerIndex = currentPlayerIndex + 1;
            if (nextPlayerIndex >= notScoredPlayers.Length)
                nextPlayerIndex = 0;

            SwitchPlayer(notScoredPlayers[nextPlayerIndex]);
            CurrentTurnPlayer.GolfPlayerContainer.GolfBall.OnBallStopped -= OnBallStopped;
        }

        private void OnBallEnteredHole(GolfBall ball)
        {
            Debug.Log("Ball entered hole");
        }
    }
}