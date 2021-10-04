using UnityEngine;

public sealed class SimpleWeapon : ShipWeapon
{
    public float bulletLifeTime;

    public float bulletSpeed;

    public override float AntiSpeed => 1 / bulletSpeed;

    public float damage;

    public Color MainProjectileColor;

    public Color EdgeProjectileColor;

    protected override void DoFire()
    {
        var q = ProjectilePool.Self.Get();
        q.transform.SetPositionAndRotation(transform.position + transform.forward, transform.rotation);
        var p = q.GetComponent<Projectile>();
        p.team = Parent.team;
        p.Parent = Parent;
        p.LifeTime = bulletLifeTime;
        p.Damage = damage;
        var mat = q.GetComponent<LineRenderer>().material;
        mat.SetColor("MainColor", MainProjectileColor);
        mat.SetColor("EdgeColor", EdgeProjectileColor);
        var rb = q.GetComponent<Rigidbody>();
        rb.velocity = Parent.RB.velocity + (transform.forward * bulletSpeed);
        var trail = q.GetComponent<TrailRenderer>();
        if (trail)
            trail.AddPosition(transform.position);
    }
}