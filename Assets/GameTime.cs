using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private float _scale;

    private float _current;

    // Use this for initialization
    void Start()
    {

    }

    private void Update()
    {
        _current += WibblyWobbly.deltaTime * _scale;
    }

    // Update is called once per frame
    void OnGUI()
    {
        _text.text = _current.ToString();
    }
}
