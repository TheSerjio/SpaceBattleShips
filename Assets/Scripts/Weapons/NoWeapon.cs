using UnityEngine;

public sealed class NoWeapon : ShipWeapon
{
    [SerializeField] private float Range;

    public override float AntiSpeed => 0;
    public override float S_EnergyConsumption => 0;
    public override float S_Bullets => 0;

    public override bool IsOutOfRange(float distance)
    {
        return distance > Range;
    }

    public override float S_MaxDPS()
    {
        return 0;
    }
}