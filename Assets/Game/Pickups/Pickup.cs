using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public abstract void DoPickup();

    public abstract bool CanPickup();

    private void OnTriggerEnter(Collider other)
    {
        print("collided!");
        if (!other.gameObject.CompareTag(GameTags.Player) || !CanPickup())
            return;

        DoPickup();
        Destroy(this.gameObject);
    }
}