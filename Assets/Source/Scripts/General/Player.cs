using Movement;
using UnityEngine;

namespace General
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;

        public void Init(Transform cameraTransform)
        {
            _movement.Init(cameraTransform);
        }
    }
}