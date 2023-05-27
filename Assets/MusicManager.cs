using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> musicLayers;
    public Island baseIsland;
    public float baseLowHealth = 0.25f;
    public float checkHealth = 0.5f;
    public float tempoIncrease = 1.5f;

    private AudioSource audioSource;
    private int currentLayerIndex = 0;
    private int loopCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = musicLayers[0];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= checkHealth)
        {
            checkHealth += checkHealth;
            CheckBaseHealth();
        }
    }

    void CheckBaseHealth()
    {
        float baseHealth = GetBaseHealth();

        if (baseHealth < baseLowHealth)
        {
            IncreaseTempo();
        }
        else
        {
            ResetTempo();
        }

        if (loopCount >= 2)
        {
            if (currentLayerIndex < musicLayers.Count - 1)
            {
                currentLayerIndex++;
                audioSource.clip = musicLayers[currentLayerIndex];
                audioSource.Play();
                loopCount = 0;
            }
        }

        if (currentLayerIndex == musicLayers.Count - 1 && loopCount >= 2)
        {
            audioSource.clip = CombineAudioClips(musicLayers);
            audioSource.Play();
        }

        loopCount++;
    }

    void IncreaseTempo()
    {
        audioSource.pitch = tempoIncrease;
    }

    void ResetTempo()
    {
        audioSource.pitch = 1.0f;
    }

    AudioClip CombineAudioClips(List<AudioClip> clips)
    {
        int totalLength = 0;
        foreach (AudioClip clip in clips)
        {
            totalLength += clip.samples;
        }

        float[] data = new float[totalLength];
        int offset = 0;

        foreach (AudioClip clip in clips)
        {
            float[] clipData = new float[clip.samples];
            clip.GetData(clipData, 0);
            clipData.CopyTo(data, offset);
            offset += clipData.Length;
        }

        AudioClip combinedClip = AudioClip.Create("Combined", totalLength, 1, audioSource.clip.frequency, false);
        combinedClip.SetData(data, 0);

        return combinedClip;
    }

    float GetBaseHealth()
    {
        return baseIsland.health;
    }
}