using UnityEngine;

public class DashSkill : SkillNAJA
{
    public float dashDistance = 3f;

    public DashSkill()
    {
        skillName = "Dash";
        cooldownTime = 3f;
    }

    public override void Activate(CharacterMovement player)
    {
        Vector3 dir = player.transform.forward;
        player.transform.position += dir * dashDistance;
        timer = 0f;
    }

    public override void UpdateSkill(CharacterMovement player)
    {
        
    }
}