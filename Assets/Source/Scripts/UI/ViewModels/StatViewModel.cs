using BindingProxy;
using Loxodon.Framework.Messaging;
using PropertyChanged;
using SL.UI.Models;
using SL.UI.ViewModels.Base;

namespace SL.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class StatViewModel : SelectableItemViewModelBase
    {
        private readonly StatsStorage _statsStorage;

        public StatViewModel(IMessenger messenger, StatsStorage statsStorage) : base(messenger)
        {
            _statsStorage = statsStorage;
        }

        public StatModel Model { get; set; }

        public override bool IsActive { get; }
        public override void SetItem(string itemId)
        {
            Model = _statsStorage.Get(itemId);
            OnModelSet();
        }

        protected override void SelectedCommandApply() { }

        protected override void OnModelSet() { }
    }
}