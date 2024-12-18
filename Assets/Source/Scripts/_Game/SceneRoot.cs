using System.Collections.Generic;
using Cinemachine;
using SL.Common;
using SL.Game.Bonfires;
using SL.Game.Settings;
using SL.Signals;
using UnityEngine;
using Zenject;

namespace SL.Game
{
    public class SceneRoot : MessengerBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraBase _mainCamera;
        [SerializeField] private Player.Player _playerPrefab;
        [SerializeField] private BonfiresSettings _bonfiresSettings;

        private Player.Player _player;
        private Dictionary<string, SpawnPoint> _bonfiresDictionary;
        private DiContainer _container;
        private BonfireManager _bonfireManager;

        [Inject]
        private void Construct(DiContainer container, BonfireManager bonfireManager)
        {
            _container = container;
            _bonfireManager = bonfireManager;
            _bonfiresSettings.Initialize();
        }

        private void Start()
        {
            _player = _container.InstantiatePrefabForComponent<Player.Player>(_playerPrefab.gameObject);
            _player.Init(_bonfiresSettings.GetPoint(_bonfireManager.LastBonfireId));

            InitCameras(_player.transform);

            Subscribe<ZeroHealthMessage>(OnZeroHealthMessage);
        }

        private void InitCameras(Transform target)
        {
            _mainCamera.Follow = target;
            _mainCamera.LookAt = target;
        }

        private void OnZeroHealthMessage()
            => _player.Init(_bonfiresSettings.GetPoint(_bonfireManager.LastBonfireId));
    }
}
