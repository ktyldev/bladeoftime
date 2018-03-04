using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpeed : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    
    void Start()
    {
        if (_text == null)
            throw new System.Exception();
    }

    void OnGUI()
    {
        _text.text = WibblyWobbly.TimeSpeed.ToString("F2");
    }
}
