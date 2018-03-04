using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NukePickup : Pickup
{
    [SerializeField]
    private float _range;

    public override bool CanPickup()
    {
        return true;
    }

    public override void DoPickup()
    {
        Physics.SphereCastAll(new Ray(transform.position, transform.forward), _range)
             .Select(h => h.collider.gameObject.GetComponent<EnemyBehave>())
             .Where(e => e != null)
             .ToList()
             .ForEach(e => e.Die());
    }
}
