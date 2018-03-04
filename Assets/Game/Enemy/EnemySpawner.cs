using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField]
    private float _spawnDelayMax = 5f;
    [SerializeField]
    private float _spawnDelayMin = .5f;
    [SerializeField]
    [Range(0.95f, 1f)]
    private float _spawnDelayMultiplier = .95f;
    [SerializeField]
    private float _spawnRate;

    private float _spawnDelay;
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
        _spawnDelay = _spawnDelayMax;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_spawnDelay * WibblyWobbly.TimeSpeed);
            if (GameOver.IsEnded())
                yield break;
            Spawn();
            _spawnDelay = Mathf.Clamp(
                _spawnDelay * _spawnDelayMultiplier,
                _spawnDelayMin,
                _spawnDelayMax
            );
        }
    }
}