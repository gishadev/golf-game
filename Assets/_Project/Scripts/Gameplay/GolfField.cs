﻿using UnityEngine;

namespace gishadev.golf.Gameplay
{
    public class GolfField : MonoBehaviour
    {
        public BallSpawnpoint BallSpawnpoint { get; private set; }
        public Hole Hole { get; private set; }
        
        private void Awake()
        {
            BallSpawnpoint = GetComponentInChildren<BallSpawnpoint>();
            Hole = GetComponentInChildren<Hole>();
        }
    }
}