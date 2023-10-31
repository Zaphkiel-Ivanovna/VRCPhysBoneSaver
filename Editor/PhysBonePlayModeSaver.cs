using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace VRCPhysBoneSaver.Editor
{
    [InitializeOnLoad]
    public static class PhysBonePlayModeSaver
    {
        private const string PREFERENCE_KEY = "VRCPhysBoneSaver.EnablePlayModeSave";
        private const string KEY_PREFIX = "ScenePresetManagement";

        static PhysBonePlayModeSaver()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange playModeState)
        {
            if (!EditorPrefs.GetBool(PREFERENCE_KEY, true)) return;

            switch (playModeState)
            {
                case PlayModeStateChange.EnteredEditMode:
                    LoadPhysBoneComponents();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    SavePhysBoneComponents();
                    break;
            }
        }

        private static void LoadPhysBoneComponents()
        {
            LoadComponentState<VRCPhysBone>();
            LoadComponentState<VRCPhysBoneCollider>();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private static void SavePhysBoneComponents()
        {
            SaveComponentState<VRCPhysBone>();
            SaveComponentState<VRCPhysBoneCollider>();
        }

        private static void LoadComponentState<T>() where T : Component
        {
            var components = Object.FindObjectsOfType<T>();
            foreach (var component in components)
            {
                var prefKey = $"{KEY_PREFIX}.{component.GetInstanceID()}";
                if (!EditorPrefs.HasKey(prefKey)) continue;

                var componentData = EditorPrefs.GetString(prefKey);
                JsonUtility.FromJsonOverwrite(componentData, component);

                EditorPrefs.DeleteKey(prefKey);
                EditorUtility.SetDirty(component);
            }
        }

        private static void SaveComponentState<T>() where T : Component
        {
            var components = Object.FindObjectsOfType<T>();
            foreach (var component in components)
            {
                var prefKey = $"{KEY_PREFIX}.{component.GetInstanceID()}";
                var componentData = JsonUtility.ToJson(component);
                EditorPrefs.SetString(prefKey, componentData);
            }
        }
    }
}
