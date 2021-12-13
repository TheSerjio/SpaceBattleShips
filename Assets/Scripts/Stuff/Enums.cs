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
    EdgeColor,
    MainColor,
    E_Skin,
    Colorio
}

public enum StarType : byte
{
    No,
    Bad,
    Good,
    Best
}