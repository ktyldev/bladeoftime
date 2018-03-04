using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _lineRenderer;
    [SerializeField]
    private float _lineLength;
    [SerializeField]
    private float _lineHeight;
    [SerializeField]
    private float _shootDistance;
    [SerializeField]
    private float _cooldownTime;
    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private int _shotChargeCost;
    [SerializeField]
    private float _totalCharge;
    [SerializeField]
    private float _chargeSpeed;
    
    public bool IsAttacking { get; private set; }
    public float CooldownPercent { get { return _currentCharge / _totalCharge; } }

    private LineRenderer _line;
    private IControlMode _input;
    private bool _active;
    private bool _onCooldown;
    private float _currentCharge;
    
    void Start()
    {
        _currentCharge = _totalCharge;
        _input = GetComponentInChildren<IControlMode>();
        _line = _lineRenderer.GetComponent<LineRenderer>();
        if (_line == null || _input == null)
            throw new Exception();
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
        if (_currentCharge < _totalCharge)
        {
            _currentCharge += WibblyWobbly.deltaTime * _chargeSpeed;
        }
    }

    public void Fire()
    {
        // TODO: Empty gun click
        if (_currentCharge < _shotChargeCost)
            return;
        
        Instantiate(_projectile, transform.position, transform.rotation);
        _currentCharge -= _shotChargeCost;
    }
}
