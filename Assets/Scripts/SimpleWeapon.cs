using UnityEngine;

public sealed class SimpleWeapon : ShipWeapon
{
    public GameObject Projectile;

    public float bulletLifeTime;

    public float bulletSpeed;

    protected override void DoFire()
    {
        var q = Instantiate(Projectile);
        q.transform.position = transform.position + transform.forward;
        q.transform.rotation = transform.rotation;
        var p = q.GetComponent<Projectile>();
        p.team = Parent.team;
        Destroy(q, bulletLifeTime);
        var rb = q.GetComponent<Rigidbody>();
        rb.velocity = Parent.RB.velocity + (transform.forward * bulletSpeed);
    }
}