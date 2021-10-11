using UnityEngine;

public sealed class SimpleWeapon : ShipWeapon
{
    public float ReloadTime;

    private float CoolDown;

    public float bulletLifeTime;

    public float bulletSpeed;

    public override float AntiSpeed => 1 / bulletSpeed;

    public float damage;

    public Color MainProjectileColor;

    public Color EdgeProjectileColor;

    public float ProjecileLentgh;

    public float ProjectileSize;

    public void Update()
    {
        if (CoolDown > 0)
            CoolDown -= Time.deltaTime;
        else if (Parent.Fire)
        {
            CoolDown += ReloadTime;
            Shoot();
        }
        else
            CoolDown = 0;
    }

    private void Shoot()
    {
        var q = ProjectilePool.Self.Get();
        q.transform.SetPositionAndRotation(transform.position + (transform.forward * ProjecileLentgh), transform.rotation);
        var p = q.GetComponent<Projectile>();
        p.team = Parent.team;
        p.Parent = Parent;
        p.LifeTime = bulletLifeTime;
        p.Damage = damage;
        var line = q.GetComponent<LineRenderer>();
        line.material.SetColor("MainColor", MainProjectileColor);
        line.material.SetColor("EdgeColor", EdgeProjectileColor);
        line.SetPositions(new Vector3[] { Vector3.back * ProjecileLentgh, Vector3.forward * ProjecileLentgh });
        line.widthMultiplier = ProjectileSize;
        var capsule = q.GetComponent<CapsuleCollider>();
        capsule.height = ProjecileLentgh;
        capsule.radius = ProjectileSize;
        var rb = q.GetComponent<Rigidbody>();
        rb.velocity = Parent.RB.velocity + (transform.forward * bulletSpeed);
        var trail = q.GetComponent<TrailRenderer>();
        if (trail)
            trail.AddPosition(transform.position);
    }
}