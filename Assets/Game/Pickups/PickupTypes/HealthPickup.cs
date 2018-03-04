using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class HealthPickup : Pickup {
    public override void DoPickup()
    {
        this.Find<Health>(GameTags.Player).DoHeal();
        print("healed!");
    }

    public override bool CanPickup()
    {
        return this.Find<Health>(GameTags.Player).NeedsHealing;
    }
}
