using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class GunCooldownBar : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    private PlayerShoot _weapon;

    // Use this for initialization
    void Start()
    {
        _weapon = this.Find<PlayerShoot>(GameTags.Player); 
    }

    // Update is called once per frame
    void OnGUI()
    {
        _image.fillAmount = _weapon.CooldownPercent;
    }
}
