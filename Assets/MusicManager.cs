using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip backgrunsMusik;
    public Island baseIsland; 
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
        if(Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkHealth;
            CheckBaseHealth();
        }
    }

    void CheckBaseHealth()
    {
        float baseHealth = baseIsland.health;
        int önskatLagerIndex = Mathf.FloorToInt(baseHealth / baseLowHealth) * lagerIncreasePerRunda;

        if(önskatLagerIndex > nuvarandeLagerIndex && önskatLagerIndex < musikLager.Count)
        {
            nuvarandeLagerIndex = önskatLagerIndex;
            audioSource.clip = musikLager[nuvarandeLagerIndex];
            audioSource.Play();
        }

        rundorSedanLastLager++;

        if(rundorSedanLastLager >= lagerIntervall)
        {
            rundorSedanLastLager = 0;
            if(nuvarandeLagerIndex < musikLager.Count - 1)
            {
                nuvarandeLagerIndex++;
                audioSource.clip = musikLager[nuvarandeLagerIndex];
                audioSource.Play();
            }
        }

        if (baseHealth < baseLowHealth)
        {
            audioSource.pitch = basTempo * lagerIncreasePerRunda;

            if(nuvarandeLagerIndex < musikLager.Count - 1)
            {
                nuvarandeLagerIndex++;
                audioSource.clip = musikLager[nuvarandeLagerIndex];
                audioSource.Play();
            }
        }
        else
        {
            audioSource.pitch = basTempo;
        }

    }

    float GetBaseHealt()
    {
        return baseIsland.health;
    }
}
