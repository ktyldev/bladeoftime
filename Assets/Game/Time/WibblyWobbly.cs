using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WibblyWobbly : MonoBehaviour
{
    // Time speed
    [SerializeField]
    private float _minTime; 
    [SerializeField]
    private float _maxTime;
    [SerializeField]
    private float _timeIncreaseRate;

    public static float deltaTime { get { return Instance.DeltaTime; } }
    private static WibblyWobbly Instance { get; set; }

    private float DeltaTime { get { return Time.deltaTime * _timeSpeed; } }
    private float _timeSpeed = 1;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
    }
    
    void Update()
    {
        _timeSpeed += _timeIncreaseRate * Time.deltaTime;
    }
}
