using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class GunCooldownBar : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    private PlayerController _player;

    // Use this for initialization
    void Start()
    {
        _player = this.Find<PlayerController>(GameTags.Player); 
    }

    // Update is called once per frame
    void OnGUI()
    {
        _image.fillAmount = _player.CooldownPercent;
    }
}
