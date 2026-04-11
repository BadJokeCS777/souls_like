using Loxodon.Framework.Commands;
using Loxodon.Framework.Messaging;
using SL.Common;

namespace SL.UI.ViewModels.Base
{
    public abstract class SelectableItemViewModelBase : ViewModelBase, ISelectable, IUniqueItem
    {
        public string Id { get; protected set; }
        public string IconPath { get; protected set; }
        public virtual bool Selected { get; set; }
        public abstract bool IsActive { get; }

        public ICommand CloseCommand { get; private set; }
        public ICommand SelectedCommand { get; private set; }

        protected SelectableItemViewModelBase(IMessenger messenger) : base(messenger)
        {
            CloseCommand = new SimpleCommand(CloseCommandApply);
            SelectedCommand = new SimpleCommand(SelectedCommandApply);
        }

        public abstract void SetItem(string itemId);

        protected abstract void SelectedCommandApply();

        protected virtual void CloseCommandApply() { }

        protected virtual void FreePreviousModel() { }

        protected abstract void OnModelSet();
    }
}