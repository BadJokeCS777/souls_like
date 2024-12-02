using UnityEngine;

namespace SL.Movement
{
    [CreateAssetMenu(menuName = "Settings/Movement", fileName = "MovementSettings")]
    public class MovementSettings : ScriptableObject
    {
        [Header("Movement")]
        public float Speed = 3.5f;
        public float RunSpeed = 7f;
        public float RotationSpeedRatio = 15f;

        [Header("Dodge")]
        public float RollingDistance = 3.5f;
        public float StepBackDistance = 1f;
        public AnimationCurve StepBackCurve;
        public AnimationCurve RoleCurve;

        [Header("Jump")]
        public float GroundedGravity = -9.8f;
        public float JumpHeight = 1f;
        public float JumpTime = 1f;
        public float FallMultiplier = 2f;
    }
}