using UnityEngine;

public class Projectile : BaseEntity
{
    public float damage;

    public Rigidbody Target;

    public BaseEntity parent;

    public float LifeTime;

    public void FixedUpdate()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            Die();
    }

    public void Die()
    {
        LifeTime = 1;
        ProjectilePool.Self.Add(gameObject);
    }

    public override void OnDamaged(float dmg, BaseEntity from) => Die();

    public void OnTriggerEnter(Collider other)
    {
        var q = other.GetComponentInParent<BaseEntity>();
        if (q)
            if (q.team != team)
            {
                q.OnDamaged(damage, parent);
                Die();
            }
    }
    public void OnCollisionEnter(Collision other) => OnTriggerEnter(other.collider);
}