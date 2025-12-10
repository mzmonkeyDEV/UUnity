using UnityEngine;

public class ThunderStrikeSkill : SkillNAJA
{
    public float range = 15f;
    public float damageMultiplier = 3f;
    public GameObject strikeEffectPrefab;

    public ThunderStrikeSkill()
    {
        skillName = "Thunder Strike";
        cooldownTime = 8f;
    }

    public override void Activate(CharacterMovement player)
    {
        Character target = FindClosestTarget(player);
        if (target == null)
            return;

        int dmg = Mathf.RoundToInt(player.atk * damageMultiplier);
        target.TakeDamage(dmg);

        if (strikeEffectPrefab != null)
        {
            Vector3 pos = target.transform.position + Vector3.up * 5f;
            RaycastHit hit;
            if (Physics.Raycast(pos, Vector3.down, out hit, 20f))
            {
                pos = hit.point;
            }
            else
            {
                pos = target.transform.position;
            }

            Object.Instantiate(strikeEffectPrefab, pos, Quaternion.identity);
        }

        timer = 0f;
    }

    public override void UpdateSkill(CharacterMovement player)
    {
    }

    Character FindClosestTarget(CharacterMovement player)
    {
        Collider[] hits = Physics.OverlapSphere(player.transform.position, range);
        Character closest = null;
        float minDist = Mathf.Infinity;

        for (int i = 0; i < hits.Length; i++)
        {
            Character c = hits[i].GetComponentInParent<Character>();
            if (c == null || c == player)
                continue;

            float d = Vector3.Distance(player.transform.position, c.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = c;
            }
        }

        return closest;
    }
}