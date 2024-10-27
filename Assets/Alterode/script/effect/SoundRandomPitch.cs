using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomPitch : MonoBehaviour
{
    AudioSource audioSource;
    float originPitch = 1;
    float range = 0.2f;

    void Awake()
    {
        if(audioSource is null)
        {
            audioSource = GetComponent<AudioSource>();
            originPitch = audioSource.pitch;
        }
    }

    void OnEnable()
    {
        audioSource.pitch = Random.Range(originPitch-range,originPitch+range);
    }
}
