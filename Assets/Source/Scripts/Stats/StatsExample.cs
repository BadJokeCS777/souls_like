using UnityEngine;
using Zenject;

namespace SL.Stats
{
    public class StatsExample : MonoBehaviour
    {
        private UI.Models.StatsModel _uiModel;
        private StatsModel _stats;
        private StatsSaver _saver;

        [Inject]
        private void Construct(UI.Models.StatsModel uiModel)
        {
            _uiModel = uiModel;
        }

        private void Start()
        {
            _saver = new StatsSaver();
            Load();
        }

        [ContextMenu(nameof(Load))]
        private void Load()
        {
            _stats = _saver.Load();
            _uiModel.Vitality = _stats.Vitality;
            _uiModel.Endurance = _stats.Endurance;
            _uiModel.Strength = _stats.Strength;
            _uiModel.Dexterity = _stats.Dexterity;
            _uiModel.Intelligence = _stats.Intelligence;
            _uiModel.Faith = _stats.Faith;
            _uiModel.Magic = _stats.Magic;
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
            _saver.Save(_stats);
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