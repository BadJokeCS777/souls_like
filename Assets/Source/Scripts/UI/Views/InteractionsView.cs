using Loxodon.Framework.Binding.Builder;
using SL.UI.ViewModels;
using SL.UI.Views.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SL.UI.Views
{
    public class InteractionsView : ViewBase<InteractionsView, InteractionsViewModel>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private Image _buttonIcon;
        [SerializeField] private GameObject _container;

        protected override void Bind(BindingSet<InteractionsView, InteractionsViewModel> bindingSet)
        {
            bindingSet.Bind(_text)
                .For(v => v.text)
                .To(vm => vm.Text)
                .OneWay();
            bindingSet.Bind(_buttonIcon.gameObject)
                .For(v => v.activeSelf)
                .ToExpression(vm => vm.ButtonIcon != null)
                .OneWay();
            bindingSet.Bind(_buttonIcon)
                .For(v => v.sprite)
                .To(vm => vm.ButtonIcon)
                .OneWay();
            bindingSet.Bind(_buttonText.gameObject)
                .For(v => v.activeSelf)
                .ToExpression(vm => vm.ButtonText != null)
                .OneWay();
            bindingSet.Bind(_buttonText)
                .For(v => v.text)
                .To(vm => vm.ButtonText)
                .OneWay();
            bindingSet.Bind(_container)
                .For(v => v.activeSelf)
                .To(vm => vm.Active)
                .OneWay();
        }
    }
}