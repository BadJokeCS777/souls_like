using UnityEngine;

namespace Movement
{
    public interface IMovement
    {
        public bool IsMoving { get; }
        public Vector3 Direction { get; }
    }
}