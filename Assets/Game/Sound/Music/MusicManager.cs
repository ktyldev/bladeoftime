using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : AudioManager {
    
    protected override string _VolumeKey
    {
        get
        {
            return GameTags.MusicVolume;
        }
    }

    public GameObject loop;
    private AudioSource _loop;
    
    void Start() {
        _loop = Instantiate(loop, transform).GetComponent<AudioSource>();
        _loop.Play();
    }

    void OnGUI() {
        _loop.volume = Volume;
    }
}