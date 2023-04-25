using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnableInfo : ScriptableObject {
    public Type SpawnableType { get; set; }
    public string TypeName { get; set; }
    public float InitialSpawnChance;
    public float CurrentSpawnChance { get; set; }
    public float SpawnChanceIncreasePerRound;
    public float SpawnRate;

    public void Init() { SpawnableType = Type.GetType(TypeName); }

}
