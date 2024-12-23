using Loxodon.Framework.Views;
using UnityEngine;

namespace SL.UI.Views
{
    public class WindowManger : MonoBehaviour
    {
        [SerializeField] private Window _stats;

        [ContextMenu(nameof(ShowStats))]
        public void ShowStats()
        {
            if (_stats.Created == false)
                _stats.Create();
            _stats.Show();
        }
    }
}