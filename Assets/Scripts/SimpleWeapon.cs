using UnityEngine;

public class SimpleWeapon : ShipWeapon
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

    public ProjectilePool.Projectile type;

    public float ProjectileExplosionPower;

    public float EnergyPerShot;

    public void Start()
    {
        CoolDown = ReloadTime * Random.value * 2;
    }

    public void Update()
    {
        if (CoolDown > 0)
            CoolDown -= Time.deltaTime;
        else if (Parent.Fire)
        {
            if (Parent.Parent.TakeEnergy(EnergyPerShot))
            {
                CoolDown += ReloadTime;
                Shoot();
            }
        }
        else
            CoolDown = 0;
    }

    private void Shoot()
    {
        var q = ProjectilePool.Self.Get(type);
        q.transform.SetPositionAndRotation(transform.position + (transform.forward * ProjecileLentgh), transform.rotation);
        var p = q.GetComponent<Projectile>();
        p.Team = Parent.Parent.team;
        p.Parent = Parent.Parent;
        p.LifeTime = bulletLifeTime;
        p.Damage = damage;
        p.Explosion = ProjectileExplosionPower;
        {
            var line = q.GetComponent<LineRenderer>();
            if (line)
            {
                line.material.SetColor("MainColor", MainProjectileColor);
                line.material.SetColor("EdgeColor", EdgeProjectileColor);
                line.SetPositions(new Vector3[] { Vector3.back * ProjecileLentgh, Vector3.forward * ProjecileLentgh });
                line.widthMultiplier = ProjectileSize;
            }
        }
        {
            var effect = q.GetComponent<UnityEngine.VFX.VisualEffect>();
            if (effect)
            {


            }
        }
        var capsule = q.GetComponent<CapsuleCollider>();
        capsule.height = Parent.Parent.UseCheats ? ProjecileLentgh * 5 : ProjecileLentgh;
        capsule.radius = Parent.Parent.UseCheats ? ProjectileSize * 5 : ProjectileSize;
        p.Velocity = Parent.Parent.RB.velocity + (transform.forward * bulletSpeed);
        var trail = q.GetComponent<TrailRenderer>();
        if (trail)
            trail.AddPosition(transform.position);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(ProjectileSize, ProjectileSize, ProjecileLentgh));
    }
}