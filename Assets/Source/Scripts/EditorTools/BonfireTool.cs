using SL.Game.Bonfires;
using SL.Game.Settings;
using UnityEditor;
using UnityEngine;

namespace SL.EditorTools
{
    public class BonfireTool
    {
        private const string SettingsPath = "Settings/Bonfires/BonfiresSettings";
        private const string PrefabPath = "Objects/Bonfire";

        [MenuItem("SL/Bonfires/Fill")]
        public static void Fill()
        {
            var settings = Resources.Load<BonfiresSettings>(SettingsPath);
            settings.Fill();
        }

        [MenuItem("SL/Bonfires/Load")]
        public static void Load()
        {
            var settings = Resources.Load<BonfiresSettings>(SettingsPath);
            settings.Load(PrefabPath);
        }
    }
}
