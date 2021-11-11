using UnityEngine;

public abstract class ShipWeapon : MonoBehaviour
{
    public IFireControl Parent
    {
        get
        {
            if (ship == null)
                ship = GetComponentInParent<Ship>();
            return ship;
        }
        set
        {
            ship = value;
        }
    }
    private IFireControl ship;

    public abstract float AntiSpeed { get; }
}