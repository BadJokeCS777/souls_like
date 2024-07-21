using Cinemachine;
using UnityEngine;

namespace SL.General
{
    public class SceneRoot : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraBase _mainCamera;
        [SerializeField] private PlayerSpawner _playerSpawner;
        
        private void Start()
        {
            Transform cameraTransform = Camera.main.transform;
            Player player = _playerSpawner.Spawn(cameraTransform);

            InitCameras(player.transform, player.transform);
        }

        private void InitCameras(Transform follow, Transform lookAt)
        {
            _mainCamera.Follow = follow;
            _mainCamera.LookAt = lookAt;
        }
    }
}
