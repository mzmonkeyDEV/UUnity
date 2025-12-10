using System.Collections.Generic;
using UnityEngine;

public class Skill_Book : MonoBehaviour
{
    public List<SkillNAJA> skillsSet = new List<SkillNAJA>();
    public GameObject[] skillEffects;
    List<SkillNAJA> DulationSkills = new List<SkillNAJA>();

    CharacterMovement player;

    void Start()
    {
        // เดิมเป็น GetComponent<Player>()
        player = GetComponent<CharacterMovement>();

        skillsSet.Clear();
        skillsSet.Add(new DashSkill()); // index 0
        skillsSet.Add(new HealSkill()); // index 1
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkill(0); // Dash
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(1); // Heal
        }

        // อัปเดต duration skills
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

    public void UseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skillsSet.Count)
            return;

        SkillNAJA skill = skillsSet[skillIndex];

        if (!skill.IsReady(Time.time))
            return;

        skill.Activate(player);
        skill.TimeStampSkill(Time.time);

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