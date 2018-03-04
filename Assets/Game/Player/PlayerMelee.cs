using Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private float _attackTime;

    [SerializeField]
    [Range(0f, 1f)]
    private float _startDamage = .2f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _endDamage = .8f;

    private SFXManager _sfx;

    public bool IsAttacking { get; private set; }
    public bool IsDamaging { get; private set; }

    void Start()
    {
        _sfx = this.Find<SFXManager>(GameTags.Sound);
    }

    public void Melee()
    {
        if (IsAttacking)
            return;

        //int attackNumber = Random.Range(1, 6);
        int attackNumber = Random.Range(1, 4);
        string trigger = string.Format("melee0{0}", attackNumber);
        _anim.SetTrigger(trigger);
        _anim.SetBool("isAttacking", true);
        _anim.SetFloat("inputV", 0f);

        _sfx.PlayRandomSoundDelayed("attack", 5, .1f);

        StartCoroutine(MeleeAttack());
    }

    private IEnumerator MeleeAttack()
    {
        IsAttacking = true;
        yield return new WaitForSecondsRealtime(_attackTime * _startDamage);
        IsDamaging = true;
        yield return new WaitForSecondsRealtime(_attackTime * (_endDamage - _startDamage));
        IsDamaging = false;
        yield return new WaitForSecondsRealtime(_attackTime * (1 - _endDamage));
        IsAttacking = false;

        _anim.SetBool("isAttacking", false);
    }
}
