using System;

namespace SL.Stats
{
    [Serializable]
    public class StatsModel
    {
        public const int MinValue = 1;
        public const int MaxValue = 99;

        public int Vitality = 1;
        public int Endurance = 1;
        public int Strength = 1;
        public int Dexterity = 1;
        public int Intelligence = 1;
        public int Faith = 1;
        public int Magic = 1;
    }
}
