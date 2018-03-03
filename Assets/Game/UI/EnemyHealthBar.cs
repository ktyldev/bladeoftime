using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private Image _healthBar;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
