using UnityEngine;

namespace SL.Stats
{
    public class StatsSaver
    {
        private const string StatsKey = nameof(StatsKey);

        public void Save(StatsModel stats)
        {
            PlayerPrefs.SetString(StatsKey, JsonUtility.ToJson(stats));
            PlayerPrefs.Save();
        }

        public StatsModel Load()
        {
            if (PlayerPrefs.HasKey(StatsKey))
                return JsonUtility.FromJson<StatsModel>(PlayerPrefs.GetString(StatsKey));

            return new StatsModel();
        }
    }
}