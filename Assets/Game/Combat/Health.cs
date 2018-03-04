using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxValue;

    public int Value { get; private set; }
    public float Percentage { get { return (float)Value / _maxValue; } }

    public UnityEvent Death { get; private set; }
    public UnityEvent Hit { get; private set; }
    public UnityEvent Heal { get; private set; }

    public void DoDamage(int damage)
    {
        if (Value <= 0)
            return;

        Value -= damage;

        if (Value <= 0)
        {
            Death.Invoke();
        }
        else
        {
            Hit.Invoke();
        }
    }

    public void DoHeal(int healAmount)
    {
        if (Value <= 0)
            return;

        Value += healAmount;
        if (Value > _maxValue)
            Value = _maxValue;

        Heal.Invoke();
    }

    public void DoDamage()
    {
        DoDamage(1);
    }

    public void DoHeal()
    {
        DoHeal(1);
    }

    void Awake()
    {
        Hit = new UnityEvent();
        Death = new UnityEvent();
    }

    void Start()
    {
        Value = _maxValue;
    }
}
