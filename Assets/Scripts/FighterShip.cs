using UnityEngine;

public class FighterShip : ShipAIController
{
    public Rigidbody target;

    public ShipWeapon mainWeapon;

    public bool isLaser;

    public float LaserAccuracy;

    public void FixedUpdate()
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
            if (isLaser)
            {
                Ship.Fire = Vector3.Dot(transform.forward, (target.position - transform.position).normalized) > 0;
                LookAt(target.position + Random.insideUnitSphere * Vector3.Distance(transform.position, target.position) / LaserAccuracy);
            }
            else
            {
                float distance = Vector3.Distance(target.position, transform.position);
                Vector3 targetPos = target.position + (a * distance * target.velocity);
                Ship.EyeA.LookAt(targetPos);
                Ship.EyeB.LookAt(RB.position + RB.velocity);
                Ship.EyeA.LookAt(Ship.EyeA.position + (Ship.EyeA.forward * 2 - Ship.EyeB.forward));
                float direction = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);
                //ship.Brake();
                LookAt(transform.position + Ship.EyeA.forward);
                Ship.Fire = direction > 0.5f;
            }
            var limit = Mathf.Sqrt(Vector3.Distance(transform.position, target.position)) + target.velocity.magnitude;

            if (Vector3.Dot(transform.forward, RB.velocity) < limit)
                Ship.Forward();
            if (RB.velocity.magnitude > limit * 2)
                Ship.Brake();
        }
        else
        {
            var obj = Utils.Choice(System.Array.FindAll(FindObjectsOfType<Ship>(), (Ship s) => s.team != Ship.team));
            if (obj)
                target = obj.RB;
            Ship.Fire = false;
        }
    }
}