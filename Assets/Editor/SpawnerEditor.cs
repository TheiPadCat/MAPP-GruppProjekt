using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects]
public class SpawnerEditor : Editor {

    private Dictionary<Type, SpawnableInfo> infoDict = new Dictionary<Type, SpawnableInfo>();

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DisplayResetAndLoad();
        DisplaySpawnableSettings();
    }

    private void ResetSettings() {
        infoDict.Clear();
        if (GUILayout.Button(new GUIContent("Reset All Spawner Settings", "Deletes all current settings object for each enemy type")))
            foreach (var enemySettings in AssetDatabase.FindAssets("", new[] { "Assets/Resources/SpawnerSettings/Current" }))
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(enemySettings));
    }

    private void LoadDefaultSettings() {
        if (GUILayout.Button(new GUIContent("Load Default Settings",
        "Will load any default settings created for each spawnable type. " +
        "Note: Any default settings object must be placed in Assets/Resources/SpawnerSettings/Default and Follow use the following naming scheme: TypeNameDefaultSettings"))) {
            Type type = typeof(Spawner);
            Assembly assembly = Assembly.GetAssembly(type);
            var spawnableTypes = assembly.GetTypes().Where(t => typeof(ISpawnable).IsAssignableFrom(t) && t != typeof(ISpawnable));

            SpawnableInfo currentSpawnInfo = null, temp = null;
            foreach (var spawnableType in spawnableTypes) {
                temp = Resources.Load($"SpawnerSettings/Default/{spawnableType.Name}DefaultSettings") as SpawnableInfo;
                if (!temp) continue;
                currentSpawnInfo = Resources.Load($"SpawnerSettings/Current/{spawnableType.Name}Settings") as SpawnableInfo;
                if (currentSpawnInfo) {
                    EditorUtility.CopySerialized(temp, currentSpawnInfo);
                    currentSpawnInfo.name = $"{spawnableType.Name}Settings";
                    AssetDatabase.SaveAssets();
                    infoDict.Add(spawnableType, currentSpawnInfo);
                } else CreateAndAddSettingsObject(spawnableType);
            }
        }

    }

    private void DisplayResetAndLoad() {
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        ResetSettings();
        LoadDefaultSettings();
        GUILayout.EndHorizontal();
    }

    private SpawnableInfo CreateAndAddSettingsObject(Type spawnableType) {
        SpawnableInfo currentSpawnInfo = ScriptableObject.CreateInstance<SpawnableInfo>();
        currentSpawnInfo.TypeName = spawnableType.Name;

        AssetDatabase.CreateAsset(currentSpawnInfo, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/SpawnerSettings/Current/{spawnableType.Name}Settings.asset"));
        AssetDatabase.SaveAssets();
        infoDict.Add(spawnableType, currentSpawnInfo);
        return currentSpawnInfo;
    }

    private void DisplaySpawnableSettings() {

        Type type = typeof(Spawner);
        Assembly assembly = Assembly.GetAssembly(type);
        var spawnableTypes = assembly.GetTypes().Where(t => typeof(ISpawnable).IsAssignableFrom(t) && t != typeof(ISpawnable));


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spawn Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Each type implementing the \"ISpawnable\" interface is listed below");
        EditorGUILayout.Space();

        SpawnableInfo currentSpawnInfo = null;
        foreach (var spawnableType in spawnableTypes) {

            // Check if already loaded to avoid loading from resources again
            if (infoDict.ContainsKey(spawnableType)) currentSpawnInfo = infoDict[spawnableType];

            // If not, check if settings object exists in resources
            else if (!currentSpawnInfo) {
                currentSpawnInfo = Resources.Load($"SpawnerSettings/Current/{spawnableType.Name}Settings") as SpawnableInfo;
                if (currentSpawnInfo) infoDict.Add(spawnableType, currentSpawnInfo);
            }

            // If not, create new settings object
            if (!currentSpawnInfo) currentSpawnInfo = CreateAndAddSettingsObject(spawnableType);

            EditorGUILayout.LabelField(spawnableType.Name, EditorStyles.boldLabel);

            currentSpawnInfo.InitialSpawnChance = EditorGUILayout.Slider(
                new GUIContent("Initial Spawn Chance", "A value of 0 means the spawnable item or enemy will never spawn, a value of 1 means it will spawn every time a roll is made by the spawner"),
                currentSpawnInfo.InitialSpawnChance, 0f, 1f);

            currentSpawnInfo.SpawnChanceIncreasePerRound =
                EditorGUILayout.Slider(new GUIContent("Spawn chance increase per round", "The number of percent the initial spawn chance should be multiplied by with every round"),
                    currentSpawnInfo.SpawnChanceIncreasePerRound, 0f, 1f);

            EditorGUILayout.BeginHorizontal();
            currentSpawnInfo.SpawnRate =
                Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Roll for spawn every", "How often (in seconds) the spawner should roll for a spawn the spawnable"), currentSpawnInfo.SpawnRate),
                    0, float.MaxValue);

            EditorGUILayout.LabelField("Seconds");
            EditorGUILayout.EndHorizontal();
            currentSpawnInfo.prefab = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Spawnable prefab", "Note: Make sure this prefab has a script of the same type the setting is for, or it will not accept it"),
                currentSpawnInfo.prefab, typeof(GameObject), false);

            // this is expensive, but it ensures only a prefab with the same type script as the current setting can be assigned
            if (currentSpawnInfo.prefab && currentSpawnInfo.prefab.GetComponent(spawnableType) == null && currentSpawnInfo.prefab.GetComponentInChildren(spawnableType) == null) {
                Debug.LogWarning($"\"{currentSpawnInfo.prefab.name}\" cannot be assigned to a spawnable of type {spawnableType.Name}, it, or its children does not have a script of that type attached!");
                currentSpawnInfo.prefab = null;
            }

            EditorGUILayout.Space(10f);
            AssetDatabase.SaveAssets();
            currentSpawnInfo = null;
        }
    }
}
