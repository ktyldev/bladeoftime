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
    private Vector3 _offset;
    //public Material postFXMaterial;

    private Transform _trackedObject;
    private static CameraController Instance { get; set; }
    private Camera _cam;
    private float _shakeTimeLeft;

    private float _targetFOV;

    void Start() {
        var player = GameObject.FindGameObjectWithTag(GameTags.Player);
        _trackedObject = player.transform;
        _offset = transform.position - _trackedObject.transform.position;

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
        Func<Vector3> originalPos = () => _trackedObject.transform.position + _offset;

        var elapsed = 0f;

        var duration = .1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            var returnPos = originalPos();

            var shakeValue = _maxShakeMagnitude;

            var x = returnPos.x + (shakeValue * (UnityEngine.Random.value - 0.5f));
            var z = returnPos.z + (shakeValue * (UnityEngine.Random.value - 0.5f));

            transform.position = new Vector3(x, returnPos.y, z);

            yield return null;
        }

        transform.position = originalPos();
    }


    void LateUpdate() {
        if (_trackedObject == null)
            return;

        var targetPosition = _trackedObject.transform.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, sensitivity);

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