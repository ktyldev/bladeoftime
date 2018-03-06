using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ControlMode : MonoBehaviour, IControlMode
{
    [SerializeField]
    private GameObject[] _templates;

    public Vector3 AimDirection { get { return _controlModes[_controlModeIndex].AimDirection; } }
    public Vector3 MoveDirection { get { return _controlModes[_controlModeIndex].MoveDirection; } }
    public UnityEvent Melee { get; private set; }
    public UnityEvent Fire { get; private set; }
    public UnityEvent Dash { get; private set; }
    public bool IsFiring { get { return _controlModes[_controlModeIndex].IsFiring; } }
    public bool IsAiming { get { return _controlModes[_controlModeIndex].IsAiming; } }
    public bool AnyButtonPressed { get { return _controlModes[_controlModeIndex].AnyButtonPressed; } }

    private int _controlModeIndex;
    private IControlMode[] _controlModes;

    void Awake()
    {
        Melee = new UnityEvent();
        Fire = new UnityEvent();
        Dash = new UnityEvent();

        _controlModes = _templates
            .Select(go =>
            {
                var controlMode = Instantiate(go, transform).GetComponent<IControlMode>();

                controlMode.Melee.AddListener(() => TryInvoke(controlMode, Melee));
                controlMode.Fire.AddListener(() => TryInvoke(controlMode, Fire));
                controlMode.Dash.AddListener(() => TryInvoke(controlMode, Dash));

                return controlMode;
            })
            .ToArray();

        _controlModeIndex = 0;
    }
    
    private void TryInvoke(IControlMode controlMode, UnityEvent @event)
    {
        if (controlMode == _controlModes[_controlModeIndex])
        {
            @event.Invoke();
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _controlModeIndex = _controlModeIndex == 1 ? 0 : 1;
            print(_controlModes[_controlModeIndex]);
        }
    }
}
