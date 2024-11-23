using Loxodon.Framework.Binding;
using Loxodon.Framework.Messaging;
using Loxodon.Framework.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SL.Interactions
{
    public class InteractionsView : UIView
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _buttonIcon;
        [SerializeField] private GameObject _container;

        private InteractionsViewModel _viewModel;

        [Inject]
        private void Construct(InteractionsModel model, IMessenger messenger)
        {
            _viewModel = new InteractionsViewModel(model, messenger);
        }

        protected override void Start()
        {
            base.Start();
            Bind();
            _viewModel.Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _viewModel.Dispose();
        }

        private void Bind()
        {
            var bindingSet = this.CreateBindingSet(_viewModel);
            bindingSet.Bind(_text)
                .For(v => v.text)
                .To(vm => vm.Text)
                .OneWay();
            bindingSet.Bind(_buttonIcon)
                .For(v => v.sprite)
                .To(vm => vm.Icon)
                .OneWay();
            bindingSet.Bind(_container)
                .For(v => v.activeSelf)
                .To(vm => vm.Active)
                .OneWay();
            bindingSet.Build();
        }
    }
}