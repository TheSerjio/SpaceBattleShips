public class FighterShip : ShipAIController
{
    public override void OnFixedUpdate()
    {
        var dir = Ship.LookAt(Utils.ShootTo(RB, target.RB, Ship.mainWeapon.AntiSpeed));

        if (dir < 0f)
        {
            Ship.Brake();
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