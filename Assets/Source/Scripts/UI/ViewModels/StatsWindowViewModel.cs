using Loxodon.Framework.Messaging;
using SL.UI.Models;

namespace SL.UI.ViewModels
{
    public class StatsWindowViewModel : WindowViewModelBase
    {
        public StatsWindowViewModel(IMessenger messenger, StatsModel model) : base(messenger)
        {
            Model = model;
        }

        public StatsModel Model { get; set; }
    }
}