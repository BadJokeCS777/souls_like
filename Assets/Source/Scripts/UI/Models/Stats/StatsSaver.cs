using System.Collections.Generic;
using SL.UI.Models;
using UnityEngine;

namespace SL.Stats
{
    public class StatsSaver
    {
        private const string StatsKey = nameof(StatsKey);
        private const string StatsListKey = nameof(StatsListKey);

        public static void Save(StatsModel stats)
        {
            PlayerPrefs.SetString(StatsKey, JsonUtility.ToJson(stats));
            PlayerPrefs.Save();
        }

        public static StatsModel Load()
        {
            if (PlayerPrefs.HasKey(StatsKey))
                return JsonUtility.FromJson<StatsModel>(PlayerPrefs.GetString(StatsKey));

            return new StatsModel();
        }

        public static void Save(List<StatModel> stats)
        {
            PlayerPrefs.SetString(StatsListKey, JsonUtility.ToJson(stats));
            PlayerPrefs.Save();
        }

        public static List<StatModel> LoadList()
        {
            if (PlayerPrefs.HasKey(StatsListKey))
                return JsonUtility.FromJson<List<StatModel>>(PlayerPrefs.GetString(StatsKey));

            return new List<StatModel>
            {
                new(StatsConsts.Vitality),
                new(StatsConsts.Endurance),
                new(StatsConsts.Strength),
                new(StatsConsts.Dexterity),
                new(StatsConsts.Intelligence),
                new(StatsConsts.Faith),
                new(StatsConsts.Magic),
            };
        }
    }
}