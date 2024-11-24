using Cinemachine;
using UnityEngine;
using Zenject;

namespace SL.General
{
    public class SceneRoot : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraBase _mainCamera;
        [SerializeField] private Player.Player _playerPrefab;

        [Inject]
        private void Construct(DiContainer container)
        {
            var player = container.InstantiatePrefabForComponent<Player.Player>(_playerPrefab.gameObject);
            InitCameras(player.transform, player.transform);
        }

        private void InitCameras(Transform follow, Transform lookAt)
        {
            _mainCamera.Follow = follow;
            _mainCamera.LookAt = lookAt;
        }
    }
}
