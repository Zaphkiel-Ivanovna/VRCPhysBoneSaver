using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VRCPhysBoneSaver.Editor
{
    public static class PhysBoneSaverSettings
    {
        private const string ENABLE_PLAYMODE_SAVE_KEY = "VRCPhysBoneSaver.EnablePlayModeSave";

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Preferences/VRCPhysBoneSaver", SettingsScope.User)
            {
                label = "VRC PhysBone Saver",
                guiHandler = DrawSettingsInterface,
                keywords = new HashSet<string>(new[] { "VRC", "PhysBone", "Saver", "Settings" })
            };

            return provider;
        }

        private static void DrawSettingsInterface(string searchContext)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10); // Adjust this value to your desired spacing
            bool enablePlayModeSave = EditorPrefs.GetBool(ENABLE_PLAYMODE_SAVE_KEY, true);
            bool newEnablePlayModeSave = EditorGUILayout.Toggle("Enable PlayMode Save", enablePlayModeSave);
            GUILayout.EndHorizontal();

            if (enablePlayModeSave != newEnablePlayModeSave)
            {
                EditorPrefs.SetBool(ENABLE_PLAYMODE_SAVE_KEY, newEnablePlayModeSave);
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Made By Zaphkiel", "boldlabel"))
                    Application.OpenURL("https://github.com/Zaphkiel-Ivanovna");
            }
        }

    }
}
