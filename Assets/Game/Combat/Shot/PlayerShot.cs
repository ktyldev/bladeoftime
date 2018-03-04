using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : Shot
{
    [SerializeField]
    private int _baseDamage;

    protected override void OnHit(GameObject hitObject)
    {
        var health = hitObject.GetComponent<Health>();
        if (health == null)
            throw new Exception();

        health.DoDamage(_baseDamage);
    }

    protected override bool ValidateHit(GameObject obj)
    {
        return obj.GetComponent<Health>() != null && 
            !obj.CompareTag(GameTags.Player);
    }
}
