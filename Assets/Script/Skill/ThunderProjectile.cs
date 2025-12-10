using UnityEngine;

public class ThunderProjectile : MonoBehaviour
{
    public int damage = 10;
    public Character owner;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile hit: " + other.name);

        Character target = other.GetComponentInParent<Character>();
        if (target == null) return;
        if (owner != null && target == owner) return;

        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}