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

    private IControlMode _input;
    private Vector3 _momentum;

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
        if (_isAttacking)
            return;

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
        Aim(_input.MoveDirection != Vector3.zero ? _input.MoveDirection : _input.AimDirection);
    }

    private void Aim(Vector3 dir)
    {
        var lookAtPos = transform.position + dir;
        transform.LookAt(lookAtPos);
    }
    
    private void Melee()
    {
        if (_isAttacking)
            return;

        StartCoroutine(MeleeAttack());
    }
    
    private void Fire()
    {
        if (_isAttacking)
            return;

        StartCoroutine(FireAttack());
    }

    private float _attackLength = 0.5f;
    private bool _isAttacking;
    private IEnumerator MeleeAttack()
    {
        _isAttacking = true;
        print("melee!");
        Aim(_input.AimDirection);
        yield return new WaitForSeconds(_attackLength);
        _isAttacking = false;
    }
    
    private IEnumerator FireAttack()
    {
        _isAttacking = true;
        print("fire!");
        Aim(_input.AimDirection);
        yield return new WaitForSeconds(_attackLength);
        _isAttacking = false;
    }

    private void Dash()
    {
        anim.SetTrigger("dash");
        print("dash!");
    }
}
