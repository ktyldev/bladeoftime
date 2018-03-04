using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    private GameObject _trail;
    [SerializeField]
    private int _baseDamage;
    [SerializeField]
    [Range(0f, 1f)]
    private float _startDamage;
    [SerializeField]
    [Range(0f, 1f)]
    private float _endDamage;
    [SerializeField]
    private float _slowTimeAmount;

    private PlayerMelee _melee;
    private SFXManager _sfx;
    private bool _isSwinging;
    
    void Start()
    {
        _melee = this.Find<PlayerMelee>(GameTags.Player);
        _sfx = this.Find<SFXManager>(GameTags.Sound);
        _trail.SetActive(false);
    }

    void Update()
    {
        if (!_isSwinging && _melee.IsAttacking)
        {
            // Start swing
            _isSwinging = true;
            _trail.SetActive(true);
            _sfx.PlayRandomSoundDelayed("nohit", 2, .5f);
        }
        if (_isSwinging && !_melee.IsAttacking)
        {
            // Swing ended
            _trail.SetActive(false);
            _isSwinging = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_melee.IsDamaging)
            return;
        
        var enemy = other.gameObject.GetComponent<EnemyBehave>();
        if (enemy == null)
            return;

        var health = enemy.GetComponent<Health>();
        if (health == null)
            throw new System.Exception();

        _sfx.PlayRandomSound("hit", 5);
        health.DoDamage(_baseDamage);
        WibblyWobbly.SlowTime(_slowTimeAmount);
    }
}
