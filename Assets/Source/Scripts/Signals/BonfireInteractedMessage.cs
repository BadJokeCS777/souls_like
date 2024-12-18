using UnityEngine;

namespace SL.Signals
{
    public struct BonfireInteractedMessage
    {
        public readonly string Id;
        public readonly Vector3 Position;

        public BonfireInteractedMessage(string id, Vector3 position)
        {
            Id = id;
            Position = position;
        }
    }
}