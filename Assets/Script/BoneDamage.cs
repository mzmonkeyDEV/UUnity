using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDamage : MonoBehaviour
{
    public int ExtraDamage = 0;
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
                target.TakeDamage(ExtraDamage);
            }

            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            return;
        }

        Destroy(gameObject);

    }
}
