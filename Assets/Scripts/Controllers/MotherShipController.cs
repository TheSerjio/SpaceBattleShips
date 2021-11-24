using UnityEngine;

public class MotherShipController : ShipController
{
    public void Update()
    {
        Ship.AutoBrake();
        Ship.Target.Fire = false;
    }

    public override void Warn(Vector3 moveTo, Ship.Warning how) { }
}