using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationZeroer : MonoBehaviour
{
    [SerializeField]
    private Transform _fuckedModel;

    public bool IsFucked { get { return _fuckedModel.transform.localEulerAngles.y != 0; } }

    public void UnFuck()
    {
        _fuckedModel.transform.localEulerAngles = new Vector3();
    }
}
