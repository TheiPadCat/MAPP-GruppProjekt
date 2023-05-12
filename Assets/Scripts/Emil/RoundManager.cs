using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    public delegate void roundEnd(int roundNumber);
    public delegate void roundBegin(int roundNumber);
    public static roundEnd RoundEnd;
    public static roundBegin RoundBegin;
    public static RoundManager Instance;
    public int RoundNumber { get; set; }
    public int KillsRequired { get; private set; }
    public int KillsThisRound { get; private set; }
    private class KillInfo { public int NeededKills, PerformedKills; }
    private Dictionary<Type, KillInfo> killsThisRoundByType = new Dictionary<Type, KillInfo>();


    // Start is called before the first frame update
    void Start() {
        Instance ??= this;

        foreach (var spawnableType in Spawner.Instance.SpawnInfo.Keys)
            if (typeof(IEnemy).IsAssignableFrom(spawnableType))
                killsThisRoundByType.Add(spawnableType, new KillInfo() { NeededKills = Spawner.Instance.SpawnInfo[spawnableType].NumberToAppearThisRound, PerformedKills = 0 });


        RoundNumber = 1;
        RoundBegin += OnRoundBegin; // after the invoke so it doesn't run twice unecessarily
        RoundEnd += OnRoundEnd;
        IEnemy.Death += OnEnemyDeath; 
        RoundBegin.Invoke(RoundNumber);
    }

    private bool IsRoundOver() {
        foreach (var enemyType in killsThisRoundByType.Values) if (enemyType.NeededKills != enemyType.PerformedKills) return false;
        return true;
    }

    private void OnEnemyDeath(Type enemyType) {
        killsThisRoundByType[enemyType].PerformedKills++;
        KillsThisRound++;
        if (IsRoundOver()) {
            RoundEnd.Invoke(RoundNumber);
            RoundNumber++;
        }
    }

    private void OnRoundBegin(int number) {
        KillsThisRound = 0;
        KillsRequired = 0;
        foreach (var enemyType in killsThisRoundByType) {
            enemyType.Value.PerformedKills = 0;
            enemyType.Value.NeededKills = Spawner.Instance.SpawnInfo[enemyType.Key].NumberToAppearThisRound;
            KillsRequired += enemyType.Value.NeededKills;
        }
    }

    private void OnRoundEnd(int number) {
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
