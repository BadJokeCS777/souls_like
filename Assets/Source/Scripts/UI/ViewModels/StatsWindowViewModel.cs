using BindingProxy;
using Loxodon.Framework.Messaging;
using PropertyChanged;
using SL.UI.Models;

namespace SL.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class StatsWindowViewModel : WindowViewModelBase
    {
        public StatsWindowViewModel(IMessenger messenger, StatsModel model) : base(messenger)
        {
            Model = model;
        }

        public StatsModel Model { get; set; }
    }
}