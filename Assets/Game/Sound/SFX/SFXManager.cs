using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string soundName;
    public GameObject soundObject;
}

public class SFXManager : AudioManager
{
    public List<SoundEffect> sounds;
    protected override string _VolumeKey
    {
        get
        {
            return GameTags.SFXVolume;
        }
    }

    public void PlaySound(GameObject sound)
    {
        var audio = Instantiate(sound, transform)
            .GetComponent<AudioSource>();

        audio.volume *= Volume;
        audio.Play();
    }

    public void PlaySound(string soundName)
    {
        var sound = sounds.SingleOrDefault(o => o.soundName == soundName);
        PlaySound(sound.soundObject);
    }
}