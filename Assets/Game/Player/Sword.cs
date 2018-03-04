using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private PlayerMelee _melee;
    private SFXManager _sfx;
    private bool _isSwinging;
    
    void Start()
    {
        _melee = this.Find<PlayerMelee>(GameTags.Player);
        _sfx = this.Find<SFXManager>(GameTags.Sound);
    }

    void Update()
    {
        if (!_isSwinging && _melee.IsAttacking)
        {
            // Start swing
            _isSwinging = true;
            _sfx.PlayRandomSoundDelayed("nohit", 2, .5f);
        }
        if (_isSwinging && !_melee.IsAttacking)
        {
            // Swing ended
            _isSwinging = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_melee.IsAttacking)
            return;
        
        var enemy = other.gameObject.GetComponent<EnemyBehave>();
        if (enemy == null)
            return;

        var health = enemy.GetComponent<Health>();
        if (health == null)
            throw new System.Exception();

        _sfx.PlayRandomSound("hit", 5);
        health.DoDamage();
    }
}
