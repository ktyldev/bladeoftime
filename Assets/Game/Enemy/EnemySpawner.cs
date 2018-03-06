using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField]
    private float _baseSpawnDelay;
    [SerializeField]
    private float _spawnDelayDecrement;
    [SerializeField]
    private float _spawnDecrementDelay;
    [SerializeField]
    private float _minSpawnDelay;
    
    private float _spawnDelay;
    protected override bool CanSpawn(GameObject template)
    {
        return true;
    }
    
    private void Start()
    {
        _spawnDelay = _baseSpawnDelay;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseSpawnRate());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(_spawnDelay * (1f / WibblyWobbly.TimeSpeed));
            if (GameOver.IsEnded())
                yield break;
        }
    }

    private IEnumerator IncreaseSpawnRate()
    {
        while (_spawnDelay > _minSpawnDelay)
        {
            yield return new WaitForSeconds(_spawnDecrementDelay * WibblyWobbly.TimeSpeed);
            _spawnDelay -= Mathf.Clamp(_spawnDelayDecrement, _minSpawnDelay, Mathf.Infinity);
        }
    }
}