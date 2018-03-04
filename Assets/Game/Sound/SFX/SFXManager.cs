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

        if (sound == null)
        {
            print("tried to play sound \'" + soundName + "\', no such sound exists");
            return;
        }

        if (sound.soundObject == null)
            return;

        PlaySound(sound.soundObject);
    }

    public void PlaySoundDelayed(string soundName, float delay)
    {
        StartCoroutine(PlaySoundDelayedCoroutine(soundName, delay));
    }

    private IEnumerator PlaySoundDelayedCoroutine(string soundName, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(soundName);
    }
}