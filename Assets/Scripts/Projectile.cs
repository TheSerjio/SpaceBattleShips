using UnityEngine;

public class Projectile : BaseEntity
{
    public float damage;

    public override void OnDamaged(float dmg)
    {
        Destroy();
    }

    public override void WhenDestroy() { }

    public void OnTriggerEnter(Collider other)
    {
        var q = other.GetComponentInParent<BaseEntity>();
        Destroy(gameObject);
    }
    public void OnCollisionEnter(Collision other) => OnTriggerEnter(other.collider);
}