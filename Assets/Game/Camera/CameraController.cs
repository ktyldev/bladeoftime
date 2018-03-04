using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    [Range(0f, 1f)]
    private float sensitivity = 0.3f;
    [SerializeField]
    private float zoomSensitivity;

    private Vector3 velocity = Vector3.zero;
    private Vector3 _offset;
    //public Material postFXMaterial;

    private Transform _trackedObject;
    private static CameraController Instance { get; set; }
    private Camera _cam;

    private float _targetFOV;

    void Start() {
        var player = GameObject.FindGameObjectWithTag(GameTags.Player);
        _trackedObject = player.transform;
        _offset = transform.position - _trackedObject.transform.position;

        _cam = GetComponent<Camera>();
        _targetFOV = _cam.fieldOfView;

        Instance = this;
    }

    /*
    void OnRenderImage (RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, postFXMaterial);
    }
    */

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