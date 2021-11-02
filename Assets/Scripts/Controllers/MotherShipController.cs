using UnityEngine;

public class MotherShipController : ShipAIController
{
    public override void OnFixedUpdate()
    {
        Ship.AutoBrake();
    }
}