using System;
using UnityEngine;

namespace SL.Common
{
    [Serializable]
    public struct SpawnPoint
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public SpawnPoint(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.eulerAngles;
        }

        public SpawnPoint(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}