using System.ComponentModel;
using BindingProxy;
using PropertyChanged;

namespace SL.UI.Models
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class HealthModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public HealthModel(float value = 100f)
        {
            Value = value;
            MaxValue = value;
        }

        public float MaxValue { get; set; }

        public float Value { get; set; }
    }
}