using Loxodon.Framework.Observables;
using SL.Stats;
using UnityEngine;
using Zenject;

namespace SL.UI.Models.Stats
{
    public class StatsExample : MonoBehaviour
    {
        private StatsModel _uiModel;
        private SL.Stats.StatsModel _stats;
        private StatsStorage _storage;

        [Inject]
        private void Construct(StatsModel uiModel, StatsStorage storage)
        {
            _uiModel = uiModel;
            _storage = storage;
        }

        private void Start()
        {
            Load();

            foreach (string id in _storage.StatsIds)
                _uiModel.Stats.Add(id);
        }

        [ContextMenu(nameof(Load))]
        private void Load()
        {
            _stats = StatsSaver.Load();
            _uiModel.Vitality = _stats.Vitality;
            _uiModel.Endurance = _stats.Endurance;
            _uiModel.Strength = _stats.Strength;
            _uiModel.Dexterity = _stats.Dexterity;
            _uiModel.Intelligence = _stats.Intelligence;
            _uiModel.Faith = _stats.Faith;
            _uiModel.Magic = _stats.Magic;

            var statsList = StatsSaver.LoadList();
            foreach (StatModel statModel in statsList)
                _storage.Get(statModel.Name).Value = statModel.Value;
        }

        [ContextMenu(nameof(Save))]
        private void Save()
        {
            _stats.Vitality = _uiModel.Vitality;
            _stats.Endurance = _uiModel.Endurance;
            _stats.Strength = _uiModel.Strength;
            _stats.Dexterity = _uiModel.Dexterity;
            _stats.Intelligence = _uiModel.Intelligence;
            _stats.Faith = _uiModel.Faith;
            _stats.Magic = _uiModel.Magic;
            StatsSaver.Save(_stats);

            _storage.Save();
        }

        [ContextMenu(nameof(IncreaseAllStats))]
        private void IncreaseAllStats()
        {
            _uiModel.Vitality++;
            _uiModel.Endurance++;
            _uiModel.Strength++;
            _uiModel.Dexterity++;
            _uiModel.Intelligence++;
            _uiModel.Faith++;
            _uiModel.Magic++;
        }

        [ContextMenu(nameof(DecreaseAllStats))]
        private void DecreaseAllStats()
        {
            _uiModel.Vitality--;
            _uiModel.Endurance--;
            _uiModel.Strength--;
            _uiModel.Dexterity--;
            _uiModel.Intelligence--;
            _uiModel.Faith--;
            _uiModel.Magic--;
        }
    }
}