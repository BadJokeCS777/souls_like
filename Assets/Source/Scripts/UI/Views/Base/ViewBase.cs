using System;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Views;
using SL.Services;
using SL.UI.ViewModels.Base;
using UnityEngine;
using Zenject;

namespace SL.UI.Views.Base
{
    public abstract class ViewBase<TView, TViewModel> : UIView
        where TView : ViewBase<TView, TViewModel>
        where TViewModel : ViewModelBase
    {
        protected TViewModel ViewModel { get; private set; }

        [Inject]
        protected void Construct(IViewModelsFactory factory)
        {
            Debug.Log(nameof(Construct));
            try
            {
                ViewModel = factory.Create<TViewModel>();
            }
            catch (Exception e)
            {
                throw new Exception($"Can not create view model :{typeof(TViewModel)} for {typeof(TView)}; \n {e}");
            }
        }

        //TODO: finish binding
        protected override void Start()
        {
            base.Start();
            ViewModel.Initialize();
            Bind();
        }

        public void Bind()
        {
            BindingSet<TView, TViewModel> bindingSet = CreateSet();
            Bind(bindingSet);
            bindingSet.Build();
        }

        protected abstract void Bind(BindingSet<TView, TViewModel> bindingSet);

        private BindingSet<TView, TViewModel> CreateSet() => ((TView)this).CreateBindingSet(ViewModel);
    }
}