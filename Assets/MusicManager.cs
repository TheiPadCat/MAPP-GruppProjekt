using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
   
    public Island baseIsland;
    public List<AudioSource> musikLager;

    public AudioSource audio1;
    public AudioSource audio2;
    public AudioSource audio3;
    public AudioSource audio4;
    public AudioSource audio5;
    public AudioSource audio6;
    public AudioSource audio7;

    public float baseLowHealth = 10f;
    public float checkHealth = 0.5f;
    public int lagerIncreasePerRunda = 1;
    public int lagerIntervall = 5; //diskutera med gruppen sen
    public float tempoIncrease = 1.5f; //ingen aning om detta kommer räcka men får testa sen

    //private AudioSource audioSource;
    private int nuvarandeLagerIndex = 0;
    private int rundorSedanLastLager = 0;
    private float nextCheckTime = 0f;
    private float basTempo = 1.0f;
    private int antalRundor = 0;
    public RoundManager rounds;


    // Start is called before the first frame update
    void Start()
    {


        audio1.Play();audio1.loop = true;
        audio2.Play();audio2.loop = true;
        audio3.Play();audio3.loop = true;
        audio4.Play(); audio4.loop = true;
        audio5.Play(); audio5.loop = true;
        audio6.Play(); audio6.loop = true;
        audio7.Play(); audio7.loop = true; 
        antalRundor = rounds.RoundNumber;
        audio2.mute = true;
        audio3.mute = true;
        audio4.mute = true;
        audio5.mute = true;
        audio6.mute = true;
        audio7.mute = true;
        
    }

    // Update is called once per frame
    void Update()
    {
            CheckBaseHealth();
        

    }

    void CheckBaseHealth()
    {
        float baseHealth = baseIsland.health;
        antalRundor = rounds.RoundNumber;

        Debug.Log(antalRundor);
        if(antalRundor < 2)
        {
            audio2.mute = false;
        }

        if(antalRundor < 4 && antalRundor > 2)
        {
            audio3.mute = false;
        }

        if(antalRundor < 6 && antalRundor > 4)
        {
            audio4.mute = false;
        }
        
        if(antalRundor < 8 && antalRundor > 6)
        {
            audio5.mute = false;
        }
        if(antalRundor < 10 && antalRundor > 8)
        {
            audio6.mute = false;
        }
        if(antalRundor < 12 && antalRundor > 10)
        {
            audio7.mute = false;
        }
        if (baseHealth < baseLowHealth)
        {
            audio1.pitch = basTempo * tempoIncrease;
            audio2.pitch = basTempo * tempoIncrease;
            audio3.pitch = basTempo * tempoIncrease;
            audio4.pitch = basTempo * tempoIncrease;
            audio5.pitch = basTempo * tempoIncrease;
            audio6.pitch = basTempo * tempoIncrease;
            audio7.pitch = basTempo * tempoIncrease;

        }
        else
        {
            audio1.pitch = basTempo;
            audio2.pitch = basTempo;
            audio3.pitch = basTempo;
            audio4.pitch = basTempo;
            audio5.pitch = basTempo;
            audio6.pitch = basTempo;
            audio7.pitch = basTempo;
        }

    }

    float GetBaseHealt()
    {
        return baseIsland.health;
    }
}
