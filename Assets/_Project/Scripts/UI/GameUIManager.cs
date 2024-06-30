using System;
using gishadev.golf.Core;
using gishadev.golf.Gameplay;
using TMPro;
using UnityEngine;

namespace gishadev.golf.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerTurnText;

        private void Awake()
        {
            playerTurnText.text = string.Empty;
        }

        private void OnEnable()
        {
            GameManager.PlayerSwitched += OnPlayerSwitched;
            GolfClubController.ClubPunch += OnClubPunch;
        }

        private void OnDisable()
        {
            GameManager.PlayerSwitched -= OnPlayerSwitched;
            GolfClubController.ClubPunch -= OnClubPunch;
        }

        private void OnPlayerSwitched(GolfPlayer golfPlayer) => playerTurnText.text = $"Player{golfPlayer.Index + 1}'s Turn";
        private void OnClubPunch(GolfBall golfBall)
        {
            playerTurnText.text = string.Empty;
        }
    }
}