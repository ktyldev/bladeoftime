using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public abstract void DoPickup();

    private void OnTriggerEnter(Collider other)
    {
        print("collided!");
        if (!other.gameObject.CompareTag(GameTags.Player))
            return;

        DoPickup();
        Destroy(this.gameObject);
    }
}