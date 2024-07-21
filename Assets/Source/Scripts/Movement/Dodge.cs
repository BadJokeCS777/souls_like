using UnityEngine;

namespace SL.Movement
{
    public class Dodge
    {
        private float _dodgeDistance;
        private float _dodgeDistanceProgress;
        
        public Vector3 Direction { get; private set; }
        public bool IsProcessing => _dodgeDistanceProgress < _dodgeDistance;

        public void Init(float distance, Vector3 direction)
        {
            _dodgeDistance = distance;
            Direction = direction;
        }

        public void Reset() => _dodgeDistanceProgress = 0f;

        public void Update(float distanceDelta)
        {
            _dodgeDistanceProgress += distanceDelta;
        }
    }
}