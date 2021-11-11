using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : MonoBehaviour
{
    public float Damage { get; set; }

    public Rigidbody Target { get; set; }

    public BaseEntity Parent { get; set; }

    public float LifeTime { get; set; }

    public float Explosion { get; set; }

    public Vector3 Velocity { get; set; }

    public Team Team { get; set; }

    public void FixedUpdate()
    {
        transform.position += Velocity * Time.deltaTime;
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
}