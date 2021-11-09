using UnityEngine;

public class Projectile : BaseEntity
{
    public float Damage { get; set; }

    public Rigidbody Target { get; set; }

    public BaseEntity Parent { get; set; }

    public float LifeTime { get; set; }

    public float Explosion { get; set; }

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
        if (Explosion != 0)
        {
            GameCore.Self.Explode(transform.position, Explosion, null);
            var boom = Instantiate(DataBase.Get().SmallExplosion, transform.position, Random.rotation);
            Destroy(boom, 10);
        }
    }

    public override void OnDamaged(float dmg, BaseEntity from) => Die();

    public void OnTriggerEnter(Collider other)
    {
        var q = other.GetComponentInParent<BaseEntity>();
        if (q)
            if (q.team != team)
            {
                q.OnDamaged(Damage, Parent);
                Die();
            }
    }

    public override void DeathDamage()
    {
        Die();
    }
}