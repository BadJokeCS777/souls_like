using Loxodon.Framework.Binding.Builder;
using SL.UI.ViewModels;
using TMPro;
using UnityEngine;

namespace SL.UI.Views
{
    public class StatsWindow : WindowBase<StatsWindow, StatsWindowViewModel>
    {
        [SerializeField] private TMP_Text _vitalityValue;
        [SerializeField] private TMP_Text _enduranceValue;
        [SerializeField] private TMP_Text _strengthValue;
        [SerializeField] private TMP_Text _dexterityValue;
        [SerializeField] private TMP_Text _intelligenceValue;
        [SerializeField] private TMP_Text _faithValue;
        [SerializeField] private TMP_Text _magicValue;

        protected override void Bind(BindingSet<StatsWindow, StatsWindowViewModel> bindingSet)
        {
            bindingSet.Bind(_vitalityValue)
                .For(v => v.text)
                .To(vm => vm.Model.Vitality)
                .OneWay();
            bindingSet.Bind(_enduranceValue)
                .For(v => v.text)
                .To(vm => vm.Model.Endurance)
                .OneWay();
            bindingSet.Bind(_strengthValue)
                .For(v => v.text)
                .To(vm => vm.Model.Strength)
                .OneWay();
            bindingSet.Bind(_dexterityValue)
                .For(v => v.text)
                .To(vm => vm.Model.Dexterity)
                .OneWay();
            bindingSet.Bind(_intelligenceValue)
                .For(v => v.text)
                .To(vm => vm.Model.Intelligence)
                .OneWay();
            bindingSet.Bind(_faithValue)
                .For(v => v.text)
                .To(vm => vm.Model.Faith)
                .OneWay();
            bindingSet.Bind(_magicValue)
                .For(v => v.text)
                .To(vm => vm.Model.Magic)
                .OneWay();
        }
    }
}