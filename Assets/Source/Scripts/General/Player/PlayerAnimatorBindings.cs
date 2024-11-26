using UnityEngine;

namespace SL.General.Player
{
    public class PlayerAnimatorBindings : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public float Speed
        {
            get => default;
            set => _animator.SetFloat(PlayerAnimatorConsts.Speed, value);
        }

        public float DodgeValue
        {
            get => default;
            set => _animator.SetFloat(PlayerAnimatorConsts.DodgeType, value);
        }

        public bool IsGrounded
        {
            get => default;
            set => _animator.SetBool(PlayerAnimatorConsts.OnGround, value);
        }

        public bool IsBonfireSitting
        {
            get => default;
            set => _animator.SetBool(PlayerAnimatorConsts.Sitting, value);
        }

        public bool IsDodging
        {
            get => default;
            set
            {
                if (value)
                    _animator.SetTrigger(PlayerAnimatorConsts.Dodge);
                else
                    _animator.ResetTrigger(PlayerAnimatorConsts.Dodge);
            }
        }
    }
}