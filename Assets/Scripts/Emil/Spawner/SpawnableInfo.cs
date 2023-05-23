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
    private int activeInstances, roundNumber;
    private Coroutine instantiator;
    private Vector3 spawnPos;
    private string[] layerNames = new string[] { "Wall", "Player" };

    private IEnumerator Instantiator() {

        while (true) {
            roundNumber = RoundManager.Instance.RoundNumber;
            if (roundNumber >= StartRound && (roundNumber <= EndRound || EndRound == 0) &&
                activeInstances < NumberToAppearThisRound && UnityEngine.Random.Range(0f, 1f) <= CurrentSpawnChance) {

                // ensures stuff doesn't spawn in walls
                // TODO, add option to change size of random area. Make spawning more dynamic than random
                spawnPos = new Vector3(UnityEngine.Random.Range(spawnArea.x, spawnArea.y), UnityEngine.Random.Range(spawnArea.x, spawnArea.y), 0);
                while (Physics2D.OverlapBox(spawnPos, Vector2.one, 0f, LayerMask.GetMask(layerNames)) != null)
                    spawnPos = new Vector3(UnityEngine.Random.Range(spawnArea.x, spawnArea.y), UnityEngine.Random.Range(spawnArea.x, spawnArea.y), 0);

                GameObject temp = Instantiate(prefab, spawnPos, Quaternion.identity) as GameObject;
                temp.name = prefab.name + "(Clone)";
                if (Spawner.ObjectSpawned != null) Spawner.ObjectSpawned.Invoke(SpawnableType);
                spawnable.Spawn();
                activeInstances++;
            }
            yield return new WaitForSecondsRealtime(currentSpawnRate);
        }
    }

    public void Init() {
        RoundManager.RoundBegin += OnNewRound;
        RoundManager.RoundEnd += OnRoundEnd;
        activeInstances = 0;
        currentSpawnRate = SpawnRate;

        SpawnableType = Type.GetType(TypeName);
        CurrentSpawnChance = InitialSpawnChance;
        NumberToAppearThisRound = StartRound == 1 ? NumberToAppearFirstRound : 0;
        if (prefab) {
            spawnable = prefab.GetComponent<ISpawnable>();
            if (spawnable == null) spawnable = prefab.transform.root.GetComponentInChildren<ISpawnable>();
        }
    }

    private void OnNewRound(int roundNumber) { instantiator = Spawner.Instance.StartCoroutine(Instantiator()); }

    private void OnRoundEnd(int roundNumber) {
        activeInstances = 0;
        Spawner.Instance.StopCoroutine(instantiator);
        float multiplier = Mathf.Pow(1f + SpawnChanceIncreasePerRound, roundNumber + 1 - (StartRound - 1));

        CurrentSpawnChance = Mathf.Clamp(InitialSpawnChance * multiplier, InitialSpawnChance * 0.25f, InitialSpawnChance * 1.75f);
        NumberToAppearThisRound = StartRound < roundNumber + 1 ? (int)(NumberToAppearFirstRound * multiplier) : NumberToAppearFirstRound;
        currentSpawnRate = Mathf.Clamp(SpawnRate * Mathf.Pow(1f + (SpawnChanceIncreasePerRound * -1), roundNumber + 1 - (StartRound - 1)), SpawnRate * 0.25f, SpawnRate * 1.75f);
    }

}
