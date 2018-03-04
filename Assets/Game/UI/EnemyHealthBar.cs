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

    private Health _enemyHealth;

    void Start()
    {
        _enemyHealth = _enemy.GetComponent<Health>();
    }

    void OnGUI()
    {
        _healthBar.fillAmount = _enemyHealth.Percentage;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
