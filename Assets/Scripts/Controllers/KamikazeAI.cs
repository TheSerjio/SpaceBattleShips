using UnityEngine;

public class KamikazeAI : ShipAIController
{
    public override void OnFixedUpdate()
    {
//        var a = new Vector2(Mathf.Cos(Time.time), Mathf.Sin(Time.time));

        var tar = Utils.ShootTo(RB, Target.RB,
            1f / (Vector3.Distance(transform.position, Target.transform.position) + 1));

        Ship.EnginePower = 1;
        
        Ship.Forward();

        Ship.LookAt(tar);


    }
}