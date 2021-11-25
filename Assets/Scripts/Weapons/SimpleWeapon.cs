using UnityEngine;

public class SimpleWeapon : ShipWeaponWithCoolDown
{
    public enum ProjectileType { Flat, Effect }

    public float bulletLifeTime;

    public float bulletSpeed;

    public override float AntiSpeed => 1 / bulletSpeed;

    public Color MainProjectileColor;

    public Color EdgeProjectileColor;

    public float ProjecileLentgh;

    public float ProjectileSize;

    public ProjectileType type;

    public float ProjectileExplosionPower;

    const float PlayerCheat = 5;

    [Range(0, 1)] public float Inaccuracy;

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

    public override float S_EnergyConsumption => EnergyPerShot / ReloadTime;

    protected override void Shoot()
    {
        var p = GameCore.Self.GetFromPool<Projectile>(Converted);
        p.transform.SetPositionAndRotation(transform.position + (transform.forward * ProjecileLentgh), transform.rotation);
        p.Team = Parent.team;
        p.Parent = Parent;
        p.LifeTime = bulletLifeTime;
        p.Damage =  Damage;
        p.Explosion = ProjectileExplosionPower;
        if (type == ProjectileType.Flat)
        {
            var line = p.LR;
            line.material.SetColor(Utils.ShaderID(ShaderName.MainColor), MainProjectileColor);
            line.material.SetColor(Utils.ShaderID(ShaderName.EdgeColor), EdgeProjectileColor);
            line.SetPositions(new Vector3[]
                {Vector3.back * ProjecileLentgh / 2f, Vector3.forward * ProjecileLentgh / 2f});
            line.widthMultiplier = ProjectileSize;
        }
        else
        {
            /*var effect = q.GetComponent<UnityEngine.VFX.VisualEffect>();
            if (effect)
            {
                //TODO Effect customization
            }*/
        }
        
        p.Velocity = Parent.RB.velocity + (transform.forward + Random.insideUnitSphere * Inaccuracy) * bulletSpeed;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var q = (ProjecileLentgh - ProjectileSize) / 2f;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * q), ProjectileSize / 2f);
        Gizmos.DrawWireSphere(transform.position - (transform.forward * q), ProjectileSize / 2f);
        Gizmos.DrawLine(transform.position - (transform.forward * q), transform.position + (transform.forward * q));

        Gizmos.color = Color.cyan;
        foreach (var v in new Vector3[] { transform.up, transform.right, -transform.up, -transform.right })
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward + v * Inaccuracy) * ushort.MaxValue);
    }

    public override bool IsOutOfRange(float distance) => distance > bulletLifeTime * bulletSpeed;
}