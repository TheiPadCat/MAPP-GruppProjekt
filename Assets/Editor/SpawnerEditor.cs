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

    private Dictionary<Type, SpawnableInfo> enemyInfoDict = new Dictionary<Type, SpawnableInfo>();

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DisplayResetAndLoad();
        DisplaySpawnableSettings();
    }

    private void ResetSettings() {
        enemyInfoDict.Clear();
        if (GUILayout.Button(new GUIContent("Reset All Spawner Settings", "Deletes all current settings object for each enemy type")))
            foreach (var enemySettings in AssetDatabase.FindAssets("", new[] { "Assets/Resources/SpawnerSettings" }))
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(enemySettings));
    }

    private void LoadDefaultSettings() {
        if (GUILayout.Button(new GUIContent("Load Default Settings",
        "Will load any default settings created for each spawnable type. " +
        "Note: Any default settings object must be placed in Assets/DefaultSettings and contain the same name as the spawnable type")))
            Debug.Log("Dummy");
    }

    private void DisplayResetAndLoad() {
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        ResetSettings();
        LoadDefaultSettings();
        GUILayout.EndHorizontal();
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
            currentSpawnInfo = Resources.Load($"SpawnerSettings/{spawnableType.Name}Settings") as SpawnableInfo;
            if (!currentSpawnInfo) {
                currentSpawnInfo = ScriptableObject.CreateInstance<SpawnableInfo>();

                currentSpawnInfo.TypeName = spawnableType.Name;

                AssetDatabase.CreateAsset(currentSpawnInfo, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/SpawnerSettings/{spawnableType.Name}Settings.asset"));
                AssetDatabase.SaveAssets();
                enemyInfoDict.Add(spawnableType, currentSpawnInfo);
            } else if (!enemyInfoDict.ContainsKey(spawnableType)) enemyInfoDict.Add(spawnableType, currentSpawnInfo);

            EditorGUILayout.LabelField($"{spawnableType.Name} ({spawnableType.BaseType})", EditorStyles.boldLabel);
            currentSpawnInfo.InitialSpawnChance = EditorGUILayout.Slider(
                new GUIContent("Initial Spawn Chance", "A value of 0 means the spawnable item or enemy will never spawn, a value of 1 means it will spawn every time a roll is made by the spawner"),
                currentSpawnInfo.InitialSpawnChance, 0f, 1f);

            currentSpawnInfo.SpawnChanceIncreasePerRound =
                EditorGUILayout.Slider(new GUIContent("Spwan chance increase per round", "The number of percent the initial spawn chance should be multiplied by with every round"),
                    currentSpawnInfo.SpawnChanceIncreasePerRound, 0f, 1f);

            EditorGUILayout.BeginHorizontal();
            currentSpawnInfo.SpawnRate =
                Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Roll for spawn every", "How often (in seconds) the spawner should roll for a spawn the spawnable"), currentSpawnInfo.SpawnRate),
                    0, float.MaxValue);

            EditorGUILayout.LabelField("Seconds");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10f);
        }
    }
}
