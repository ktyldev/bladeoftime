using Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _trails;
    [SerializeField]
    private int _baseDamage;
    [SerializeField]
    [Range(0f, 1f)]
    private float _startDamage;
    [SerializeField]
    [Range(0f, 1f)]
    private float _endDamage;
    [SerializeField]
    private float _slowTimeAmount;
    [SerializeField]
    private float _maxLightIntensity;
    [SerializeField]
    private GameObject[] _lights;
    [Range(0, 1)]
    [SerializeField]
    private float _lightUpSpeed;

    private bool Trails { set { _trails.ToList().ForEach(t => t.SetActive(value)); } }

    private PlayerMelee _melee;
    private SFXManager _sfx;
    private bool _isSwinging;
    private float _initialLightIntensity;
    
    void Start()
    {
        _melee = this.Find<PlayerMelee>(GameTags.Player);
        _sfx = this.Find<SFXManager>(GameTags.Sound);
        _trails.ToList().ForEach(t => t.SetActive(false));
        _initialLightIntensity = _lights.First().GetComponent<Light>().intensity;

        StartCoroutine(FadeLights());
    }
    
    void Update()
    {
        if (!_isSwinging && _melee.IsAttacking)
        {
            // Start swing
            _isSwinging = true;
            Trails = true;
            _sfx.PlayRandomSoundDelayed("nohit", 2, .5f);

        }
        if (_isSwinging && !_melee.IsAttacking)
        {
            // Swing ended
            Trails = false;
            _isSwinging = false;
        }
    }

    private IEnumerator FadeLights()
    {
        var start = Time.time;
        var lights = _lights.Select(go => go.GetComponent<Light>());
        var brightness = lights.First().intensity;
        
        while (true)
        {
            foreach (var light in lights)
            {
                if (_isSwinging)
                {
                    light.intensity = Mathf.Lerp(light.intensity, _maxLightIntensity, _lightUpSpeed);
                }
                else
                {
                    light.intensity = Mathf.Lerp(light.intensity, _initialLightIntensity, _lightUpSpeed);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_melee.IsDamaging)
            return;
        
        var enemy = other.gameObject.GetComponent<EnemyBehave>();
        if (enemy == null)
            return;

        var health = enemy.GetComponent<Health>();
        if (health == null)
            throw new System.Exception();

        _sfx.PlayRandomSound("hit", 5);
        health.DoDamage(_baseDamage);
        WibblyWobbly.SlowTime(_slowTimeAmount);
    }
}
