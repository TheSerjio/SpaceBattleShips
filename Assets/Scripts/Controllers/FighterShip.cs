public class FighterShip : ShipAIController
{
    public ShipWeapon mainWeapon;

    public override void OnFixedUpdate()
    {
        var dir = Ship.LookAt(Utils.ShootTo(RB, target.RB, mainWeapon.AntiSpeed));

        if (dir < 0f)
        {
            Ship.BrakePower = 1;
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