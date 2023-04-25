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

    public void Spawn() {
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

public interface ISpawnable {
    // All spawnable objects need to implement the Spawn method
    public abstract void Spawn();

    // Some spawnable objects may want to implement a Despawn method
    public void Despawn() { }
}

public class Spawner : MonoBehaviour {

    public static Spawner Instance { get; private set; }
    public delegate void objectSpawned(Type enemyType);
    public static objectSpawned ObjectSpawned; // subscribe to this event to do things when enemies spawn
    public readonly Dictionary<Type, SpawnableInfo> spawnInfo = new Dictionary<Type, SpawnableInfo>();

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
            currentSpawnableInfo = Resources.Load($"SpawnerSettings/{spawnableSubtype.Name}Settings") as SpawnableInfo;
            if (!currentSpawnableInfo) continue;
            spawnInfo.Add(spawnableSubtype, currentSpawnableInfo);
            currentSpawnableInfo.Init();
        }
    }

    // Update is called once per frame
    void Update() {
        foreach (var entry in spawnInfo) {
            if (UnityEngine.Random.Range(0f, 1f) <= entry.Value.InitialSpawnChance) print($"{entry.Value.SpawnableType.Name} Spawned!");
        }
    }

    private void OnDestroy() { Instance = null; }
    private void OnDisable() { Instance = null; }
}