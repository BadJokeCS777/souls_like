using UnityEngine;

namespace SL.Movement
{
    public interface IMovement
    {
        public bool IsMoving { get; }
        public Vector3 Direction { get; }

        public void Init(Transform cameraTransform);
    }
}