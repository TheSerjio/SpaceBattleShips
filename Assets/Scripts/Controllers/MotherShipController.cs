public class MotherShipController : ShipAIController
{
    public override void OnFixedUpdate()
    {
        Ship.AutoBrake();
        Ship.Fire = true;
    }
}