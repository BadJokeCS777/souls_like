using Loxodon.Framework.Binding;
using Loxodon.Framework.Execution;
using SL.Common;
using UnityEngine;
using Zenject;

namespace SL.General.Player
{
    public class PlayerAnimatorView : MonoBehaviour
    {
        [SerializeField] private PlayerAnimatorBindings _animatorBindings;
        [SerializeField] private AnimationsSettings _settings;

        private PlayerAnimatorViewModel _viewModel;

        [Inject]
        private void Construct(ICoroutineExecutor coroutineExecutor, PlayerAnimatorModel model)
        {
            _viewModel = new PlayerAnimatorViewModel(_settings, coroutineExecutor, model);
            Bind();
        }

        private void Bind()
        {
            var bindingSet = this.CreateBindingSet(_viewModel);

            bindingSet.Bind(_animatorBindings)
                .For(v => v.Speed)
                .To(vm => vm.Speed);
            bindingSet.Bind(_animatorBindings)
                .For(v => v.IsGrounded)
                .To(vm => vm.Model.IsGrounded);
            bindingSet.Bind(_animatorBindings)
                .For(v => v.IsBonfireSitting)
                .To(vm => vm.Model.IsBonfireSitting);
            bindingSet.Bind(_animatorBindings)
                .For(v => v.DodgeValue)
                .To(vm => vm.Model.DodgeValue);
            bindingSet.Bind(_animatorBindings)
                .For(v => v.IsDodging)
                .To(vm => vm.IsDodging);

            bindingSet.Build();
        }
    }
}