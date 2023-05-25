using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpawnerSettings/SpawnableInfo")]
public class SpawnableInfo : ScriptableObject {
    public Type SpawnableType { get; set; }
    [HideInInspector] public string TypeName;
    public float InitialSpawnChance;
    public float CurrentSpawnChance { get; set; }
    public float SpawnChanceIncreasePerRound;
    public float SpawnRate;
    private float currentSpawnRate;
    public int NumberToAppearFirstRound, StartRound, EndRound;
    public int NumberToAppearThisRound { get; set; }
    public GameObject prefab;
    public Vector2 spawnArea;
    private ISpawnable spawnable;
    private Coroutine instantiator;
    private Vector3 spawnPos;
    private string[] layerNames = new string[] { "Wall", "Player" };
    private List<GameObject> spawnedThisRound = new List<GameObject>();

    private IEnumerator Instantiator() {
        while (true) {
            if (spawnedThisRound.Count < NumberToAppearThisRound && UnityEngine.Random.Range(0f, 1f) <= CurrentSpawnChance) {
                // ensures stuff doesn't spawn in walls
                // TODO Make spawning more dynamic than random
                spawnPos = new Vector3(UnityEngine.Random.Range(spawnArea.x, spawnArea.y), UnityEngine.Random.Range(spawnArea.x, spawnArea.y), 0);
                while (Physics2D.OverlapBox(spawnPos, Vector2.one, 0f, LayerMask.GetMask(layerNames)) != null)
                    spawnPos = new Vector3(UnityEngine.Random.Range(spawnArea.x, spawnArea.y), UnityEngine.Random.Range(spawnArea.x, spawnArea.y), 0);

                GameObject temp = Instantiate(prefab, spawnPos, Quaternion.identity) as GameObject;
                temp.name = prefab.name + "(Clone)";
                if (Spawner.ObjectSpawned != null) Spawner.ObjectSpawned.Invoke(SpawnableType);
                spawnable.Spawn();
                spawnedThisRound.Add(temp);
            }
            yield return new WaitForSecondsRealtime(currentSpawnRate);
            Debug.Log("Spawned this round: " + spawnedThisRound.Count);
        }
    }

    public void Init() {
        RoundManager.RoundBegin += OnNewRound;
        RoundManager.RoundEnd += OnRoundEnd;
        spawnedThisRound.Clear();
        currentSpawnRate = SpawnRate;

        SpawnableType = Type.GetType(TypeName);
        CurrentSpawnChance = InitialSpawnChance;
        SetNumberToAppearThisRound(1);
        if (prefab) {
            spawnable = prefab.GetComponent<ISpawnable>();
            if (spawnable == null) spawnable = prefab.transform.root.GetComponentInChildren<ISpawnable>();
        }
    }

    private void SetNumberToAppearThisRound(int round, float multiplier = 1f) {
        if (StartRound <= round && (EndRound == 0 || EndRound > round)) NumberToAppearThisRound = (int)(NumberToAppearFirstRound * multiplier);
        else if (StartRound > round || (EndRound != 0 && EndRound <= round)) NumberToAppearThisRound = 0;
    }

    private void OnNewRound(int round) { instantiator = Spawner.Instance.StartCoroutine(Instantiator()); }

    private void OnRoundEnd(int round) {
        foreach (var spawned in spawnedThisRound) if (spawned) Destroy(spawned); // This should not be needed, but somehow the spawner manages to spawn + 1 more than it should
        spawnedThisRound.Clear();
        Spawner.Instance.StopCoroutine(instantiator);
        float multiplier = Mathf.Pow(1f + SpawnChanceIncreasePerRound, round + 1 - (StartRound - 1));

        CurrentSpawnChance = Mathf.Clamp(InitialSpawnChance * multiplier, InitialSpawnChance * 0.25f, InitialSpawnChance * 1.75f);
        SetNumberToAppearThisRound(round + 1, multiplier);

        currentSpawnRate = Mathf.Clamp(SpawnRate * Mathf.Pow(1f + (SpawnChanceIncreasePerRound * -1), round + 1 - (StartRound - 1)), SpawnRate * 0.25f, SpawnRate * 1.75f);
    }

}
