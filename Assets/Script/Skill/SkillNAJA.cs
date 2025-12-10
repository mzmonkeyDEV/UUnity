using UnityEngine;

public abstract class SkillNAJA
{
    public string skillName;
    public float cooldownTime = 1f;
    public float lastUsedTime = -999f;
    public float timer;

    public bool IsReady(float currentTime)
    {
        return currentTime - lastUsedTime >= cooldownTime;
    }

    public void TimeStampSkill(float currentTime)
    {
        lastUsedTime = currentTime;
    }
    public abstract void Activate(CharacterMovement player);
    public abstract void UpdateSkill(CharacterMovement player);
    public virtual void Deactivate(CharacterMovement player) { }
}