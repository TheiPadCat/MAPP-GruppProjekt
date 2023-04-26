using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpawnerSettings/SpawnableInfo")]
public class SpawnableInfo : ScriptableObject {
    public Type SpawnableType { get; set; }
    public string TypeName { get; set; }
    public float InitialSpawnChance;
    public float CurrentSpawnChance { get; set; }
    public float SpawnChanceIncreasePerRound;
    public float SpawnRate;
    public GameObject prefab;
    private ISpawnable spawnable;

    private IEnumerator Instantiator() {
        while (true) {
            if (UnityEngine.Random.Range(0f, 1f) <= CurrentSpawnChance) {
                GameObject temp = Instantiate(prefab) as GameObject;
                //Spawner.ObjectSpawned.Invoke(SpawnableType);
                spawnable.Spawn();
            }
            yield return new WaitForSecondsRealtime(SpawnRate);
        }
    }

    public void Init() {
        // subscribe to new and end round events
        SpawnableType = Type.GetType(TypeName);
        CurrentSpawnChance = InitialSpawnChance;
        if (prefab){
            spawnable = prefab.GetComponent<ISpawnable>();
            if(spawnable == null) spawnable = prefab.GetComponentInChildren<ISpawnable>();
        } 
        OnNewRound(1);
    }

    private void OnNewRound(int roundNumber) {
        Spawner.Instance.StartCoroutine(Instantiator());
        CurrentSpawnChance = InitialSpawnChance * Mathf.Pow(1f + SpawnChanceIncreasePerRound, roundNumber);
    }

    private void OnRoundEnd() {
        Spawner.Instance.StopCoroutine(Instantiator());
    }

}
