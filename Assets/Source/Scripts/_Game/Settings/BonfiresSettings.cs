using System.Collections.Generic;
using System.Linq;
using SL.Common;
using SL.Game.Bonfires;
using UnityEngine;
using Zenject;

namespace SL.Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/Bonfires", fileName = "BonfiresSettings")]
    public class BonfiresSettings : ScriptableObject, IInitializable
    {
        private const string BonfireRoot = "BonfireRoot";

        [SerializeField] private SpawnPoint _startPoint;
        [SerializeField] private BonfireData[] _bonfireDatas;

        private Dictionary<string, SpawnPoint> _spawnPoints;

        public void Initialize()
            => _spawnPoints = _bonfireDatas.ToDictionary(data => data.Id, data => data.SpawnPoint);

        public SpawnPoint GetPoint(string bonfireId) =>
            string.IsNullOrEmpty(bonfireId)
                ? _startPoint
                : _spawnPoints[bonfireId];

#if UNITY_EDITOR
        [ContextMenu(nameof(Fill))]
        public void Fill()
        {
            Bonfire[] bonfires = FindObjectsByType<Bonfire>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            _bonfireDatas = new BonfireData[bonfires.Length];
            for (var i = 0; i < bonfires.Length; i++)
            {
                _bonfireDatas[i].Id = bonfires[i].Id;
                _bonfireDatas[i].SpawnPoint = new SpawnPoint(bonfires[i].SpawnPoint);
            }
        }

        [ContextMenu(nameof(Load))]
        public void Load(string prefabPath)
        {
            var bonfirePrefab = Resources.Load<Bonfire>(prefabPath);
            var parent = (GameObject.Find(BonfireRoot) ?? new GameObject(BonfireRoot)).transform;

            foreach (BonfireData data in _bonfireDatas)
            {
                var spawnPoint = data.SpawnPoint;
                var rotation = Quaternion.Euler(spawnPoint.Rotation);
                var bonfire = Instantiate(bonfirePrefab, spawnPoint.Position, rotation, parent);
                bonfire.Id = data.Id;
            }
        }
#endif
    }
}