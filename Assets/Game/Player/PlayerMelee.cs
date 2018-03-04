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

    private SFXManager _sfx;

    public bool IsAttacking { get; private set; }

    void Start()
    {
        _sfx = this.Find<SFXManager>(GameTags.Sound);
    }

    public void Melee()
    {
        if (IsAttacking)
            return;

        int attackNumber = Random.Range(1, 6);
        string trigger = string.Format("melee0{0}", attackNumber);
        _anim.SetTrigger(trigger);
        _anim.SetFloat("inputV", 0f);

        _sfx.PlayRandomSoundDelayed("attack", 5, .1f);

        StartCoroutine(MeleeAttack());
    }

    private IEnumerator MeleeAttack()
    {
        IsAttacking = true;
        yield return new WaitForSecondsRealtime(_attackTime);
        IsAttacking = false;
    }
}
