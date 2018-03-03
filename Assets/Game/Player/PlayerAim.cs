using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField]
    private GameObject _lineRenderer;
    [SerializeField]
    private float _lineLength;

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
            print("aim start");
        }
        if (!_input.IsAiming && _active)
        {
            _active = false;
            print("aim end");
        }
    }
}
