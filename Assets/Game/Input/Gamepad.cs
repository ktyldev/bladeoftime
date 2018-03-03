using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

public class Gamepad : MonoBehaviour, IControlMode
{
    bool _lastFire;

    public Vector3 AimDirection { get { return GetAimDirection(); } }
    public Vector3 MoveDirection { get { return GetMoveDirection(); } }
    public UnityEvent Melee { get; private set; }
    public UnityEvent Fire { get; private set; }
    public bool IsAiming { get { return Input.GetAxisRaw("AimTrigger") > .3f; } }

    void Awake()
    {
        Melee = new UnityEvent();
        Fire = new UnityEvent();
    }

    void Update()
    {
        if (Input.GetAxisRaw("Fire") > 0.3f)
        {
            if (IsAiming && !_lastFire)
            {
                _lastFire = true;
                Fire.Invoke();
            }
        }
        else
        {
            _lastFire = false;
        }

        if (Input.GetButtonDown("Melee"))
        {
            Melee.Invoke();
        }
    }

    private Vector3 GetMoveDirection()
    {
        var h = Input.GetAxis("Gamepad_Horizontal");
        var v = Input.GetAxis("Gamepad_Vertical");

        return new Vector3(h, 0, v);
    }

    private Vector3 GetAimDirection()
    {
        if (IsAiming) return GetMoveDirection();
        var h = Input.GetAxis("RightH");
        var v = Input.GetAxis("RightV");

        return new Vector3(h, 0, v);
    }
}