using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Music
{
    public GameObject intro;
    public GameObject loop;
}

public class MusicManager : AudioManager {
    
    protected override string _VolumeKey
    {
        get
        {
            return GameTags.MusicVolume;
        }
    }

    [SerializeField]
    private float filterFreq = 400f;

    [SerializeField]
    private Music[] music;
    private Music _music;

    private GameObject _intro;
    private GameObject _loop;

    private AudioSource _introSource;
    private AudioSource _loopSource;

    private float _targetFreq;
    private float _currentFreq;

    private float _targetPitch;
    private float _currentPitch;

    private AudioLowPassFilter _filter;

    private bool _isFiltered = false;

    private static MusicManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
        _music = music[Random.Range(0, music.Length)];

        _intro = Instantiate(_music.intro, transform);
        _loop = Instantiate(_music.loop, transform);

        _introSource = _intro.GetComponent<AudioSource>();
        _loopSource = _loop.GetComponent<AudioSource>();

        _filter = _intro.GetComponent<AudioLowPassFilter>();

        var introLength = _introSource.clip.length;

        _introSource.Play();
    }

    void Start() {
        _targetFreq = _filter.cutoffFrequency;
        _targetPitch = _introSource.pitch;
    }

    void Update()
    {
        _currentFreq = Mathf.Lerp(_currentFreq, _targetFreq, 0.05f);
        _currentPitch = Mathf.Lerp(_currentPitch, _targetPitch, 0.05f);

        var _currSound = (_intro == null) ? _loopSource : _introSource;
        _currSound.pitch = _currentPitch;
        _filter.cutoffFrequency = _currentFreq;
    }

    private void FixedUpdate()
    {
        if (!_loopSource.isPlaying && !_introSource.isPlaying )
        {
            Destroy(_intro);
            _loopSource.Play();
            _filter = _loop.GetComponent<AudioLowPassFilter>();
        }
    }

    public bool Filter {
        get { return _isFiltered; }
        set
        {
            if (value == _isFiltered || _filter == null)
                return;
            _isFiltered = !_isFiltered;

            _targetFreq = (_isFiltered) ? filterFreq : 22000f;
            _targetPitch = (_isFiltered) ? .85f : 1f;
        }
    }

    public static void SetFilter(bool value)
    {
        Instance.Filter = value;
    }

    public static void FadeOut()
    {
        Instance._targetFreq = 0f;
        Instance._targetPitch = 0f;
    }
}