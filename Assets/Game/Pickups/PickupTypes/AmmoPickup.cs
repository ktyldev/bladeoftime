using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using Extensions;

public class AmmoPickup : Pickup {

    public override void DoPickup()
    {
        this.Find<PlayerShoot>(GameTags.Player).Reload();
    }

    public override bool CanPickup()
    {
        return !this.Find<PlayerShoot>(GameTags.Player).IsCharged();
    }
}
