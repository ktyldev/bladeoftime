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

    [SerializeField]
    private float _shootDistance;
    [SerializeField]
    private float _baseGunCooldown; // This is affected by wibbly wobbly time

    private IControlMode _input;
    private Vector3 _momentum;
    private Quaternion _lookRotation;

    private bool _isDashing = false;
    private bool _isAttacking;
    private bool _gunOnCooldown;

    private void Awake()
    {
        _input = Instantiate(_controlMode, transform)
            .GetComponent<IControlMode>();
    }

    void Start()
    {
        GetComponent<Health>().Death.AddListener(() =>
        {
            print("game over!");
        });
        
        if (_input == null)
            throw new System.Exception();

        _input.Melee.AddListener(Melee);
        _input.Fire.AddListener(Fire);
        _input.Dash.AddListener(Dash);
    }

    void Update()
    {
        if (_isAttacking || _isDashing)
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

        string trigger = string.Format("melee0{0}", Random.Range(1, 6).ToString());
        print(trigger);
        anim.SetTrigger(trigger);
        StartCoroutine(MeleeAttack());
    }

    private void Fire()
    {
        if (_isAttacking || _isDashing || _gunOnCooldown)
            return;

        StartCoroutine(FireAttack());
    }

    
    private IEnumerator MeleeAttack()
    {
        _isAttacking = true;
        print("melee!");
        Aim(_input.AimDirection);

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
        print(cooldownTime);
        yield return new WaitForSeconds(cooldownTime);
        _gunOnCooldown = false;
    }
    
    private void Dash()
    {
        if (_isDashing || _momentum == Vector3.zero || _input.MoveDirection == Vector3.zero)
            return;

        StartCoroutine(DoDash(_dashDuration));
        print("dash!");
    }

    IEnumerator DoDash(float duration)
    {
        _isDashing = true;
        var start = Time.time;
        //anim.SetFloat("inputV", 0);
        anim.SetTrigger("dash");

        while (Time.time - start < duration)
        {
            transform.Translate(transform.forward * _dashSpeed * Time.deltaTime, Space.World);
            yield return new WaitForEndOfFrame();
        }
        
        _isDashing = false;
    }
}
