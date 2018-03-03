using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    [Range(0f, 1f)]
    private float sensitivity = 0.3f;

    private Vector3 _offset;
    //public Material postFXMaterial;

    private Transform _trackedObject;

    void Start() {
        var player = GameObject.FindGameObjectWithTag(GameTags.Player);
        _trackedObject = player.transform;
        _offset = transform.position - _trackedObject.transform.position;
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
        transform.position = Vector3.Lerp(transform.position, targetPosition, sensitivity);
    }
}