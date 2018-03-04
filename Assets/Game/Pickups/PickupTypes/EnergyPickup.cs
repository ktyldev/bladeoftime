using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : Pickup
{
    private PlayerShoot _weapon;

    void Start()
    {
        _weapon = this.Find<PlayerShoot>(GameTags.Player);
    }

    public override bool CanPickup()
    {
        return true;
    }

    public override void DoPickup()
    {
        _weapon.Recharge();
    }
}
