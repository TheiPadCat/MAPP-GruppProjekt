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
    public readonly Dictionary<Type, SpawnableInfo> SpawnInfo = new Dictionary<Type, SpawnableInfo>();


    // Start is called before the first frame update
    void Start() {
        Instance ??= this;
        LoadSpawnerSettings();
    }

    private void LoadSpawnerSettings() {
        Type spawnableType = typeof(Spawner);
        Assembly assembly = Assembly.GetAssembly(spawnableType);
        var spawnableSubtypes = assembly.GetTypes().Where(t => typeof(ISpawnable).IsAssignableFrom(t) && t != typeof(ISpawnable));
        SpawnableInfo currentSpawnableInfo = null;

        foreach (var spawnableSubtype in spawnableSubtypes) {
            currentSpawnableInfo = Resources.Load<SpawnableInfo>($"SpawnerSettings/Current/{spawnableSubtype.Name}Settings");
            if (!currentSpawnableInfo) continue;
            SpawnInfo.Add(spawnableSubtype, currentSpawnableInfo);
            currentSpawnableInfo.Init();
        }
    }

    private void OnDestroy() { Instance = null; }
    private void OnDisable() { Instance = null; }
}