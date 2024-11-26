using UnityEngine;

namespace SL.General.Player
{
    public static class PlayerAnimatorConsts
    {
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
        public static readonly int DodgeType = Animator.StringToHash(nameof(DodgeType));
        public static readonly int Dodge = Animator.StringToHash(nameof(Dodge));
        public static readonly int Sitting = Animator.StringToHash(nameof(Sitting));
        public static readonly int OnGround = Animator.StringToHash(nameof(OnGround));
    }
}