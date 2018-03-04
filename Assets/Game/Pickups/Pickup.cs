using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public abstract void DoPickup();

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.gameObject.CompareTag(GameTags.Player))
            return;

        DoPickup();
        Destroy(this.gameObject);
    }
}