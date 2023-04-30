using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip backgrunsMusik;
    public List<AudioClip> musikLager;
    public float baseLowHealth = 0.25f;
    public float checkHealth = 0.5f;
    public int lagerIncreasePerRunda = 1;
    public int lagerIntervall = 5; //diskutera med gruppen sen
    public float tempoIncrease = 1.5f; //ingen aning om detta kommer räcka men får testa sen

    private AudioSource audioSource;
    private int nuvarandeLagerIndex = 0;
    private int rundorSedanLastLager = 0;
    private float nextCheckTime = 0f;
    private float basTempo = 1.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgrunsMusik;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
