using BindingProxy;
using Loxodon.Framework.Observables;
using PropertyChanged;

namespace SL.UI.Models
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class StatsModel
    {
        public int Vitality { get; set; }
        public int Endurance { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Faith { get; set; }
        public int Magic { get; set; }

        public ObservableList<string> Stats { get; set; } = new();
    }
}