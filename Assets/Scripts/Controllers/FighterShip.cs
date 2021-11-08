public class FighterShip : ShipAIController
{
    public ShipWeapon mainWeapon;

    public override void OnFixedUpdate()
    {
        float a = 0;
        if (mainWeapon)
            a = mainWeapon.AntiSpeed;
        else
            mainWeapon = Ship.GetWeapon<ShipWeapon>();
        Ship.BrakePower = 1;
        Ship.EnginePower = 1;

        var dir = Ship.LookAt(Utils.ShootTo(RB, target.RB, a));

        if (dir < 0.5f)
        {
            Ship.Brake();
            Ship.Fire = false;
        }
        else
        {
            Ship.Forward();
            Ship.Fire = true;
        }
    }
}