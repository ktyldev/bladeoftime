using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : Spawner {

    [SerializeField]
    private float _spawnDelay;

    protected override bool CanSpawn(GameObject template)
    {
        return true;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);
            if (GameOver.IsEnded())
                yield break;
            
            Spawn();
        }
    }
}
