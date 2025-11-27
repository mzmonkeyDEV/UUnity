using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    public float hp;
    public float atk;
    public float def;

    public void TakeDamage()
    {

    }

    public bool IsDead()
    {
        return hp <= 0;
    }

}
