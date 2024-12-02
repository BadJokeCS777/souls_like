using Cinemachine;
using UnityEngine;
using Zenject;

namespace SL.General
{
    public class SceneRoot : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraBase _mainCamera;
        [SerializeField] private Player.Player _playerPrefab;
        [SerializeField] private Transform _spawnPoint;

        [Inject]
        private void Construct(DiContainer container)
        {
            var player = container.InstantiatePrefabForComponent<Player.Player>(
                _playerPrefab.gameObject,
                _spawnPoint.position,
                _spawnPoint.rotation,
                null);
            InitCameras(player.transform);
        }

        private void InitCameras(Transform target)
        {
            _mainCamera.Follow = target;
            _mainCamera.LookAt = target;
        }
    }
}
