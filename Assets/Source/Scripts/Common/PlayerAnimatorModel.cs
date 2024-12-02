using System.ComponentModel;
using BindingProxy;
using PropertyChanged;

namespace SL.Common
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class PlayerAnimatorModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsMoving { get; set; }
        public bool IsDodging { get; set; }
        public bool IsGrounded { get; set; } = true;
        public bool IsBonfireSitting { get; set; }
        public float DodgeValue { get; set; } = 0f;
    }
}