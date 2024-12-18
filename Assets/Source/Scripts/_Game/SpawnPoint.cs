using System;
using UnityEngine;

namespace SL.Game
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
    }
}