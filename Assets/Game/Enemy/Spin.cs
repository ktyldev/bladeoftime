using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    
    void Update()
    {
        transform.Rotate(Vector3.up, _speed * WibblyWobbly.deltaTime);
    }
}
