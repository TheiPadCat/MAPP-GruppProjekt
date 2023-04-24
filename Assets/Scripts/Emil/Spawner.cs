using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

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
    public float InitialSpawnRate;
    public float CurrentSpawnRate;

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
            if (UnityEngine.Random.Range(0f, 1f) <= entry.Value.InitialSpawnRate) print($"{entry.Value.EnemyType.Name} Spawned!");
        }
    }


    private void OnDestroy() { Instance = null; }
    private void OnDisable() { Instance = null; }
}

[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects]
public class SpawnerEditor : Editor {

    private class EnemyEditorInfo {
        public float SpawnRate;
        public float Health;
        public float Damage;
    }

    private Dictionary<Type, EnemyEditorInfo> enemyEditorInfo = new Dictionary<Type, EnemyEditorInfo>();

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

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
                enemyEditorInfo.Add(enemySubtype, new EnemyEditorInfo() { SpawnRate = 0f });
            } else if (!enemyEditorInfo.ContainsKey(enemySubtype)) enemyEditorInfo.Add(enemySubtype, new EnemyEditorInfo() { SpawnRate = currentEnemyInfo.InitialSpawnRate });
            else enemyEditorInfo[enemySubtype].SpawnRate = currentEnemyInfo.InitialSpawnRate;

            EditorGUILayout.LabelField(enemySubtype.Name);
            enemyEditorInfo[enemySubtype].SpawnRate = EditorGUILayout.Slider("Initial Spawnrate", enemyEditorInfo[enemySubtype].SpawnRate, 0f, 1f);
            currentEnemyInfo.InitialSpawnRate = enemyEditorInfo[enemySubtype].SpawnRate;
        }
    }
}
