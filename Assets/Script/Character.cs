using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    public int maxHp = 100;
    public int currentHp;

    public int atk;
    public int def;

    public bool isDead = false;

    protected virtual void Start()
    {
        currentHp = maxHp;
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int finalDamage = Mathf.Clamp(damage - def, 1, damage);
        currentHp -= finalDamage;

        Debug.Log($"{gameObject.name} took {finalDamage} damage");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} is Dead");
    }

}
