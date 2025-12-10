using UnityEngine;

public class HealSkill : SkillNAJA
{
    public int healingAmountPerSecond = 5;
    public float duration = 5f;

    float healAccumulator = 0f;

    public HealSkill()
    {
        skillName = "Heal";
        cooldownTime = 8f;
    }

    public override void Activate(CharacterMovement player)
    {
        timer = duration;
        healAccumulator = 0f;
    }

    public override void UpdateSkill(CharacterMovement player)
    {
        if (timer <= 0f)
        {
            Deactivate(player);
            return;
        }

        timer -= Time.deltaTime;
        healAccumulator += Time.deltaTime;

        if (healAccumulator >= 1f)
        {
            player.Heal(healingAmountPerSecond);
            healAccumulator = 0f;
        }
    }

    public override void Deactivate(CharacterMovement player)
    {
        timer = 0f;
    }
}