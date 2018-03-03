using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class EnemyBehave : MonoBehaviour {

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _attackDistance;

    private Transform _player;

    // Use this for initialization
    void Start () {
        _player = this.Find(GameTags.Player).transform;
        GetComponent<Health>().Death.AddListener(() =>
        {
            print("enemy killed!");
            Destroy(this.gameObject);
        });
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(_player);
        transform.Translate(Vector3.forward * _speed * WibblyWobbly.deltaTime);

        if (Vector3.Distance(transform.position, _player.position) < _attackDistance)
        {
            // print("enemy in range!");
        }
    }

    void OnDestroy()
    {
        WibblyWobbly.SlowTime(.1f);
    }
}
