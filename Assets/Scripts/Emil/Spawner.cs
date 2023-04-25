using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//Temp placement of these here for easier editing

[Serializable]
public abstract class Enemy : MonoBehaviour {
    public abstract void Attack();
    public abstract void TakeDamage();
    public abstract void Die();
}

public class Squid : Enemy {
    public override void Attack() {
        throw new NotImplementedException();
    }

    public override void Die() {
        throw new NotImplementedException();
    }

    public override void TakeDamage() {
        throw new NotImplementedException();
    }
}

public class Pirate : Enemy {
    public override void Attack() {
        throw new NotImplementedException();
    }

    public override void Die() {
        throw new NotImplementedException();
    }

    public override void TakeDamage() {
        throw new NotImplementedException();
    }
}


public class EnemyInfo : ScriptableObject {
    public Type EnemyType { get; set; }
    public string EnemyTypeName { get; set; }
    public float InitialSpawnChance;
    public float CurrentSpawnChance;
    public float Health;
    public float AttackDamage;

    public void Init() { EnemyType = Type.GetType(EnemyTypeName); }
}

public class Spawner : MonoBehaviour {

    public static Spawner Instance { get; private set; }
    public delegate void enemySpawned(Type enemyType);
    public static enemySpawned EnemySpawned; // subscribe to this event to do things when enemies spawn
    public readonly Dictionary<Type, EnemyInfo> enemyInfo = new Dictionary<Type, EnemyInfo>();

    // Start is called before the first frame update
    void Start() {
        Instance ??= this;
        LoadEnemySettings();
    }

    private void LoadEnemySettings() {
        Type enemyType = typeof(Enemy);
        Assembly assembly = Assembly.GetAssembly(enemyType);
        var enemySubTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(enemyType));
        EnemyInfo currentEnemyInfo = null;

        foreach (var enemySubtype in enemySubTypes) {
            currentEnemyInfo = Resources.Load($"EnemyInfo/{enemySubtype.Name}Settings") as EnemyInfo;
            if (!currentEnemyInfo) continue;
            enemyInfo.Add(enemySubtype, currentEnemyInfo);
            currentEnemyInfo.Init();
        }
    }

    // Update is called once per frame
    void Update() {
        foreach (var entry in enemyInfo) {
            if (UnityEngine.Random.Range(0f, 1f) <= entry.Value.InitialSpawnChance) print($"{entry.Value.EnemyType.Name} Spawned!");
        }
    }


    private void OnDestroy() { Instance = null; }
    private void OnDisable() { Instance = null; }
}

[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects]
public class SpawnerEditor : Editor {

    private Dictionary<Type, EnemyInfo> enemyInfoDict = new Dictionary<Type, EnemyInfo>();

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DisplayResetAndLoad();
        DisplayEnemySettings();
    }

    private void ResetSettings() {
        enemyInfoDict.Clear();
        if (GUILayout.Button(new GUIContent("Reset All Spawner Settings", "Deletes all current settings object for each enemy type")))
            foreach (var enemySettings in AssetDatabase.FindAssets("", new[] { "Assets/Resources/EnemyInfo" }))
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

    private void DisplayEnemySettings() {

        Type enemyType = typeof(Enemy);
        Assembly assembly = Assembly.GetAssembly(enemyType);
        var enemySubTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(enemyType));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Enemy Spawn Settings", EditorStyles.boldLabel);

        EnemyInfo currentEnemyInfo = null;
        foreach (var enemySubtype in enemySubTypes) {
            currentEnemyInfo = Resources.Load($"EnemyInfo/{enemySubtype.Name}Settings") as EnemyInfo;
            if (!currentEnemyInfo) {
                currentEnemyInfo = ScriptableObject.CreateInstance<EnemyInfo>();

                currentEnemyInfo.EnemyTypeName = enemySubtype.Name;

                AssetDatabase.CreateAsset(currentEnemyInfo, AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/EnemyInfo/{enemySubtype.Name}Settings.asset"));
                AssetDatabase.SaveAssets();
                enemyInfoDict.Add(enemySubtype, currentEnemyInfo);
            } else if (!enemyInfoDict.ContainsKey(enemySubtype)) enemyInfoDict.Add(enemySubtype, currentEnemyInfo);

            EditorGUILayout.LabelField(enemySubtype.Name, EditorStyles.boldLabel);
            currentEnemyInfo.InitialSpawnChance = EditorGUILayout.Slider(
                new GUIContent("Initial Spawn Chance", "A value of 0 means the enemy will never spawn, a value of 1 means it will spawn every time a roll is made by the spawner"),
                currentEnemyInfo.InitialSpawnChance, 0f, 1f);
            currentEnemyInfo.Health = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Enemy Health", "The amount of health point the enemy type has, clamped at a minimum of 1"),
            currentEnemyInfo.Health), 1f, float.MaxValue);
            currentEnemyInfo.AttackDamage = Mathf.Clamp(EditorGUILayout.FloatField("Attack Damage", currentEnemyInfo.AttackDamage), 0, float.MaxValue);
            EditorGUILayout.Space(10f);
        }
    }
}
