using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject _controlMode;

    private IControlMode _input;

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
    }

    private void Aim()
    {
        var dir = _input.AimDirection;
    }
}
