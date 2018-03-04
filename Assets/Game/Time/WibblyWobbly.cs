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
    [SerializeField]
    private float _startTimeRate;

    [SerializeField]
    [Range(0f, 2f)]
    private float _gameOverReduction = .5f;
    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float _gameOverLerp = 0.03f;

    public static float deltaTime { get {
            return Instance.DeltaTime;
        } }

    public static float TimeSpeed { get {
            return Instance._timeSpeed;
        } }

    private static WibblyWobbly Instance { get; set; }

    private float DeltaTime { get { return Time.deltaTime * _timeSpeed; } }
    private float _timeSpeed;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
        _timeSpeed = _startTimeRate;
    }

    void Update()
    {
        if (GameOver.IsEnded())
        {
            _timeSpeed = Mathf.Lerp(_timeSpeed, Mathf.Max(_timeSpeed - _gameOverReduction, 0f), _gameOverLerp);
            if (_timeSpeed < 0.01f)
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
