using UnityEngine;

namespace SL.General.Player
{
    [CreateAssetMenu(menuName = "Settings/Animations", fileName = "AnimationsSettings")]
    public class AnimationsSettings : ScriptableObject
    {
        //TODO: move weight to player stats
        [Min(0f)] public float Weight = 0.25f;
        [Min(0f)] public float HeavyRollThreshold = 0.5f;
        [Min(0f)] public float MovingChangeDuration = 0.1f;
        public float StayingSpeed = 0f;
        public float MovingSpeed = 1f;
        public float RunningSpeed = 2f;
    }
}