using System.Collections.Generic;
using Cinemachine;
using SL.Game.Bonfires;
using UnityEngine;
using Zenject;

namespace SL.Game
{
    public class SceneRoot : MonoBehaviour
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
            //TODO: add respawn when HP == 0
            SpawnPoint spawnPoint = _bonfiresSettings.GetPoint(_bonfireManager.LastBonfireId);

            _player = _container.InstantiatePrefabForComponent<Player.Player>(
                _playerPrefab.gameObject,
                spawnPoint.Position,
                Quaternion.Euler(spawnPoint.Rotation),
                null);

            InitCameras(_player.transform);
        }

        private void InitCameras(Transform target)
        {
            _mainCamera.Follow = target;
            _mainCamera.LookAt = target;
        }
    }
}
