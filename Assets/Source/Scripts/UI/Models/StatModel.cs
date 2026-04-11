using BindingProxy;
using PropertyChanged;

namespace SL.UI.Models
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class StatModel
    {
        public StatModel()
        {
        }

        public StatModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Icon { get; set; }
        public int Value { get; set; }
    }
}