using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using Debug = UnityEngine.Debug;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one AudioManager in the scene");
        }
        instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    
    public EventInstance CreateInstance(EventReference eventReference, Vector3 worldPos)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.set3DAttributes(worldPos.To3DAttributes());
        return eventInstance;
    }
}
