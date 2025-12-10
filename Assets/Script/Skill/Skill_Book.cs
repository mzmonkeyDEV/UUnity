using System.Collections.Generic;
using UnityEngine;

public class Skill_Book : MonoBehaviour
{
    public List<SkillNAJA> skillsSet = new List<SkillNAJA>();
    public GameObject[] skillEffects;
    public Vector3[] effectOffsets;

    // Prefab กระสุนสายฟ้าสำหรับ Thunder Bolt
    public GameObject thunderProjectilePrefab;

    List<SkillNAJA> DulationSkills = new List<SkillNAJA>();

    CharacterMovement player;

    float lastShiftTime = 0f;
    public float doubleShiftThreshold = 0.3f;

    void Start()
    {
        player = GetComponent<CharacterMovement>();

        skillsSet.Clear();
        skillsSet.Add(new DashSkill());        // index 0
        skillsSet.Add(new HealSkill());        // index 1
        skillsSet.Add(new BerserkSkill());     // index 2

        // --- สร้าง ThunderBoltSkill แล้วส่ง prefab กระสุนเข้าไป ---
        ThunderBoltSkill thunder = new ThunderBoltSkill();
        thunder.projectilePrefab = thunderProjectilePrefab;
        skillsSet.Add(thunder);                // index 3
        // ------------------------------------------------------------
    }

    void Update()
    {
        DetectDashDoubleShift(); // Dash (index 0)
        DetectHeal();            // Heal (index 1)
        DetectBerserk();         // Berserk (index 2)
        DetectThunderBolt();     // Thunder Bolt (index 3)

        // อัปเดตสกิลที่มี duration
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

    void DetectDashDoubleShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time - lastShiftTime <= doubleShiftThreshold)
            {
                UseSkill(0); // Dash
            }

            lastShiftTime = Time.time;
        }
    }

    void DetectHeal()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(1); // Heal
        }
    }

    void DetectBerserk()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkill(2); // Berserk
        }
    }

    void DetectThunderBolt()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseSkill(3); // Thunder Bolt
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

        // สร้าง effect ประกอบ (เช่น แสงตอนกดสกิล)
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