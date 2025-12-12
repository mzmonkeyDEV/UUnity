using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyCombatService
{
    private readonly Character character;
    private readonly Transform _transform;
    private readonly EnemyConfig _config;
    private readonly EnemyAI _ai;
    private Animator _anim;

    private float _lastSkillTime;
    private float _lastMeleeTime;

    public EnemyCombatService(Transform transform, EnemyConfig config, EnemyAI ai)
    {
        _transform = transform;
        _config = config;
        _ai = ai;
        _anim = ai.GetComponent<Animator>();

        _lastSkillTime = -_config.skillCooldown;
    }

    // TIMERS
    public bool CanUseSkill() => Time.time >= _lastSkillTime + _config.skillCooldown;
    public bool CanMelee() => Time.time >= _lastMeleeTime + _config.meleeCooldown;

    private Quaternion FlatRotation()
    {
        Vector3 forward = _transform.forward;
        forward.y = 0;
        forward.Normalize();
        return Quaternion.LookRotation(forward);
    }

    private Quaternion FlatRotationTowards(Transform target)
    {
        Vector3 dir = target.position - _transform.position;
        dir.y = 0; // remove pitch
        if (dir == Vector3.zero) dir = _transform.forward;
        return Quaternion.LookRotation(dir);
    }



    
    // MINION — MELEE (ANIMATION EVENT controls the hit)
    
    public void MeleeAttack()
    {
        if (!CanMelee()) return;

        _anim.SetTrigger("Melee");
        _lastMeleeTime = Time.time;
    }

    // Animation event calls this
    public void MeleeDealDamage()
    {
        Debug.Log("Minion melee hit!");

        if (_ai.Player == null) return;

        if (Vector3.Distance(_transform.position, _ai.Player.position) <= _config.meleeRange)
        {
            Character target = _ai.Player.GetComponent<Character>();

            if (target != null)
            {
                target.TakeDamage((int)_config.meleeDamage);
            }
        }
    }


    
    // BOSS — SKILL 1 : BIG BONE
    
    public void SkillBigBone_Start()
    {
        if (!CanUseSkill()) return;

        _anim.SetTrigger("SkillBigBone");
        _lastSkillTime = Time.time;
    }

    // ANIMATION EVENT
    public void Event_SkillBigBone_Release()
    {
        Quaternion rot = FlatRotation();
        GameObject obj = Object.Instantiate(_config.bigBoneProjectile, _transform.position + Vector3.up * 1, rot);
        obj.GetComponent<Rigidbody>().AddForce(_transform.forward * _config.bigBoneForce, ForceMode.Impulse);

        _ai.RegisterSkillUsed();
    }


    
    // BOSS — SKILL 2 : RAPID FIRE
    
    public void SkillRapidFire_Start()
    {
        if (!CanUseSkill()) return;

        _anim.SetTrigger("SkillRapidFire");
        _lastSkillTime = Time.time;
    }

    // Animation event (called many times from the animation)

    public IEnumerator RapidFireCoroutine()
    {




        for (int i = 0; i < _config.rapidFireCount; i++)
        {
            // Boss spins gradually (spin speed based on count)
          
            // Fire projectile in current facing direction
           Event_SkillRapidFire_Shoot();

            yield return new WaitForSeconds(_config.rapidFireRate);
        }

        if (_config.enableBossWarp)
            _ai.RegisterSkillUsed();
    }
    public void Event_SkillRapidFire_Shoot()
    {
        Quaternion rot = FlatRotation();
        GameObject obj = Object.Instantiate(_config.rapidFireProjectile, _transform.position + Vector3.up * 1, rot);
        obj.GetComponent<Rigidbody>().AddForce(_transform.forward * _config.rapidFireForce, ForceMode.Impulse);
    }


    
    // BOSS — SKILL 3 : AOE
    
    public void SkillAOE_Start()
    {
        if (!CanUseSkill()) return;

        _anim.SetTrigger("SkillAOE");
        _lastSkillTime = Time.time;
    }

    public void Event_SkillAOE_Release()
    {
        Object.Instantiate(_config.aoeBonePrefab, _transform.position, Quaternion.identity);
        _ai.RegisterSkillUsed();
    }


    
    // BOSS — SKILL 4 : SUMMON
    
    public void SkillSummon_Start()
    {
        if (!CanUseSkill()) return;

        _anim.SetTrigger("SkillSummon");
        _lastSkillTime = Time.time;
    }

    // Animation event
    public void Event_SkillSummon_Spawn()
    {
        for (int i = 0; i < _config.summonCount; i++)
        {
            Vector3 pos = _transform.position + Random.insideUnitSphere * 2f;
            pos.y = _transform.position.y;

            Object.Instantiate(_config.minionPrefab, pos, Quaternion.identity);
        }

        _ai.RegisterSkillUsed();
    }
}
