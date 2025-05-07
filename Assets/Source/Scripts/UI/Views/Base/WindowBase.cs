using System;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Messaging;
using Loxodon.Framework.Views;
using SL.Services;
using SL.UI.ViewModels;
using Zenject;

namespace SL.UI.Views.Base
{
    public abstract class WindowBase<TView, TViewModel> : Window
        where TViewModel : WindowViewModelBase
        where TView : WindowBase<TView, TViewModel>
    {
        private TViewModel _viewModel;

        public TViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnViewModelChanged();
            }
        }

        [Inject]
        protected void Construct(IMessenger messenger, IViewModelsFactory factory)
        {
            try
            {
                ViewModel = factory.Create<TViewModel>();
            }
            catch (Exception e)
            {
                throw new Exception($"Can not create view model :{typeof(TViewModel)} for {typeof(TView)}; \n {e}");
            }
        }

        private BindingSet<TView, TViewModel> CreateSet(TView w) => w.CreateBindingSet(ViewModel);

        protected override void OnCreate(IBundle bundle)
        {
            var bindingSet = CreateSet(this as TView);
            bindingSet.Bind().For(v => v.CloseWindow).To(vm => vm.CloseWindowRequest);
            bindingSet.Bind(gameObject).For(v => v.activeSelf).To(vm => vm.CloseWindowRequest);
            Bind(bindingSet);
            bindingSet.Build();
        }

        protected abstract void Bind(BindingSet<TView, TViewModel> bindingSet);

        protected override void OnShow()
        {
            ViewModel.Init();
            base.OnShow();
        }

        protected override void OnHide()
        {
            ViewModel.OnHide();
            base.OnHide();
        }

        protected void CloseWindow(object sender, InteractionEventArgs e)
        {
            Hide();
        }

        protected virtual void OnViewModelChanged()
        {
        }
    }
}