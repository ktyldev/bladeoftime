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
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(_player);
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _player.position) < _attackDistance)
        {
            // print("enemy in range!");
        }
    }
}
