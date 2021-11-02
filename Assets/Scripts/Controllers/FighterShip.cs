using UnityEngine;

public class FighterShip : ShipAIController
{
    public ShipWeapon mainWeapon;

    public override void OnFixedUpdate()
    {
        if (target)
        {
            float a = 0;
            if (mainWeapon)
                a = mainWeapon.AntiSpeed;
            else
                mainWeapon = Ship.GetWeapon<ShipWeapon>();
            Ship.BrakePower = 1;
            Ship.EnginePower = 1;

            float distance = Vector3.Distance(target.transform.position, transform.position);
            var dir = Ship.LookAt(target.transform.position + (a * distance * (target.RB.velocity - RB.velocity)));
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
}