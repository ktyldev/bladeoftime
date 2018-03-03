using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    public Animator anim;
    [SerializeField]
    private GameObject _controlMode;
    [SerializeField]
    [Range(0, 1)]
    private float _moveSensitivity;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    [Range(0, 2)]
    private float _rotateSensitivity;

    private IControlMode _input;
    private Vector3 _momentum;

    private Quaternion _lookRotation;

    void Start()
    {
        GetComponent<Health>().Death.AddListener(() =>
        {
            print("game over!");
        });


        _input = Instantiate(_controlMode)
            .GetComponent<IControlMode>();

        if (_input == null)
            throw new System.Exception();

        _input.Melee.AddListener(Melee);
        _input.Fire.AddListener(Fire);
        _input.Dash.AddListener(Dash);
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
        transform.Translate(_momentum * _moveSpeed * Time.deltaTime, Space.World);
        anim.SetFloat("inputV", dir != Vector3.zero ? _momentum.magnitude * _moveSpeed : 0);
        // TODO: Remove this
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }

    }

    private void Aim()
    {
        bool isStill = _input.MoveDirection == Vector3.zero;
        var targetDirection = isStill ? _input.AimDirection : _input.MoveDirection;
        var targetRotation = (targetDirection == Vector3.zero) ?
            transform.rotation :
            Quaternion.LookRotation(targetDirection, Vector3.up);

        _lookRotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotateSensitivity);
        transform.rotation = _lookRotation;
    }


    private void Melee()
    {
        print("melee!");
    }

    private void Fire()
    {
        print("fire!");
    }

    private void Dash()
    {
        anim.SetTrigger("dash");
        print("dash!");
    }
}
