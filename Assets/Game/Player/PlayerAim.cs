using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField]
    private GameObject _lineRenderer;
    [SerializeField]
    private float _lineLength;
    [SerializeField]
    private float _lineHeight;

    private LineRenderer _line;
    private IControlMode _input;
    private bool _active;

    void Start()
    {
        _input = GetComponentInChildren<IControlMode>();
        _line = _lineRenderer.GetComponent<LineRenderer>();
        if (_line == null || _input == null)
            throw new System.Exception();
    }

    void Update()
    {
        if (_input.IsAiming && !_active)
        {
            _active = true;
        }
        if (!_input.IsAiming && _active)
        {
            _active = false;
        }

        if (_active)
        {
            var start = transform.position + Vector3.up * _lineHeight;
            var end = start + _input.AimDirection * _lineLength;
            _line.SetPositions(new[] { start, end });
        }
        else
        {
            _line.SetPositions(new [] { Vector3.zero, Vector3.zero });
        }
    }
}
