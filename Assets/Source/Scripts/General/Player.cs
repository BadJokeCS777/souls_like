using SL.Movement;
using UnityEngine;

namespace SL.General
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour
    {
        public void Init(Transform cameraTransform)
        {
            GetComponent<IMovement>().Init(cameraTransform);
        }
    }
}