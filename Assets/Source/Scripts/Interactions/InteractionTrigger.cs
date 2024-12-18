using SL.Common;
using SL.Signals;
using UnityEngine;

namespace SL.Interactions
{
    public class InteractionTrigger : MessengerBehaviour
    {
        private const float StraightAngle = 90f;

        [SerializeField] private string _text;
        [SerializeField] private Sprite _buttonIcon;
        [SerializeField] private string _buttonText;
        [SerializeField, Range(0f, 180f)] private float _lookingAngle;

        private float _lookingValue;
        private Transform _interactorTransform;

        public bool IsInteractorLooking
        {
            get
            {
                if (_interactorTransform == null)
                    return false;

                Vector3 direction = (transform.position - _interactorTransform.position).normalized;
                float dot = Vector3.Dot(_interactorTransform.forward, direction);
                return dot > _lookingValue;
            }
        }

        private void Start()
        {
            _lookingValue = (StraightAngle - _lookingAngle) / StraightAngle;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Interactor interactor) == false)
                return;

            _interactorTransform = interactor.transform;
            PublishShowMessage();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Interactor>() == null)
                return;

            _interactorTransform = null;
            Publish(new HideInteractionMessage());
        }

        private void PublishShowMessage()
        {
            if (_buttonIcon != null)
                Publish(new ShowInteractionMessage(this, _text, _buttonIcon));
            else if (string.IsNullOrEmpty(_buttonText) == false)
                Publish(new ShowInteractionMessage(this, _text, _buttonText));
        }
    }
}