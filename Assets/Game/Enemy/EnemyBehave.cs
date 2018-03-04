using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class EnemyBehave : MonoBehaviour {

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _attackDistance;
    [SerializeField]
    private float _attackCooldown;
    [SerializeField]
    private float _slowTimeAmount;

    private Transform _player;
    private bool _canAttack = true;
    private bool _canScream = true;
    private Rigidbody _physics;
    private SFXManager _sfx;

    // Use this for initialization
    void Start () {
        _sfx = this.Find<SFXManager>(GameTags.Sound);
        _player = this.Find(GameTags.Player).transform;
        _physics = GetComponent<Rigidbody>();

        GetComponent<Health>().Death.AddListener(() =>
        {
            Destroy(gameObject);
        });
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(_player);
        _physics.velocity = transform.forward * _speed * 2f *  WibblyWobbly.TimeSpeed;

        if (GameOver.IsEnded())
            return;

        float _playerDist = Vector3.Distance(transform.position, _player.position);
        if (_playerDist < _attackDistance + 1f && _canScream)
        {
            StartCoroutine(Scream());
        }
        if (_playerDist < _attackDistance && _canAttack)
        {
            StartCoroutine(Attack());
        }
    }
    
    private IEnumerator Attack()
    {
        _canAttack = false;
        var health = _player.GetComponent<Health>();
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
}
