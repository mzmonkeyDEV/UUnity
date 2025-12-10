using UnityEngine;

public class DashSkill : SkillNAJA
{
    public float dashForce = 15f;
    public float dashDuration = 0.15f;
    private float dashTime;

    public DashSkill()
    {
        skillName = "Dash";
        cooldownTime = 1.2f;
    }

    public override void Activate(CharacterMovement player)
    {
        dashTime = dashDuration;
        timer = dashDuration;
    }

    public override void UpdateSkill(CharacterMovement player)
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            player.rb.MovePosition(player.rb.position + player.transform.forward * dashForce * Time.deltaTime);
        }
    }
}