using UnityEngine;

namespace SL.Game
{
    public class HealthChanger : MonoBehaviour
    {
        [SerializeField] private float _value;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHealthOwner healthOwner))
                healthOwner.ApplyDamage(_value);
        }
    }
}