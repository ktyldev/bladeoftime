using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationZeroer : MonoBehaviour
{
    [SerializeField]
    private Transform _fuckedModel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UnFuckRotation()
    {
        _fuckedModel.transform.localEulerAngles = new Vector3();
    }
}
