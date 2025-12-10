using UnityEngine;

public class BerserkSkill : SkillNAJA
{
    public float duration = 15f;          
    public float attackMultiplier = 1.5f;
    public float moveSpeedMultiplier = 1.3f;

    private int originalAtk;
    private float originalMoveSpeed;
    private bool isActive = false;

    public BerserkSkill()
    {
        skillName = "Berserk";
        cooldownTime = 10f;           
    }

    public override void Activate(CharacterMovement player)
    {
        if (!isActive)
        {
            originalAtk = player.atk;
            originalMoveSpeed = player.moveSpeed;

            player.atk = Mathf.RoundToInt(player.atk * attackMultiplier);
            player.moveSpeed = player.moveSpeed * moveSpeedMultiplier;

            isActive = true;
        }

        timer = duration;

        Debug.Log("Berserk activated! atk=" + player.atk + " moveSpeed=" + player.moveSpeed);
    }

    public override void UpdateSkill(CharacterMovement player)
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Deactivate(player);
        }
    }

    public override void Deactivate(CharacterMovement player)
    {
        if (!isActive) return;

        player.atk = originalAtk;
        player.moveSpeed = originalMoveSpeed;

        isActive = false;
        timer = 0f;

        Debug.Log("Berserk ended. atk=" + player.atk + " moveSpeed=" + player.moveSpeed);
    }
}