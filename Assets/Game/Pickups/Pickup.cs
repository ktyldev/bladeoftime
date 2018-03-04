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

    private void Start()
    {
        Destroy(gameObject, 60);
    }

    private void Update()
    {
        Vector3 _rot = this.transform.eulerAngles;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, _rot.y + .5f, 0f));


    }
}