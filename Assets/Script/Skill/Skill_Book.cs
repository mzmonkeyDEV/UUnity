using System.Collections.Generic;
using UnityEngine;

public class Skill_Book : MonoBehaviour
{
    public List<SkillNAJA> skillsSet = new List<SkillNAJA>();
    public GameObject[] skillEffects;
    public Vector3[] effectOffsets;

    public GameObject thunderProjectilePrefab;
    public GameObject thunderStrikeEffectPrefab;

    List<SkillNAJA> DulationSkills = new List<SkillNAJA>();

    CharacterMovement player;
    Animator anim;

    void Start()
    {
        player = GetComponent<CharacterMovement>();
        anim = player != null ? player.animator : GetComponentInChildren<Animator>();

        skillsSet.Clear();
        skillsSet.Add(new DashSkill());       // index 0
        skillsSet.Add(new HealSkill());       // index 1
        skillsSet.Add(new BerserkSkill());    // index 2

        ThunderBoltSkill thunder = new ThunderBoltSkill();
        thunder.projectileSpeed = 20f;
        thunder.damageMultiplier = 2.5f;
        thunder.projectilePrefab = thunderProjectilePrefab;
        skillsSet.Add(thunder);               // index 3

        ThunderStrikeSkill strike = new ThunderStrikeSkill();
        strike.range = 15f;
        strike.damageMultiplier = 3f;
        strike.strikeEffectPrefab = thunderStrikeEffectPrefab;
        skillsSet.Add(strike);                // index 4
    }

    void Update()
    {
        DetectDash();
        DetectHeal();
        DetectBerserk();
        DetectThunderBolt();
        DetectThunderStrike();

        for (int i = DulationSkills.Count - 1; i >= 0; i--)
        {
            SkillNAJA s = DulationSkills[i];
            s.UpdateSkill(player);

            if (s.timer <= 0)
            {
                DulationSkills.RemoveAt(i);
            }
        }
    }

    void DetectDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (anim != null) anim.SetTrigger("Dash");
            UseSkill(0);
        }
    }

    void DetectHeal()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (anim != null) anim.SetTrigger("Heal");
            UseSkill(1);
        }
    }

    void DetectBerserk()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (anim != null) anim.SetTrigger("Berserk");
            UseSkill(2);
        }
    }

    void DetectThunderBolt()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (anim != null) anim.SetTrigger("ThunderBolt");
            UseSkill(3);
        }
    }

    void DetectThunderStrike()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (anim != null) anim.SetTrigger("ThunderStrike");
            UseSkill(4);
        }
    }

    public void UseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skillsSet.Count)
            return;

        SkillNAJA skill = skillsSet[skillIndex];

        if (!skill.IsReady(Time.time))
            return;

        skill.Activate(player);
        skill.TimeStampSkill(Time.time);

        if (skillEffects != null &&
            skillIndex < skillEffects.Length &&
            skillEffects[skillIndex] != null)
        {
            Vector3 spawnPos = player.transform.position;

            if (effectOffsets != null && skillIndex < effectOffsets.Length)
            {
                spawnPos += effectOffsets[skillIndex];
            }

            GameObject fx = Instantiate(
                skillEffects[skillIndex],
                spawnPos,
                player.transform.rotation
            );

            fx.transform.SetParent(player.transform);
            Destroy(fx, 3f);
        }

        if (skill.timer > 0 && !DulationSkills.Contains(skill))
        {
            DulationSkills.Add(skill);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}