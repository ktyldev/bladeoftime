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
    public UnityEvent Hit { get; private set; }

    public void DoDamage(int damage)
    {
        Value -= damage;
        Hit.Invoke();

        if (Value <= 0)
        {
            Death.Invoke();
        }
    }

    public void DoDamage()
    {
        DoDamage(1);
    }

    void Awake()
    {
        Hit = new UnityEvent();
        Death = new UnityEvent();
    }

    void Start()
    {
        Value = _initialValue;
    }
}
