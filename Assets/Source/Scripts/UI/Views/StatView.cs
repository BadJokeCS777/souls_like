using Loxodon.Framework.Binding.Builder;
using SL.UI.ViewModels;
using SL.UI.Views.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SL.UI.Views
{
    public class StatView : ItemViewBase<StatViewModel>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _value;

        protected override void Bind(BindingSet<ItemViewBase<StatViewModel>, StatViewModel> bindingSet)
        {
            bindingSet.Bind(_icon)
                .For(v => v.sprite)
                .To(vm => vm.Model.Icon)
                .WithConversion(UiConstants.SpriteConverter)
                .OneWay();
            bindingSet.Bind(_name)
                .For(v => v.text)
                .To(vm => vm.Model.Name)
                .OneWay();
            bindingSet.Bind(_value)
                .For(v => v.text)
                .To(vm => vm.Model.Value)
                .OneWay();
        }
    }
}