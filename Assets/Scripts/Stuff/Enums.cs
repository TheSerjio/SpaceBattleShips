public enum Team : byte
{
    Defenders,
    Attackers,
    Derelict,
    Pirates,
    /// <summary>
    /// Entity cant have this team
    /// </summary>
    _SYSTEM_
}

public enum ExplosionType : byte
{
    Large, Small
}
public enum Poolable : byte
{
    FlatProjectile, ParticleEffectProjectile, SmallExplosion, ShipExplosion
}