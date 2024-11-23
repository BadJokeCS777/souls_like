using Loxodon.Framework.Binding.Builder;
using SL.UI.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace SL.UI.Views
{
    public class HealthView : ViewBase<HealthView, HealthViewModel>
    {
        [SerializeField] private Image _bar;

        protected override void Bind(BindingSet<HealthView, HealthViewModel> bindingSet)
        {
            bindingSet.Bind(_bar)
                .For(v => v.fillAmount)
                .To(vm => vm.FillAmount);
        }
    }
}
