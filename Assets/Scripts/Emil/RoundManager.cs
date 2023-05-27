using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoundManager : MonoBehaviour {

    public delegate void roundEnd(int roundNumber);
    public delegate void roundBegin(int roundNumber);
    public static roundEnd RoundEnd;
    public static roundBegin RoundBegin;
    public static RoundManager Instance;
    public CinemachineCameraShake camera;
    public int RoundNumber { get; set; }
    public int KillsRequired { get; private set; }
    public int KillsThisRound { get; private set; }
    private class KillInfo { public int NeededKills, PerformedKills; }
    private Dictionary<SpawnableInfo, KillInfo> killsThisRoundByType = new Dictionary<SpawnableInfo, KillInfo>();


    // Start is called before the first frame update
    void Start() {
        Instance ??= this;

        foreach (var spawnInfo in Spawner.Instance.spawnInfoRaw) {
            if (typeof(IEnemy).IsAssignableFrom(Type.GetType(spawnInfo.TypeName)))
                killsThisRoundByType.Add(spawnInfo, new KillInfo() { NeededKills = spawnInfo.NumberToAppearThisRound, PerformedKills = 0 });
        }

        RoundNumber = 1;
        RoundBegin += OnRoundBegin; // after the invoke so it doesn't run twice unecessarily
        RoundEnd += OnRoundEnd;
        IEnemy.Death += OnEnemyDeath;
        RoundBegin.Invoke(RoundNumber);
    }

    private bool IsRoundOver() {
        //Ändrade till detta för att lösa bugg med rundorna
        if(KillsThisRound < KillsRequired)
        {
            return false;
        }
        return true;

        //Old version
        /*
        foreach (var enemyType in killsThisRoundByType.Values) if (enemyType.NeededKills > enemyType.PerformedKills) return false;
        return true;
        */
    }

    private void OnEnemyDeath(Type enemyType, GameObject prefab) {
        // this is very ugly, but I don't see any other way of doing it with the current system
        foreach (var entry in killsThisRoundByType) {
            if (prefab.name == $"{entry.Key.prefab.name}(Clone)") {
                entry.Value.PerformedKills++;
                KillsThisRound++;
            }
        }
        if (IsRoundOver()) {
            RoundEnd.Invoke(RoundNumber);
            RoundNumber++;
        }
    }

    private void OnRoundBegin(int number) {
        camera.GetComponent<CinemachineCameraShake>().enabled = true;
        KillsThisRound = 0;
        KillsRequired = 0;
        foreach (var enemyType in killsThisRoundByType) {
            enemyType.Value.PerformedKills = 0;
            enemyType.Value.NeededKills = enemyType.Key.NumberToAppearThisRound;
            KillsRequired += enemyType.Value.NeededKills;
        }
    }

    private void OnRoundEnd(int number) {
        camera.GetComponent<CinemachineCameraShake>().enabled = false;
        print("Round " + number + " is over!");
    }

    private void OnDestroy() {
        Instance = null;
        RoundBegin -= OnRoundBegin;
        RoundEnd -= OnRoundEnd;
        IEnemy.Death -= OnEnemyDeath;
    }

    private void OnEnable() {
        Instance ??= this;
    }

    private void OnDisable() {
        Instance = null;
    }
}
