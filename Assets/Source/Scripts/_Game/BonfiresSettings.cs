using System;
using System.Collections.Generic;
using System.Linq;
using SL.Game.Bonfires;
using UnityEngine;
using Zenject;

namespace SL.Game
{
    [CreateAssetMenu(menuName = "Settings/Bonfires", fileName = "BonfiresSettings")]
    public class BonfiresSettings : ScriptableObject, IInitializable
    {
        [Serializable]
        private struct BonfireData
        {
            public string Id;
            public SpawnPoint SpawnPoint;
        }

        public SpawnPoint StartPoint;
        [SerializeField] private BonfireData[] _bonfireDatas;

        private Dictionary<string, SpawnPoint> _spawnPoints;

        public void Initialize()
            => _spawnPoints = _bonfireDatas.ToDictionary(data => data.Id, data => data.SpawnPoint);

        public SpawnPoint GetPoint(string bonfireId) =>
            string.IsNullOrEmpty(bonfireId)
                ? StartPoint
                : _spawnPoints[bonfireId];

        [ContextMenu(nameof(Fill))]
        private void Fill()
        {
           Bonfire[] bonfires = FindObjectsOfType<Bonfire>();

           _bonfireDatas = new BonfireData[bonfires.Length];
           for (int i = 0; i < bonfires.Length; i++)
           {
               _bonfireDatas[i].Id = bonfires[i].Id;
               _bonfireDatas[i].SpawnPoint = new SpawnPoint(bonfires[i].SpawnPoint);
           }
        }
    }
}