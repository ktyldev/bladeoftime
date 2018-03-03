using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar;

    private Health _health;

    void Start()
    {
        _health = this.Find<Health>(GameTags.Player); 
    }
    
    void OnGUI()
    {
        _healthBar.fillAmount = _health.Percentage;
    }
}
