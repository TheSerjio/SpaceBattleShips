using UnityEngine;

public sealed class SimpleWeapon : ShipWeapon
{
    public GameObject Projectile;

    public float bulletLifeTime;


    protected override void DoFire()
    {
        var q = Instantiate(Projectile);
        var p = q.GetComponent<Projectile>();
        p.team = Parent.team;
    }
}