using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField]
    private float _spawnFrequencyMax;
    [SerializeField]
    private float _spawnFrequencyMin;

    private float _spawnFrequency;
    protected override bool CanSpawn(GameObject template)
    {
        return true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn();
        }
    }

    private void Awake()
    {
        _spawnFrequency = _spawnFrequencyMax;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_spawnFrequency);
            Spawn();
            _spawnFrequency = Mathf.Clamp(
                _spawnFrequency * .98f,
                _spawnFrequencyMin,
                _spawnFrequencyMax
            );
            print("spawn frequency: " + _spawnFrequency);
        }
    }
}