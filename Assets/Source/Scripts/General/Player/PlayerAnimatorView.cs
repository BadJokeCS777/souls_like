using Loxodon.Framework.Binding;
using Loxodon.Framework.Execution;
using SL.Common;
using UnityEngine;
using Zenject;

namespace SL.General.Player
{
    public class PlayerAnimatorView : MonoBehaviour
    {
        //TODO: move weight to player stats
        //TODO: move all to config
        [SerializeField, Min(0f)] private float _weight = 0.25f;
        [SerializeField] private float _movingSpeed = 1f;
        [SerializeField, Min(0f)] private float _movingChangeDuration = 0.1f;
        [SerializeField] private PlayerAnimatorBindings _animatorBindings;

        private PlayerAnimatorViewModel _viewModel;

        [Inject]
        private void Construct(ICoroutineExecutor coroutineExecutor, PlayerAnimatorModel model)
        {
            _viewModel = new PlayerAnimatorViewModel(_weight, _movingSpeed, _movingChangeDuration, coroutineExecutor, model);
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