using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public interface ISpawnable {
    // All spawnable objects need to implement the Spawn method
    public abstract void Spawn();

    // Some spawnable objects may want to implement a Despawn method
    public virtual void Despawn() { }
}

public class Spawner : MonoBehaviour {

    public static Spawner Instance { get; private set; }
    public delegate void objectSpawned(Type enemyType);
    public static objectSpawned ObjectSpawned; // subscribe to this event to do things when enemies spawn
    public readonly Dictionary<Type, List<SpawnableInfo>> SpawnInfoByType = new Dictionary<Type, List<SpawnableInfo>>();
    public readonly HashSet<SpawnableInfo> spawnInfoRaw = new HashSet<SpawnableInfo>();


    private void Awake()
    {
        Instance ??= this;
        LoadSpawnerSettings();
    }
    // Start is called before the first frame update
    void Start() {
       // Instance ??= this;
        //DontDestroyOnLoad(Instance);
       // LoadSpawnerSettings();
    }
     
    private void LoadSpawnerSettings() {
        Type spawnableType = typeof(Spawner);
        Assembly assembly = Assembly.GetAssembly(spawnableType);
        var spawnableSubtypes = assembly.GetTypes().Where(t => typeof(ISpawnable).IsAssignableFrom(t) && t != typeof(ISpawnable));
        SpawnableInfo currentSpawnableInfo = null;

        foreach (var spawnableSubtype in spawnableSubtypes) {
            currentSpawnableInfo = Resources.Load<SpawnableInfo>($"SpawnerSettings/Current/{spawnableSubtype.Name}0Settings");
            if (!currentSpawnableInfo) continue;
            int index = 1;
            while (currentSpawnableInfo != null) {
                if (!SpawnInfoByType.ContainsKey(spawnableSubtype)) SpawnInfoByType.Add(spawnableSubtype, new List<SpawnableInfo>());
                SpawnInfoByType[spawnableSubtype].Add(currentSpawnableInfo);
                spawnInfoRaw.Add(currentSpawnableInfo);
                currentSpawnableInfo.Init();
                currentSpawnableInfo = Resources.Load<SpawnableInfo>($"SpawnerSettings/Current/{spawnableSubtype.Name}{index}Settings");
                index++;
            }

        }

    }

    private void OnDestroy() { Instance = null; }
    private void OnDisable() { Instance = null; }
}