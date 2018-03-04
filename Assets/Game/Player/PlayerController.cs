using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

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
    [SerializeField]
    [Range(0, 1)]
    private float _attackMoment;

    [SerializeField]
    private float _shootDistance;
    [SerializeField]
    private float _baseGunCooldown; // This is affected by wibbly wobbly time

    // I want to be sick
    private RotationZeroer _zeroer;

    public float CooldownPercent { get; private set; }

    private IControlMode _input;
    private Vector3 _momentum;
    private SFXManager _sfx;

    private bool _isDashing = false;
    private bool _isAttacking;
    private bool _gunOnCooldown;

    private bool _isBusy { get { return (_isAttacking || _isDashing); } }

    private void Awake()
    {
        _input = Instantiate(_controlMode, transform)
            .GetComponent<IControlMode>();
    }

    void Start()
    {
        CooldownPercent = 1;

        _sfx = this.Find<SFXManager>(GameTags.Sound);

        _zeroer = GetComponentInChildren<RotationZeroer>();
        if (_zeroer == null)
            throw new System.Exception("Everything's fucked :/");

        GetComponent<Health>().Death.AddListener(() =>
        {
            print("game over!");
        });

        GetComponent<Health>().Hit.AddListener(() =>
        {
            int soundNum = Random.Range(1, 4);
            _sfx.PlaySound(string.Format("hurt0{0}", soundNum));
        });
        
        if (_input == null)
            throw new System.Exception();

        _input.Melee.AddListener(Melee);
        _input.Fire.AddListener(Fire);
        _input.Dash.AddListener(Dash);
    }

    void Update()
    {
        Move();

        if (_isBusy)
            return;
        Aim();
    }

    private void Move()
    {
        Vector3 dir;
        float speed;
        if (_isDashing)
        {
            dir = transform.forward;
            speed = _dashSpeed;
        }
        else
        {
            dir = _input.MoveDirection;
            if (_isAttacking)
            {
                speed = 0;
                anim.SetFloat("inputV", 0f);
            }
            else
            {
                // fucking fight me
                if (_zeroer.IsFucked)
                {
                    _zeroer.UnFuck();
                }

                speed = _moveSpeed;
                anim.SetFloat("inputV", dir != Vector3.zero ? _momentum.magnitude * _moveSpeed : 0);
            }
        }
        _momentum = Vector3.Lerp(_momentum, dir, _moveSensitivity);
        transform.Translate(_momentum * speed * Time.deltaTime, Space.World);
    }

    private void Aim()
    {
        if (_isDashing)
            return;
        bool isStill = _input.MoveDirection == Vector3.zero;
        var targetDirection = isStill ? _input.AimDirection : _input.MoveDirection;

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

        int attackNumber = Random.Range(1, 6);
        string trigger = string.Format("melee0{0}", attackNumber);
        _sfx.PlaySoundDelayed(string.Format("attack0{0}", attackNumber), .1f);

        anim.SetFloat("inputV", 0f);
        anim.SetTrigger(trigger);
        StartCoroutine(MeleeAttack());
    }

    private void Fire()
    {
        if (_isBusy || _gunOnCooldown)
            return;

        StartCoroutine(FireAttack());
    }
    
    private IEnumerator MeleeAttack()
    {
        _isAttacking = true;
        print("melee!");
        Aim(_input.AimDirection);
        float _movementOffset = (_momentum == Vector3.zero) ? 0.15f : 0f;
        yield return new WaitForSeconds(
            Mathf.Clamp((_attackTime * _attackMoment) + _movementOffset,
            0f,
            _attackTime
        ));

        // cast from ~the middle of the player
        var ray = new Ray(transform.position + Vector3.up, transform.forward);
        var hitObjects = Physics.SphereCastAll(ray, 1f) // this radius parameter doesn't seem to work :/
            .Where(h =>
            {
                return Vector3.Angle(transform.forward, h.transform.position - transform.position) < _meleeConeAngle / 2f;
            })
            .Where(h =>
            {
                return Vector3.Distance(transform.position, h.transform.position) < _meleeDistance; // Hacky distance
            })
            .Select(h => h.collider.gameObject.GetComponent<Health>())
            .Where(h => h != null)
            .ToArray();

        foreach (var hitHealth in hitObjects)
        {
            hitHealth.DoDamage();
        }

        yield return new WaitForSeconds(
            Mathf.Clamp((_attackTime * (1f - _attackMoment)) - _movementOffset,
            0f,
            _attackTime
        ));
        _isAttacking = false;
    }

    private IEnumerator FireAttack()
    {
        _isAttacking = true;
        print("fire!");
        Aim(_input.AimDirection);

        var ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _shootDistance))
        {
            var enemy = hit.collider.gameObject;
            var enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.DoDamage(1);
            }
        }
        
        yield return new WaitForSeconds(_attackTime);
        _isAttacking = false;

        _gunOnCooldown = true;
        var cooldownTime = _baseGunCooldown * Mathf.Pow(1 / WibblyWobbly.TimeSpeed, 2);

        var cooldownStart = Time.time;
        var elapsedTime = 0f;
        while (Time.time - cooldownStart < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            CooldownPercent = elapsedTime / cooldownTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(cooldownTime);
        _gunOnCooldown = false;
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
        anim.SetTrigger("dash");

        yield return new WaitForSeconds(duration);

        _isDashing = false;
    }
}
