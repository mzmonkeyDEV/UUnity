using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Character ownerStats;
    public int damageMultiplier = 1;

    private Collider hitbox;

    private void Awake()
    {
        hitbox = GetComponent<Collider>();
        hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character target = other.GetComponent<Character>();

        if (target != null && target != ownerStats)
        {
            int finalDamage = ownerStats.atk * damageMultiplier;
            target.TakeDamage(finalDamage);

            Debug.Log("Hit " + other.name + finalDamage + " damage");
        }
    }

    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }
}
