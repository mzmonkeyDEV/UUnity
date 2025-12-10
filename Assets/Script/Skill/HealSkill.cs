using UnityEngine;

public class HealSkill : SkillNAJA
{
    public int healAmount = 25;

    public HealSkill()
    {
        skillName = "Heal";
        cooldownTime = 5f;
    }

    public override void Activate(CharacterMovement player)
    {
        int newHp = Mathf.Clamp(player.currentHp + healAmount, 0, player.maxHp);
        player.currentHp = newHp;
        timer = 0f;
    }

    public override void UpdateSkill(CharacterMovement player)
    {
    }
}