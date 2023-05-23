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

    private Dictionary<string, HashSet<SpawnableInfo>> infoDict;
    private Queue<Action> uiModificationsQueue = new Queue<Action>();
    private bool uiModificationsMade;

    private void OnEnable() {
        EditorApplication.update += OnEditorUpdate;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        LoadDictData();
    }
    private void OnDisable() {
        EditorApplication.update -= OnEditorUpdate;
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        SaveDictData();
    }

    private void OnPlayModeChanged(PlayModeStateChange stateChange) {
        switch (stateChange) {
            case PlayModeStateChange.EnteredPlayMode:
            case PlayModeStateChange.ExitingPlayMode:
                LoadDictData();
                break;
            case PlayModeStateChange.ExitingEditMode:
                SaveDictData();
                break;
        }
        /* if (stateChange == PlayModeStateChange.ExitingPlayMode || stateChange == PlayModeStateChange.EnteredPlayMode)
            LoadDictData(); */
    }

    private void LoadDictData() {
        DictionaryData data = Resources.Load("SpawnerSettings/DictData/CurrentDictData") as DictionaryData;
        infoDict = new Dictionary<string, HashSet<SpawnableInfo>>();
        if (data) {
            data.DeserializeData();
            foreach (var entry in data.data) infoDict.Add(entry.Key, new HashSet<SpawnableInfo>(entry.Value));
        }
    }

    private void SaveDictData() {
        DictionaryData data = Resources.Load("SpawnerSettings/DictData/CurrentDictData.asset") as DictionaryData;
        if (!data) {
            data = ScriptableObject.CreateInstance<DictionaryData>();
            AssetDatabase.CreateAsset(data, "Assets/Resources/SpawnerSettings/DictData/CurrentDictData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        foreach (var entry in infoDict) data.data.Add(entry.Key, new List<SpawnableInfo>(entry.Value));
        data.SerializeData();
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
    }

    private void OnEditorUpdate() {
        if (!uiModificationsMade) return;
        while (uiModificationsQueue.Count > 0) uiModificationsQueue.Dequeue().Invoke();
        uiModificationsMade = false;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DisplayResetAndLoad();
        DisplaySpawnableSettings();
    }

    private void ResetSettingsGUI() {
        if (GUILayout.Button(new GUIContent("Reset All Spawner Settings", "Deletes all current settings object for each enemy type")))
            ResetSettings(true);


    }

    private void ResetSettings(bool deleteDict = false) {
        if (infoDict != null) infoDict.Clear();
        foreach (var enemySettings in AssetDatabase.FindAssets("", new[] { "Assets/Resources/SpawnerSettings/Current" }))
            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(enemySettings));

        if (deleteDict) AssetDatabase.DeleteAsset("Assets/Resources/SpawnerSettings/DictData/CurrentDictData.asset");
    }

    private void LoadDefualtSettingsActual() {
        Type type = typeof(Spawner);
        Assembly assembly = Assembly.GetAssembly(type);
        var spawnableTypes = assembly.GetTypes().Where(t => typeof(ISpawnable).IsAssignableFrom(t) && t != typeof(ISpawnable));

        SpawnableInfo currentSpawnInfo = null, defaultSpawnInfo = null;
        foreach (var spawnableType in spawnableTypes) {
            string[] settings = AssetDatabase.FindAssets(spawnableType.Name, new[] { "Assets/Resources/SpawnerSettings/Default" });
            for (int i = 0; i < settings.Length; i++) {
                defaultSpawnInfo = Resources.Load($"SpawnerSettings/Default/{spawnableType.Name}{i}DefaultSettings") as SpawnableInfo;
                if (!defaultSpawnInfo) break;
                currentSpawnInfo = Resources.Load($"SpawnerSettings/Current/{spawnableType.Name}{i}Settings") as SpawnableInfo;
                if (!currentSpawnInfo) {
                    currentSpawnInfo = CreateInstance<SpawnableInfo>();
                    AssetDatabase.CreateAsset(currentSpawnInfo, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/SpawnerSettings/Current/{spawnableType.Name}{i}Settings.asset"));
                }
                EditorUtility.CopySerialized(defaultSpawnInfo, currentSpawnInfo);
                currentSpawnInfo.name = $"{spawnableType.Name}{i}Settings";
                AssetDatabase.SaveAssets();
                if (!infoDict.ContainsKey(spawnableType.Name)) infoDict.Add(spawnableType.Name, new HashSet<SpawnableInfo>());
                infoDict[spawnableType.Name].Add(currentSpawnInfo);
                // } else CreateAndAddSettingsObject(spawnableType, i, i != 0);
            }
        }
    }

    private void LoadDefaultSettings() {
        if (GUILayout.Button(new GUIContent("Load Default Settings",
        "Will load any default settings created for each spawnable type. " +
        "Note: Any default settings object must be placed in Assets/Resources/SpawnerSettings/Default and Follow use the following naming scheme: TypeNameDefaultSettings"))) {
            uiModificationsQueue.Enqueue(delegate { LoadDefualtSettingsActual(); });
            uiModificationsMade = true;
        }
    }

    private void DisplayResetAndLoad() {
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        ResetSettingsGUI();
        LoadDefaultSettings();
        GUILayout.EndHorizontal();
    }

    private SpawnableInfo CreateAndAddSettingsObject(Type spawnableType, int index, bool justAdding = false) {
        SpawnableInfo currentSpawnInfo = ScriptableObject.CreateInstance<SpawnableInfo>();
        currentSpawnInfo.TypeName = spawnableType.Name;

        AssetDatabase.CreateAsset(currentSpawnInfo, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/SpawnerSettings/Current/{spawnableType.Name}{index}Settings.asset"));
        AssetDatabase.SaveAssets();
        if (!justAdding) infoDict[spawnableType.Name] = new HashSet<SpawnableInfo>();
        infoDict[spawnableType.Name].Add(currentSpawnInfo);
        return currentSpawnInfo;
    }

    private void AddVariant(Type spawnableType, int index) {
        uiModificationsQueue.Enqueue(delegate { CreateAndAddSettingsObject(spawnableType, index + 1, true); });
        uiModificationsMade = true;
    }

    private void RemoveVariant(Type spawnableType, SpawnableInfo currentSpawnInfo, int index) {
        uiModificationsQueue.Enqueue(delegate {
            infoDict[spawnableType.Name].Remove(currentSpawnInfo);
            AssetDatabase.DeleteAsset($"Assets/Resources/SpawnerSettings/Current/{spawnableType.Name}{index}Settings.asset");
        });
        uiModificationsMade = true;
    }

    private void DisplaySpawnableSettings() {

        Type type = typeof(Spawner);
        Assembly assembly = Assembly.GetAssembly(type);
        var spawnableTypes = assembly.GetTypes().Where(t => typeof(ISpawnable).IsAssignableFrom(t) && t != typeof(ISpawnable));


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spawn Settings", EditorStyles.whiteLargeLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Each type implementing the \"ISpawnable\" interface is listed below");
        EditorGUILayout.Space();

        SpawnableInfo currentSpawnInfo = null;
        foreach (var spawnableType in spawnableTypes) {
            for (int i = 0; !infoDict.ContainsKey(spawnableType.Name) ? i < 1 : i < infoDict[spawnableType.Name].Count; i++) {

                // If not, check if settings object exists in resources
                if (!currentSpawnInfo) {
                    currentSpawnInfo = Resources.Load($"SpawnerSettings/Current/{spawnableType.Name}{i}Settings") as SpawnableInfo;
                    if (currentSpawnInfo) {
                        if (!infoDict.ContainsKey(spawnableType.Name)) infoDict.Add(spawnableType.Name, new HashSet<SpawnableInfo>());
                        infoDict[spawnableType.Name].Add(currentSpawnInfo);
                    }
                }

                // If not, create new settings object
                if (!currentSpawnInfo) {
                    currentSpawnInfo =
                         CreateAndAddSettingsObject(spawnableType, infoDict.ContainsKey(spawnableType.Name) ? i + 1 : i, infoDict.ContainsKey(spawnableType.Name));
                }

                // have to do this here in case it's not created via the method above
                currentSpawnInfo.TypeName = spawnableType.Name;

                if (i == 0) EditorGUILayout.LabelField(spawnableType.Name, EditorStyles.whiteLargeLabel);
                else EditorGUILayout.LabelField($"{spawnableType.Name} variant {i}", EditorStyles.boldLabel);

                currentSpawnInfo.InitialSpawnChance = EditorGUILayout.Slider(
                    new GUIContent("Initial Spawn Chance", "A value of 0 means the spawnable item or enemy will never spawn, a value of 1 means it will spawn every time a roll is made by the spawner"),
                    currentSpawnInfo.InitialSpawnChance, 0f, 1f);

                currentSpawnInfo.SpawnChanceIncreasePerRound =
                    EditorGUILayout.Slider(new GUIContent("Spawn chance in- or decrease per round", "The number of percent the initial spawn chance should be multiplied by with every round"),
                        currentSpawnInfo.SpawnChanceIncreasePerRound, -1f, 1f);

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

                currentSpawnInfo.NumberToAppearFirstRound =
                    Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Number on initial round", "The number of the spawnable type to appear on it's first round. Must be at least one" +
                    ". This number will incerase with the percentage set for spawn chance increase"),
                    currentSpawnInfo.NumberToAppearFirstRound), 1, int.MaxValue);

                currentSpawnInfo.StartRound = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Appears first on round", "The round number that the spawnable will start spawning on"),
                currentSpawnInfo.StartRound), 1, int.MaxValue);

                currentSpawnInfo.EndRound = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Stops appearing on round", "The round the spawnable type stops spawning on, leave at 0 for never"),
                currentSpawnInfo.EndRound), 0, int.MaxValue);

                currentSpawnInfo.spawnArea = EditorGUILayout.Vector2Field(new GUIContent("Spawn area min & max", "These values should represent in which sub-square of the map square the spawnable should spawn. For example -60, 60"), currentSpawnInfo.spawnArea);

                bool shouldDisplay = i == infoDict[spawnableType.Name].Count - 1 && i != 0;

                EditorGUILayout.BeginHorizontal();
                if (shouldDisplay || infoDict[spawnableType.Name].Count == 1) { if (GUILayout.Button("Add variant")) AddVariant(spawnableType, i); }
                if (shouldDisplay) { if (GUILayout.Button("Remove variant")) RemoveVariant(spawnableType, currentSpawnInfo, i); }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(10f);
                AssetDatabase.SaveAssets();
                currentSpawnInfo = null;
            }
        }
    }
}
