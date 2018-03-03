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
    [SerializeField]
    private float _attackTime;

    private bool _isAttacking;
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
        if (_isAttacking)
            return;

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
        var targetRotation = (targetDirection == Vector3.zero) ? transform.rotation : Quaternion.LookRotation(targetDirection, Vector3.up);
        
        _lookRotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotateSensitivity);
        transform.rotation = _lookRotation;
    }

    private void Aim(Vector3 dir)
    {
        var lookAtPos = transform.position + dir;
        transform.LookAt(lookAtPos);
    }

    private void Melee()
    {
        if (_isAttacking || _isDashing)
            return;

        StartCoroutine(MeleeAttack());
    }

    private void Fire()
    {
        if (_isAttacking || _isDashing)
            return;

        StartCoroutine(FireAttack());
    }

    
    private IEnumerator MeleeAttack()
    {
        _isAttacking = true;
        print("melee!");
        Aim(_input.AimDirection);

        // cast from ~the middle of the player
        var ray = new Ray(transform.position + Vector3.up, _input.AimDirection);
        var hitObjects = Physics.SphereCastAll(ray, 1f) // this radius parameter doesn't seem to work :/
            .Where(h =>
            {
                return Vector3.Angle(_input.AimDirection, h.transform.position - transform.position) < _meleeConeAngle / 2f;
            })
            .Where(h =>
            {
                return Vector3.Distance(transform.position, h.transform.position) < _meleeDistance; // Hacky distance
            })
            .Select(h => h.collider.gameObject.GetComponent<Health>())
            .Where(h => h != null)
            .ToArray();

        foreach (var hit in hitObjects)
        {
            print(hit.gameObject);
            hit.gameObject.GetComponent<Health>().DoDamage();

        }
        yield return new WaitForSeconds(_attackTime);
        _isAttacking = false;
    }

    private IEnumerator FireAttack()
    {
        _isAttacking = true;
        print("fire!");
        Aim(_input.AimDirection);
        yield return new WaitForSeconds(_attackTime);
        _isAttacking = false;
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
        yield return new WaitForSeconds(duration);
        _isDashing = false;
    }
}
