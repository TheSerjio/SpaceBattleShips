public enum Team : byte
{
    Defenders,
    Attackers,
    Derelict,
    Pirates,

    /// <summary>
    /// Entity cant have this team
    /// </summary>
    SYSTEM,
    Player
}
public enum Poolable : byte
{
    FlatProjectile,
    ParticleEffectProjectile,
    SmallExplosion,
    ShipExplosion
}
public enum ShaderName : byte
{
    Alpha,
    Damage,
}