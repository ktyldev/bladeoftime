using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Shot : MonoBehaviour
{
    [SerializeField]
    private float _life;
    [SerializeField]
    private float _speed;

    void Awake()
    {
        //_mover = GetComponent<Mover>();
        Destroy(gameObject, _life);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * WibblyWobbly.deltaTime);
    }
    
    protected abstract bool ValidateHit(GameObject obj);
    protected abstract void OnHit(GameObject hitObject);

    private void OnTriggerEnter(Collider other)
    {
        if (!ValidateHit(other.gameObject))
            return;

        Destroy(gameObject);
        OnHit(other.gameObject);
    }
}
