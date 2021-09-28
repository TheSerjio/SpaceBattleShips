using UnityEngine;

public class FighterShip : ShipController
{
    public Rigidbody target;

    public ShipWeapon mainWeapon;

    public float SpeedLimit;

    public bool isLaser;

    public void FixedUpdate()
    {
        if (target)
        {
            float a = 0;
            if (mainWeapon)
                a = mainWeapon.AntiSpeed;
            else
                mainWeapon = ship.GetWeapon<ShipWeapon>();
            ship.BrakePower = 0.5f;
            ship.EnginePower = 1;
            if (isLaser)
            {
                ship.Fire();
                ship.LookAt(target.position);
            }
            else
            {
                float distance = Vector3.Distance(target.position, transform.position);
                Vector3 targetPos = target.position + (a * distance * target.velocity);
                ship.EyeA.LookAt(targetPos);
                ship.EyeB.LookAt(RB.position + RB.velocity);
                ship.EyeA.LookAt(ship.EyeA.position + (ship.EyeA.forward * 2 - ship.EyeB.forward));
                float direction = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);
                //ship.Brake();
                ship.LookAt(transform.position + ship.EyeA.forward);
                if (direction > 0.5f)
                    ship.Fire();
            }
            if (Vector3.Dot(transform.forward, RB.velocity) < SpeedLimit)
                ship.Forward();
        }
        else
        {
            var obj = Utils.Choice(System.Array.FindAll(FindObjectsOfType<Ship>(), (Ship s) => s.team != ship.team));
            if (obj)
                target = obj.RB;
        }
    }
}