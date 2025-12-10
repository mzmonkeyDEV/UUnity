using UnityEngine;

public class ThunderBoltSkill : SkillNAJA
{
    public float projectileSpeed = 20f;
    public float damageMultiplier = 2.5f;
    public GameObject projectilePrefab;

    public ThunderBoltSkill()
    {
        skillName = "Thunder Bolt";
        cooldownTime = 5f;
    }

    public override void Activate(CharacterMovement player)
    {
        if (projectilePrefab == null)
            return;

        Vector3 spawnPos = player.transform.position + player.transform.forward * 1.2f + Vector3.up * 1f;
        Quaternion spawnRot = Quaternion.LookRotation(player.transform.forward);

        GameObject bolt = Object.Instantiate(projectilePrefab, spawnPos, spawnRot);

        Rigidbody rb = bolt.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = player.transform.forward * projectileSpeed;

        ThunderProjectile proj = bolt.GetComponent<ThunderProjectile>();
        if (proj != null)
        {
            proj.damage = Mathf.RoundToInt(player.atk * damageMultiplier);
            proj.owner = player;
        }

        timer = 0f;

        Object.Destroy(bolt, 3f);
    }

    public override void UpdateSkill(CharacterMovement player)
    {
    }
}