using UnityEngine;

public class ShipDefenderAI : ShipAIController
{
    public Ship Object;

    private int index;

    public float Dist;

    public override void OnStart()
    {
        if (!Object)
            foreach (var q in FindObjectsOfType<MotherShip>())
                if (q.team == Ship.team)
                    Object = q;
    }

    public override void OnFixedUpdate()
    {
        if (Object)
        {
            if (Object.Teammates[index] != Ship)
            {
                var team = Object.Teammates;
                for (var i = 0; i < team.Length; i++)
                    if (!team[i])
                    {
                        team[i] = Ship;
                        index = i;
                        break;
                    }
            }
            var pos = Object.Formation[index].position;
            if (Vector3.Distance(pos, transform.position) > Dist)
            {
                Ship.LookAt(pos);
                if (Vector3.Dot(transform.forward, (pos-transform.position).normalized) < 0.9f)
                {
                    Debug.Log("braking");
                    Ship.Brake(false);
                }
                else
                {
                    Debug.Log("forward");
                    Ship.EnginePower = 1;
                    Ship.Forward();
                }
                return;
            }
            else
            {
                Ship.Brake(false);
            }
        }

        Ship.Target.Fire = Vector3.Dot(transform.forward, (Target.transform.position - transform.position).normalized) > 0.99f;
        Ship.LookAt(Utils.ShootTo(RB, Target.RB, Ship.mainWeapon.AntiSpeed,1));
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Dist);
    }
}