using UnityEngine;

public class FighterShip : Ship
{
    private Rigidbody target;

    public ShipWeapon mainWeapon;

    public override void WhenDestroy() { }

    public override void OnStart() { }

    public override void OnFixedUpdate()
    {
        if (target)
        {
            float a = 0;
            if (mainWeapon)
                a = mainWeapon.AntiSpeed;
            else
                mainWeapon = GetWeapon<ShipWeapon>();
             float distance = Vector3.Distance(target.position, transform.position);
            Vector3 targetPos = target.position + (a * distance * target.velocity);
            EyeA.LookAt(targetPos);
            EyeB.LookAt(RB.position + RB.velocity);
            EyeA.LookAt(EyeA.position + (EyeA.forward * 2 - EyeB.forward));
            float direction = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);
            BrakePower = 1 - (distance / (distance + 10));
            Brake();
            LookAt(transform.position + EyeA.forward);
                if (direction > 0.5f)
                    Fire();
            EnginePower = direction / 4 + 0.5f;
            Forward();
            ConfigTrails(1);
        }
        else
        {
            var obj = Utils.Choice(System.Array.FindAll(FindObjectsOfType<Ship>(), (Ship s) => s.team != team));
            if (obj)
                target = obj.RB;
        }
    }

    public override void OnUpdate() { }
}