using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

[RequireComponent(typeof(Health), typeof(PlayerShoot), typeof(PlayerMelee))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;
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
    private float _attackTime;
    [SerializeField]
    [Range(0, 1)]
    private float _attackMoment;
    [Range(0, 1)]
    [SerializeField]
    private float _acceleration;
    [Range(0, 0.1f)]
    [SerializeField]
    private float _deceleration;
    [SerializeField]
    private float _shootDistance;
    [SerializeField]
    private float _baseGunCooldown; // This is affected by wibbly wobbly time

    // I want to be sick
    private RotationZeroer _zeroer;

    private IControlMode _input;
    private Vector3 _momentum;
    private SFXManager _sfx;
    private PlayerShoot _gun;
    private PlayerMelee _melee;
    private float _currentMoveSpeed;

    private bool _isDashing = false;
    private bool _hasCooldown = false;

    private bool _isBusy { get { return (_melee.IsAttacking || _isDashing || _hasCooldown); } }
    public IControlMode Input {
        get { return _input; }
    }

    private void Awake()
    {
        _input = Instantiate(_controlMode, transform)
            .GetComponent<IControlMode>();
    }

    void Start()
    {
        _sfx = this.Find<SFXManager>(GameTags.Sound);
        _gun = GetComponent<PlayerShoot>();
        _melee = GetComponent<PlayerMelee>();

        _zeroer = GetComponentInChildren<RotationZeroer>();
        if (_zeroer == null)
            throw new System.Exception("Everything's fucked :/");

        GetComponent<Health>().Death.AddListener(() =>
        {
            print("game over!");
            _anim.SetFloat("inputV", 0f);
            _anim.SetTrigger("deathAnimation");
            StartCoroutine(DoDeathZoom());
        });

        GetComponent<Health>().Hit.AddListener(() =>
        {
            _sfx.PlayRandomSound("hurt", 4);
            _anim.SetFloat("inputV", 0f);
            StartCoroutine(DoDamagedHit());
        });

        if (_input == null)
            throw new System.Exception();

        _input.Melee.AddListener(Melee);
        _input.Fire.AddListener(Fire);
        _input.Dash.AddListener(Dash);
    }

    void Update()
    {
        if (GameOver.IsEnded())
            return;

        Move();

        if (_isBusy)
            return;

        Aim();
    }

    private void Move()
    {
        Vector3 dir;
        if (_isDashing)
        {
            dir = transform.forward;
            _currentMoveSpeed = _dashSpeed;
        }
        else
        {
            dir = _input.MoveDirection;
            if (_melee.IsAttacking || _input.IsAiming)
            {
                _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, 0, _deceleration);
            }
            else // Moving normally
            {
                _anim.SetBool("isAttacking", false);
                // fucking fight me
                if (_zeroer.IsFucked)
                {
                    _zeroer.UnFuck();
                }
                if (dir != Vector3.zero)
                {
                    _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, _moveSpeed, _acceleration);
                }
                else
                {
                    _currentMoveSpeed = 0;
                }
            }
            _anim.SetFloat("inputV", _currentMoveSpeed);
        }

        _momentum = Vector3.Lerp(_momentum, dir, _moveSensitivity);
        transform.Translate(_momentum * _currentMoveSpeed * Time.deltaTime, Space.World);
    }

    private void Aim()
    {
        if (_isDashing)
            return;

        bool isStill = _input.MoveDirection == Vector3.zero;
        var targetDirection = isStill || _input.IsAiming ? _input.AimDirection : _input.MoveDirection;

        if (targetDirection == Vector3.zero)
            return;

        var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotateSensitivity);
    }

    private void Aim(Vector3 dir)
    {
        var lookAtPos = transform.position + dir;
        transform.LookAt(lookAtPos);
    }

    private void Melee()
    {
        if (_isBusy)
            return;

        Aim(_input.AimDirection);
        _melee.Melee();
    }

    private void Fire()
    {
        if (_isBusy)
            return;

        _gun.Fire();
    }

    private void Dash()
    {
        if (_isBusy || _momentum == Vector3.zero || _input.MoveDirection == Vector3.zero)
            return;

        StartCoroutine(DoDash(_dashDuration));
    }

    IEnumerator DoDash(float duration)
    {
        _isDashing = true;
        var start = Time.time;
        _anim.SetTrigger("dash");

        yield return new WaitForSeconds(duration);

        _isDashing = false;
    }

    IEnumerator DoDeathZoom()
    {
        yield return new WaitForSeconds(.8f);
        CameraController.ZoomMultiply(.6f);
    }

    IEnumerator DoDamagedHit()
    {
        yield return new WaitForSeconds(.1f);
        CameraController.Pulse();
    }
}
