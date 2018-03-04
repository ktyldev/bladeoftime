using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    [Range(0f, 1f)]
    private float sensitivity = 0.3f;
    [SerializeField]
    private float zoomSensitivity;
    [SerializeField]
    private float _maxShakeMagnitude;

    private Vector3 velocity = Vector3.zero;
    private Vector3 _dampOffset;
    //public Material postFXMaterial;

    private Transform _trackedObject;
    private static CameraController Instance { get; set; }
    private Camera _cam;
    private Vector3 _targetPosition;
    private float _shakeTimeLeft;

    private float _targetFOV;

    void Start() {
        var player = GameObject.FindGameObjectWithTag(GameTags.Player);
        _trackedObject = player.transform;
        _dampOffset = transform.position - _trackedObject.transform.position;

        _cam = GetComponent<Camera>();
        _targetFOV = _cam.fieldOfView;

        Instance = this;
    }

    void Update()
    {
        if (_shakeTimeLeft > 0)
        {
            _shakeTimeLeft -= Time.deltaTime;
        }
    }

    void OnGUI()
    {
        if (_shakeTimeLeft > 0)
        {
            StartCoroutine(Shake());
        }
    }

    public void ShakeForSeconds(float seconds)
    {
        _shakeTimeLeft = seconds;
    }

    private IEnumerator Shake()
    {
        var elapsed = 0f;

        var duration = .1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            var shakeValue = _maxShakeMagnitude;

            var _shakeOffset = new Vector3(
                shakeValue * (UnityEngine.Random.value - 0.5f), 
                0, 
                shakeValue * (UnityEngine.Random.value - 0.5f));
            
            transform.position = _targetPosition + _shakeOffset;

            yield return new WaitForEndOfFrame();
        }
    }

    void LateUpdate() {
        if (_trackedObject == null)
            return;

        _targetPosition = Vector3.SmoothDamp(
            transform.position,
            _trackedObject.transform.position + _dampOffset,
            ref velocity,
            sensitivity);

        transform.position = _targetPosition;

        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _targetFOV, 0.02f);
    }

    public static void Zoom(float value)
    {
        Instance._targetFOV = value;
    }

    public static void ZoomMultiply(float multiplier)
    {
        Instance._targetFOV *= multiplier;
    }

    public static void Pulse()
    {
        Instance._cam.fieldOfView *= .9f;
    }
}