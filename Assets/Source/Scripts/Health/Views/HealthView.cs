using Loxodon.Framework.Binding;
using Loxodon.Framework.Views;
using SL.Health.Models;
using SL.Health.ViewModels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SL.Health.Views
{
    public class HealthView : UIView
    {
        [SerializeField] private Image _bar;

        private HealthViewModel _viewModel;

        [Inject]
        private void Construct(HealthModel model)
        {
            _viewModel = new HealthViewModel(model);
            _viewModel.Initialize();
        }

        protected override void Start()
        {
            base.Start();
            Bind();
        }

        private void Bind()
        {
            var bindingSet = this.CreateBindingSet(_viewModel);
            bindingSet.Bind(_bar)
                .For(v => v.fillAmount)
                .To(vm => vm.FillAmount);
            bindingSet.Build();
        }
    }
}
