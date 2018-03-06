using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField]
    private float _despawnDistance;

    private GameObject _player;
    
    public abstract void DoPickup();

    public abstract bool CanPickup();

    private void OnTriggerEnter(Collider other)
    {
        _player = this.Find(GameTags.Player);

        print("collided!");
        if (!other.gameObject.CompareTag(GameTags.Player) || !CanPickup())
            return;

        DoPickup();
        Destroy(this.gameObject);
    }

    void Awake()
    {
        _player = this.Find(GameTags.Player);
    }

    private void Start()
    {
        Destroy(gameObject, 60);
    }
    
    private void Update()
    {
        Vector3 _rot = this.transform.eulerAngles;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, _rot.y + .5f, 0f));

        if (Vector3.Distance(transform.position, _player.transform.position) > _despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}