using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    public AudioSource bomb;
    public AudioSource split;
    public AudioSource enemy;
     public void Bomb(Vector2 v)
    {
        transform.position = v;
        bomb.pitch = UnityEngine.Random.Range(0.8f, 1f);
        bomb.Play();
    }

    public void Split(Vector2 v)
    {
        transform.position = v;
        split.pitch = UnityEngine.Random.Range(0.8f, 1f);
        split.Play();
    }

    public void Enemy(Vector2 v)
    {
        transform.position = v;
        enemy.pitch = UnityEngine.Random.Range(0.8f, 1f);
        enemy.Play();
    }
}
