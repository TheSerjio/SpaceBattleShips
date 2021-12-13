using UnityEngine;

public class SimpleWeapon : ShipWeaponWithCoolDown
{
    public enum ProjectileType
    {
        Flat,
        Effect
    }

    public float bulletLifeTime;

    public float bulletSpeed;

    public override float AntiSpeed => 1 / bulletSpeed;

    public Color MainProjectileColor;

    public Color EdgeProjectileColor;

    public float ProjecileLentgh;

    public float ProjectileSize;

    public ProjectileType type;

    public float ProjectileExplosionPower;

    [Range(0, 1)] public float Inaccuracy;
    public override float FrameDistance => bulletLifeTime * bulletSpeed;
    public override float S_Bullets => bulletLifeTime / ReloadTime;

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

        p.Team = Parent.team;
        p.Parent = Parent;
        p.LifeTime = bulletLifeTime;
        p.Damage = Damage;
        p.Explosion = ProjectileExplosionPower;
        if (type == ProjectileType.Flat)
        {
            var line = p.LR;
            line.material.SetColor(Utils.ShaderID(ShaderName.MainColor), MainProjectileColor);
            line.material.SetColor(Utils.ShaderID(ShaderName.EdgeColor), EdgeProjectileColor);
            line.SetPositions(new[]
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

        p.Radius = Parent.UseCheats ? ProjectileSize * 2.5f : ProjectileSize / 2f;

        p.Velocity = Parent.RB.velocity + (transform.forward + Random.insideUnitSphere * Inaccuracy) * bulletSpeed;
        p.transform.LookAt(p.transform.position + p.Velocity);
        p.transform.position = transform.position + (transform.forward * ProjecileLentgh / 2f);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var q = (ProjecileLentgh - ProjectileSize) / 2f;
        var myPos = Ttransform.position;
        var forward = Ttransform.forward;
        Gizmos.DrawWireSphere(myPos + (forward * q), ProjectileSize / 2f);
        Gizmos.DrawWireSphere(myPos - (forward * q), ProjectileSize / 2f);
        Gizmos.DrawLine(myPos - (forward * q), myPos + (forward * q));

        Gizmos.color = Color.cyan;
        foreach (var v in new[] {Ttransform.up, Ttransform.right, -Ttransform.up, -Ttransform.right})
            Gizmos.DrawLine(Ttransform.position, myPos + (Ttransform.forward + v * Inaccuracy) * FrameDistance * 2f);
    }

    public override bool IsOutOfRange(float distance) => distance > bulletLifeTime * bulletSpeed;
}