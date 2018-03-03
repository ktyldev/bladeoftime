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
    public bool IsAiming { get { return Input.GetAxisRaw(GameTags.AimTrigger) > .3f; } }

    void Awake()
    {
        Melee = new UnityEvent();
        Fire = new UnityEvent();
    }

    void Update()
    {
        if (Input.GetAxisRaw(GameTags.Fire) > 0.3f)
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

        if (Input.GetButtonDown(GameTags.Melee))
        {
            Melee.Invoke();
        }
    }

    private Vector3 GetMoveDirection()
    {
        var h = Input.GetAxis(GameTags.GamepadMoveHorizontal);
        var v = Input.GetAxis(GameTags.GamepadMoveVertical);

        return new Vector3(h, 0, v);
    }

    private Vector3 GetAimDirection()
    {
        if (IsAiming) return GetMoveDirection();
        var h = Input.GetAxis(GameTags.GamepadAimHorizontal);
        var v = Input.GetAxis(GameTags.GamepadAimVertical);

        return new Vector3(h, 0, v);
    }
}