using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    public AudioSource bomb;
    public AudioSource split;
    public AudioSource enemy;
     public void Bomb()
    {
        bomb.pitch = UnityEngine.Random.Range(0.8f, 1f);
        bomb.Play();
    }

    public void Split()
    {
        split.pitch = UnityEngine.Random.Range(0.8f, 1f);
        split.Play();
    }

    public void Enemy()
    {
        enemy.pitch = UnityEngine.Random.Range(0.8f, 1f);
        enemy.Play();
    }
}
