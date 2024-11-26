using BindingProxy;
using PropertyChanged;

namespace SL.General.Player
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class PlayerAnimatorModel
    {
        public bool IsGrounded { get; set; }
        public bool IsMoving { get; set; }
        public bool IsSitting { get; set; }
    }
}