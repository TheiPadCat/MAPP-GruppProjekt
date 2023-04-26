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
    public int RoundNumber { get; private set; }

    // Start is called before the first frame update
    void Start() {
        Instance ??= this;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDestroy() {
        Instance = null;
    }

    private void OnEnable() {
        Instance ??= this;
    }

    private void OnDisable() {
        Instance = null;
    }
}
