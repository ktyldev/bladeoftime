using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class TimePickup : Pickup {
    [SerializeField]
    private float timeSlowAmount;

    public override void DoPickup()
    {
        WibblyWobbly.SlowTime(timeSlowAmount);
    }

    public override bool CanPickup()
    {
        return true;
    }
}
