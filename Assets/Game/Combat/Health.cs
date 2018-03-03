using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _initialValue;

    public int Value { get; private set; }
    public float Percentage { get { return (float)Value / _initialValue; } }

    public UnityEvent Death { get; private set; }

    void Awake()
    {
        Death = new UnityEvent();
    }

    void Start()
    {
        Value = _initialValue;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Value != 0)
        {
            Value--;
            if (Value == 0)
            {
                Death.Invoke();
            }
        }
    }
}
