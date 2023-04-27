using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spawn : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private bool random;
    [SerializeField] int despawnTime;


    public void SpawnPrefab() {
        if (random) {
            float x = Random.Range(-spawnPosition.x, spawnPosition.x);
            float y = Random.Range(-spawnPosition.y, spawnPosition.y);
            GameObject turret = Instantiate(prefab, new Vector2(x, y), Quaternion.identity);
            StartCoroutine(WaitBeforeDelete(despawnTime, turret));
        } else {

            GameObject turret = Instantiate(prefab, spawnPosition, Quaternion.identity);
            StartCoroutine(WaitBeforeDelete(2, turret));
        }

    }
    IEnumerator WaitBeforeDelete(int seconds, Object instantiatedTurret) {
        yield return new WaitForSeconds(seconds);
        Destroy(instantiatedTurret);

    }

    void Start() {
        StartCoroutine(SpawnTimer(1.5f));
    }

    IEnumerator SpawnTimer(float seconds) {
        while (true) {
            yield return new WaitForSeconds(seconds);
            SpawnPrefab();
        }
    }
}
