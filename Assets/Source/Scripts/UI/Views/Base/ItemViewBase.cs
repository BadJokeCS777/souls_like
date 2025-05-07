using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Views;
using SL.Services;
using SL.UI.ViewModels.Base;
using Zenject;

namespace SL.UI.Views.Base
{
    public abstract class ItemViewBase<T> : ItemViewBase where T : SelectableItemViewModelBase
    {
        private IViewModelsFactory _factory;
        public T ViewModel { get; protected set; }

        public override void SetItem(string id)
        {
            Id = ViewModel.Id;
            //ViewModel.SetGroup(Group);
            ViewModel.SetItem(id);
        }

        [Inject]
        private void Construct(IViewModelsFactory factory)
        {
            ViewModel = factory.Create<T>();
        }

        protected override void Start()
        {
            BindInternal();
            base.Start();
        }

        private void BindInternal()
        {
            var bindingSet = this.CreateBindingSet(ViewModel);
            bindingSet.Bind(gameObject)
                .For(v => v.activeSelf)
                .ToExpression(v => v != null);
            Bind(bindingSet);
            bindingSet.Build();
        }

        protected abstract void Bind(BindingSet<ItemViewBase<T>, T> bindingSet);
    }

    public abstract class AbstractItemViewBase : UIView
    {
        //public ISelectionGroup Group { get; set; }
    }

    public abstract class AbstractItemViewBase<TItem> : AbstractItemViewBase
    {
        public TItem Id { get; protected set; }
        public abstract void SetItem(TItem id);
    }

    public abstract class ItemViewBase : AbstractItemViewBase<string>
    {
    }
}