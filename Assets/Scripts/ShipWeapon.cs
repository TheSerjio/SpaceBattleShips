using UnityEngine;

public abstract class ShipWeapon : MonoBehaviour
{
    public Ship Parent
    {
        get
        {
            if (!ship)
                ship = GetComponentInParent<Ship>();
            return ship;
        }
    }
    private Ship ship;

    public abstract float AntiSpeed { get; }
}