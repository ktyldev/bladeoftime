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
    [SerializeField]
    private float filterFreq = 400f;

    private float _targetFreq;
    private GameObject _loopInstance;
    private AudioSource _loop;
    private bool _isFiltered = false;
    private AudioLowPassFilter _filter;

    private static MusicManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
    }

    void Start() {
        _loopInstance = Instantiate(loop, transform);
        _loop = _loopInstance.GetComponent<AudioSource>();
        _filter = _loopInstance.GetComponent<AudioLowPassFilter>();
        _targetFreq = _filter.cutoffFrequency;
        _loop.Play();
    }

    void Update()
    {
        _filter.cutoffFrequency = Mathf.Lerp(_filter.cutoffFrequency, _targetFreq, 0.05f);
    }

    void OnGUI() {
        _loop.volume = Volume;
    }

    public bool Filter {
        get { return _isFiltered; }
        set
        {
            if (value == _isFiltered || _filter == null)
                return;
            _isFiltered = !_isFiltered;

            _targetFreq = (_isFiltered) ? filterFreq : 22000f;
        }
    }

    public static void SetFilter(bool value)
    {
        Instance.Filter = value;
    }
}