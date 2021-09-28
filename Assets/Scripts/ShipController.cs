using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Ship))]
public abstract class ShipController : MonoBehaviour
{
    protected Ship ship;
    protected Rigidbody RB => ship.RB;

    public void Awake()
    {
        ship = GetComponent<Ship>();
    }
}