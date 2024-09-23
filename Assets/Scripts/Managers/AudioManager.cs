using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two singleton failure");
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    /////////////////////////////////////
    //Play a sound or song///////////////
    /////////////////////////////////////
    //Call AudioManager.Play("soundName")
    /////////////////////////////////////
    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if(s == null)
        {
            Debug.Log("Sound: " + soundName + " not Found!");
            return;
        }
        s.source.Play();
    }

    /////////////////////////////////////
    //Stop a sound or song///////////////
    /////////////////////////////////////
    //Call AudioManager.Stop("soundName")
    /////////////////////////////////////
    
    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not Found!");
            return;
        }
        s.source.Stop();
    }
}