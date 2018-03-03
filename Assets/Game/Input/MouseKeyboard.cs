using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

public class MouseKeyboard : MonoBehaviour, IControlMode
{
    [SerializeField]
    private int _fireButton = 0; // Left mouse button
    [SerializeField]
    private int _aimButton = 1; // Right mouse button
    [SerializeField]
    private KeyCode _dashButton;

    private Plane _hitPlane;
    private Transform _player;

    public Vector3 AimDirection { get { return GetAimDirection(); } }
    public Vector3 MoveDirection { get { return GetMoveDirection(); } }
    public UnityEvent Melee { get; private set; }
    public UnityEvent Fire { get; private set; }
    public bool IsAiming { get { return Input.GetMouseButton(_aimButton); } }
    // Spacebar
    public UnityEvent Dash { get { throw new System.NotImplementedException(); } }

    void Awake()
    {
        Melee = new UnityEvent();
        Fire = new UnityEvent();

        _hitPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Start()
    {
        _player = this.Find(GameTags.Player).transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(_fireButton))
        {
            (IsAiming ? Fire : Melee).Invoke();
        }
    }
    
    private Vector3 GetMoveDirection()
    {
        var h = Input.GetAxis(GameTags.KeyboardHorizontal);
        var v = Input.GetAxis(GameTags.KeyboardVertical);

        return new Vector3(h, 0, v);
    }

    private Vector3 GetAimDirection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        Vector3 aimPos;
        if (_hitPlane.Raycast(ray, out distance))
        {
            aimPos = ray.GetPoint(distance);
        }
        else
        {
            return Vector3.zero;
        }

        var dir = aimPos - _player.position;

        return new Vector3(dir.x, 0, dir.z).normalized;
    }
}
