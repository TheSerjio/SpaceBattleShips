public class FighterShip : ShipAIController
{
    public override void OnFixedUpdate()
    {
        var dir = Ship.LookAt(Utils.ShootTo(RB, target.RB, Ship.mainWeapon.AntiSpeed));

        if (dir < 0.9f)
        {
            Ship.Brake(false);
            Ship.Fire = false;
        }
        else
        {
            var dot = UnityEngine.Vector3.Dot(transform.forward, RB.velocity);
            if (dot < -2)
            {
                Ship.Brake(false);
                Ship.Fire = false;
            }
            else
            {
                Ship.EnginePower = dir;
                Ship.Forward();
                Ship.Fire = true;
            }
        }
    }
}