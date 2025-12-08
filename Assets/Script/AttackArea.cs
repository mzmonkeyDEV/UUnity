using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public Character ownerStats;

    private void OnTriggerEnter(Collider other)
    {
        Character target = other.GetComponent<Character>();

        if (target != null && target != ownerStats)
        {
            target.TakeDamage(ownerStats.atk);

            Debug.Log("Hit " + other.name + ownerStats.atk + " damage");
        }
    }
}
