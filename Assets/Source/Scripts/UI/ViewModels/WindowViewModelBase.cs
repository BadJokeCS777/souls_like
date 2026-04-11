using Loxodon.Framework.Commands;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Messaging;
using Loxodon.Log;
using SL.UI.ViewModels.Base;

namespace SL.UI.ViewModels
{
    public abstract class WindowViewModelBase : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WindowViewModelBase));
        public InteractionRequest CloseWindowRequest { get; }
        public ICommand CloseCommand { get; }

        protected WindowViewModelBase(IMessenger messenger) : base(messenger)
        {
            CloseCommand = new SimpleCommand(OnCloseClick);
            CloseWindowRequest = new InteractionRequest(this);
        }

        protected virtual void OnCloseClick()
        {
            Log.Info($"{nameof(OnCloseClick)}");
            RequestClose();
        }

        protected void RequestClose()
        {
            CloseWindowRequest.Raise();
        }

        public virtual void Init()
        {
        }

        public virtual void OnHide()
        {
        }
    }
}