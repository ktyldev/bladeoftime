using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class EnemyBehave : MonoBehaviour {
    private Transform _player;
    public float speed;

    
    


    // Use this for initialization
    void Start () {
        _player = this.Find(GameTags.Player).transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(_player);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
