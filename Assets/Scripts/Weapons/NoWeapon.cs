public sealed class NoWeapon : ShipWeapon
{
    public override float AntiSpeed => 0;
    public override float MaxFireDist => 0;

    public override bool IsOutOfRange(float distance)
    {
        return true;
    }

    public override float MaxDPS()
    {
        return 0;
    }
}