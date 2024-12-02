using Loxodon.Framework.Messaging;
using SL.Signals;
using UnityEngine;
using Zenject;

namespace AnimationActions
{
    public class StandUpBehaviour : StateMachineBehaviour
    {
        private IMessenger _messenger;

        [Inject]
        private void Construct(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _messenger.Publish(new RestorePlayerMessage());
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _messenger.Publish(new EnableMovementMessage());
        }
    }
}
