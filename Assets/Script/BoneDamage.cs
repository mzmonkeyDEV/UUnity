using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDamage : MonoBehaviour
{
    public int baseDamage = 10;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character target = other.GetComponent<Character>();
            if (target != null)
            {
                target.TakeDamage(baseDamage);
            }

            Destroy(gameObject);
            return;
        }

    }
}
