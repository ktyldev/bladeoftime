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

    public static float deltaTime { get {
            return Instance.DeltaTime;
        } }

    public static float TimeSpeed { get {
            return Instance._timeSpeed;
        } }

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
        if (GameOver.IsEnded())
        {
            _timeSpeed = Mathf.Lerp(_timeSpeed, Mathf.Max(_timeSpeed - .5f, 0f), 0.02f);
            if (_timeSpeed < 0.001f)
                _timeSpeed = 0f;
        }
        else
        {
            _timeSpeed = Mathf.Clamp(_timeSpeed + _timeIncreaseRate * Time.deltaTime, _minTime, _maxTime);
        }
    }

    protected void _slowTime(float amount)
    {
        if (GameOver.IsEnded())
            return;

        _timeSpeed = Mathf.Clamp(_timeSpeed - amount, _minTime, _maxTime);
    }

    public static void SlowTime(float amount)
    {
        if (GameOver.IsEnded())
            return;

        Instance._slowTime(amount);
    }
}
