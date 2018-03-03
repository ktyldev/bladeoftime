using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float _dashSpeed;
    [SerializeField]
    private float _dashDuration = 1.8f;
    [SerializeField]
    [Range(0, 2)]
    private float _rotateSensitivity;
    [SerializeField]
    private float _meleeDistance;
    [SerializeField]
    private float _meleeConeAngle;

    private IControlMode _input;
    private Vector3 _momentum;
    private Quaternion _lookRotation;

    private bool _isDashing = false;
    private IEnumerator dashRoutine;

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
        var dir = _isDashing ? transform.forward :_input.MoveDirection;
        var speed = _isDashing ? _dashSpeed : _moveSpeed;
        _momentum = Vector3.Lerp(_momentum, dir, _moveSensitivity);
        transform.Translate(_momentum * speed * Time.deltaTime, Space.World);
        anim.SetFloat("inputV", dir != Vector3.zero ? _momentum.magnitude * _moveSpeed : 0);
    }

    private void Aim()
    {
        if (_isDashing)
            return;
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
        if (_isDashing)
            return;
        print("melee!");
    }

    private void Fire()
    {
        if (_isDashing)
            return;
        print("fire!");
    }

    private void Dash()
    {
        if (_isDashing)
            return;
        dashRoutine = DoDash(_dashDuration);
        StartCoroutine(dashRoutine);
        anim.SetTrigger("dash");
        print("dash!");
    }

    IEnumerator DoDash(float duration)
    {
        _isDashing = true;
        anim.SetBool("jump", true); // TODO: delete this
        yield return new WaitForSeconds(duration);
        anim.SetBool("jump", false); // TODO: delete this
        _isDashing = false;
    }
}
