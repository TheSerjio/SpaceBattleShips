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
            Ship.LookAt(target.transform.position + (a * distance * (target.RB.velocity - RB.velocity)));
            float direction = Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized);
            //ship.Brake();
            Ship.Fire = direction > 0.5f;

            // 8 is fine
            var limit = Vector3.Distance(transform.position, target.transform.position) / 2;

            if (Vector3.Dot(transform.forward, RB.velocity - target.RB.velocity) < limit)
                Ship.Forward();
            if (RB.velocity.magnitude > limit * 2)
                Ship.Brake();
        }
    }
}