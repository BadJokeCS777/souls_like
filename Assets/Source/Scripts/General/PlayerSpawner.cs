using System;
using UnityEngine;

namespace SL.General
{
    [Serializable]
    public class PlayerSpawner
    {
        [SerializeField] private Player _player;
        
        public Player Spawn(Transform cameraTransform)
        {
            var player = UnityEngine.Object.Instantiate(_player);
            player.Init(cameraTransform);
            return player;
        }
    }
}