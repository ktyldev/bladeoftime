using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;
using System;

public class EnemyBehave : MonoBehaviour {

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _screamDistance;
    [SerializeField]
    private float _attackCooldown;
    [SerializeField]
    private float _slowTimeAmount;
    [SerializeField]
    private GameObject[] _spawnLights;
    [SerializeField]
    private float _lightFadeSpeed;
    [SerializeField]
    private float _lightFadeTime;
    [SerializeField]
    private GameObject _deathFX;
    
    private Transform _player;
    private bool _canAttack = true;
    private bool _canScream = true;
    private Rigidbody _physics;
    private SFXManager _sfx;
    private float _spawnLightIntensity;
    private bool _ded = false;

    // Use this for initialization
    void Start () {
        _sfx = this.Find<SFXManager>(GameTags.Sound);
        _player = this.Find(GameTags.Player).transform;
        _physics = GetComponent<Rigidbody>();

        _spawnLightIntensity = _spawnLights
            .First()
            .GetComponent<Light>().intensity;

        StartCoroutine(FadeLights(false));

        GetComponent<Health>().Death.AddListener(Die);
	}

    public void Die()
    {
        _ded = true;
        StartCoroutine(FadeLights(true));
    }

    private IEnumerator FadeLights(bool turnOn)
    {
        var start = Time.time;
        var lights = _spawnLights
            .Select(go => go.GetComponent<Light>())
            .ToList();
        
        while (Time.time - start < _lightFadeTime)
        {
            lights.ForEach(l => l.intensity = Mathf.Lerp(l.intensity, turnOn ? _spawnLightIntensity : 0, _lightFadeSpeed));
            yield return new WaitForEndOfFrame();
        }

        if (turnOn)
        {
            Destroy(gameObject);
            var fx = Instantiate(_deathFX, transform.position, transform.rotation);
            Destroy(fx, 2);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_ded)
            return;
        
        _physics.velocity = (_player.transform.position - transform.position).normalized * _speed * 2f *  WibblyWobbly.TimeSpeed;

        if (GameOver.IsEnded())
            return;

        float _playerDist = Vector3.Distance(transform.position, _player.position);
        if (_playerDist < _screamDistance && _canScream)
        {
            StartCoroutine(Scream());
        }

        if (_playerDist > 100f)
        {
            Destroy(gameObject);
        }
    }
    
    private IEnumerator Attack()
    {
        _canAttack = false;
        var health = _player.GetComponent<Health>();

        if (_ded)
            yield break;

        health.DoDamage(1);
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    private IEnumerator Scream()
    {
        _canScream = false;
        _sfx.PlaySound("enemy_scream");
        yield return new WaitForSeconds(_attackCooldown);
        _canScream = true;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (!_canAttack || !coll.collider.gameObject.CompareTag(GameTags.Player))
            return;

        StartCoroutine(Attack());
    }

    private void OnCollisionStay(Collision coll)
    {
        OnCollisionEnter(coll);
    }
}
