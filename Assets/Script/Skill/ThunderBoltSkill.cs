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

        Vector3 dir = player.transform.forward;

        Camera cam = Camera.main;
        if (cam != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Vector3 target = hit.point;
                target.y = player.transform.position.y;

                Vector3 flatDir = target - player.transform.position;
                flatDir.y = 0f;

                if (flatDir.sqrMagnitude > 0.01f)
                    dir = flatDir.normalized;
            }
        }

        player.transform.rotation = Quaternion.LookRotation(dir);

        Vector3 spawnPos = player.transform.position + dir * 1.2f + Vector3.up * 1f;
        Quaternion spawnRot = Quaternion.LookRotation(dir);

        GameObject bolt = Object.Instantiate(projectilePrefab, spawnPos, spawnRot);

        Rigidbody rb = bolt.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = dir * projectileSpeed;

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