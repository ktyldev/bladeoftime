﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject _controlMode;
    [SerializeField]
    [Range(0, 1)]
    private float _moveSensitivity;
    [SerializeField]
    private float _moveSpeed;

    private IControlMode _input;
    private Vector3 _momentum;
    
    void Start()
    {
        _input = Instantiate(_controlMode)
            .GetComponent<IControlMode>();
        
        if (_input == null)
            throw new System.Exception();

        _input.Fire.AddListener(() =>
        {
            print("fire!");
        });

        _input.Melee.AddListener(() =>
        {
            print("melee!");
        });
    }
    
    void Update()
    {
        Aim();
        Move();
    }

    private void Move()
    {
        var dir = _input.MoveDirection;
        _momentum = Vector3.Lerp(_momentum, dir, _moveSensitivity);
        transform.Translate(_momentum * _moveSpeed * Time.deltaTime);
    }

    private void Aim()
    {
        var dir = _input.AimDirection;
    }
}
