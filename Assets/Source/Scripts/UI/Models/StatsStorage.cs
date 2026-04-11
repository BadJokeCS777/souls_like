using System.Collections.Generic;
using System.Linq;
using SL.Stats;
using Zenject;

namespace SL.UI.Models
{
    public class StatsStorage : IInitializable
    {
        private readonly Dictionary<string, StatModel> _statsDict = new();

        public List<string> StatsIds => _statsDict.Keys.ToList();

        public void Initialize()
        {
            var list = StatsSaver.LoadList();
            foreach (StatModel stat in list)
                _statsDict.Add(stat.Name, new StatModel
                {
                    Name = stat.Name,
                    Icon = stat.Icon,
                    Value = stat.Value
                });
        }

        public StatModel Get(string name)
            => _statsDict.ContainsKey(name) ? _statsDict[name] : null;

        public void Save()
            => StatsSaver.Save(_statsDict.Values.ToList());
    }
}