using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : PoolableComponent
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
        if (Explosion != 0)
        {
            GameCore.Self.Explode(transform.position, Explosion, Team);
            GameCore.Self.MakeBoom(transform.position, Poolable.SmallExplosion, 1);//TODO formula of explosion size
        }
        gameObject.SetActive(false);
    }

    public override void ReInit() { }
}