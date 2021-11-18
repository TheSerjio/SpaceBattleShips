using UnityEngine;

public class SimpleWeapon : ShipWeapon
{
    public enum ProjectileType { Flat, Effect }

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

    public ProjectileType type;

    public float ProjectileExplosionPower;

    public float EnergyPerShot;

    const float PlayerCheat = 5;

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

    private Poolable Converted
    {
        get
        {
            switch (type)
            {
                case ProjectileType.Flat:
                    return Poolable.FlatProjectile;
                case ProjectileType.Effect:
                    return Poolable.ParticleEffectProjectile;
                default:
                    Debug.LogError(type);
                    return default;
            }
        }
    }

    private void Shoot()
    {
        var q = GameCore.Self.GetFromPool(Converted);
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
                line.SetPositions(new Vector3[] { Vector3.back * ProjecileLentgh / 2f, Vector3.forward * ProjecileLentgh / 2f });
                line.widthMultiplier = ProjectileSize;
            }
        }
        {
            var effect = q.GetComponent<UnityEngine.VFX.VisualEffect>();
            if (effect)
            {
                //TODO Effect customization
            }
        }
        var capsule = q.GetComponent<CapsuleCollider>();
        capsule.height = Parent.Parent.UseCheats ? ProjecileLentgh * PlayerCheat : ProjecileLentgh;
        capsule.radius = (Parent.Parent.UseCheats ? ProjectileSize * PlayerCheat : ProjectileSize) / 2f;
        p.Velocity = Parent.Parent.RB.velocity + (transform.forward * bulletSpeed);
        var trail = q.GetComponent<TrailRenderer>();
        if (trail)
            trail.AddPosition(transform.position);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float q = (ProjecileLentgh - ProjectileSize) / 2f;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * q), ProjectileSize / 2f);
        Gizmos.DrawWireSphere(transform.position - (transform.forward * q), ProjectileSize / 2f);
        Gizmos.DrawLine(transform.position - (transform.forward * q), transform.position + (transform.forward * q));
    }

    public override bool IsOutOfRange(float distance) => distance > bulletLifeTime * bulletSpeed;
}