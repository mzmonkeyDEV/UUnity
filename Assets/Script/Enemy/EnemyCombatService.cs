using System.Collections;
using UnityEngine;

public class EnemyCombatService
{
    private readonly Transform _transform;
    private readonly EnemyConfig _config;

    private float _lastSkillTime;
    private float _lastMeleeTime;

    public EnemyCombatService(Transform transform, EnemyConfig config)
    {
        _transform = transform;
        _config = config;
        _lastSkillTime = -_config.skillCooldown;
    }

    public bool CanUseSkill() =>
        Time.time >= _lastSkillTime + _config.skillCooldown;

    public bool CanMelee() =>
        Time.time >= _lastMeleeTime + _config.meleeCooldown;

    // ------------------------------- MINION MELEE -------------------------------
    public void MeleeAttack(Transform target)
    {
        if (!CanMelee()) return;
        if (Vector3.Distance(_transform.position, target.position) <= _config.meleeRange)
        {
            // Apply damage here
            Debug.Log("Minion deals melee damage");
            _lastMeleeTime = Time.time;
        }
    }

    // ------------------------------- BOSS SKILL 1: Big Bone ---------------------
    public void SkillBigBone()
    {
        if (!CanUseSkill()) return;

        GameObject obj = Object.Instantiate(_config.bigBoneProjectile, _transform.position, _transform.rotation);
        obj.GetComponent<Rigidbody>().AddForce(_transform.forward * _config.bigBoneForce, ForceMode.Impulse);

        _lastSkillTime = Time.time;
    }

    // ------------------------------- BOSS SKILL 2: Rapid Fire -------------------
    public IEnumerator SkillRapidFire()
    {
        if (!CanUseSkill()) yield break;

        for (int i = 0; i < _config.rapidFireCount; i++)
        {
            GameObject obj = Object.Instantiate(_config.rapidFireProjectile, _transform.position, _transform.rotation);
            obj.GetComponent<Rigidbody>().AddForce(_transform.forward * _config.rapidFireForce, ForceMode.Impulse);
            yield return new WaitForSeconds(_config.rapidFireRate);
        }

        _lastSkillTime = Time.time;
    }

    // ------------------------------- BOSS SKILL 3: AOE --------------------------
    public IEnumerator SkillAOE()
    {
        if (!CanUseSkill()) yield break;

        yield return new WaitForSeconds(_config.aoeDelay);
        Object.Instantiate(_config.aoeBonePrefab, _transform.position, Quaternion.identity);

        _lastSkillTime = Time.time;
    }

    // ------------------------------- BOSS SKILL 4: Summon -----------------------
    public void SkillSummon()
    {
        if (!CanUseSkill()) return;

        for (int i = 0; i < _config.summonCount; i++)
        {
            Vector3 pos = _transform.position + Random.insideUnitSphere * 2f;
            pos.y = _transform.position.y;
            Object.Instantiate(_config.minionPrefab, pos, Quaternion.identity);
        }

        _lastSkillTime = Time.time;
    }
}
