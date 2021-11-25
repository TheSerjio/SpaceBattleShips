using UnityEngine;

[RequireComponent(typeof(Ship))]
public abstract class ShipController : MonoBehaviour
{
    public Ship Ship { get; private set; }
    protected Rigidbody RB => Ship.RB;

    public void Awake()
    {
        Ship = GetComponent<Ship>();
    }

    public abstract void Warn(Vector3 moveTo, Ship.Warning how);
}

public abstract class ShipAIController : ShipController
{
    private float time;

    private Vector3 to;

    public Ship Target { get; set; }

    public Targeting.TargetRigidBody target;

    public override void Warn(Vector3 moveTo, Ship.Warning how)
    {
        time = Time.time;
        to = moveTo;
    }

    public Locating TargetFinding;

    public void Start()
    {
        time = Time.time - 60;
        target = new Targeting.TargetRigidBody(null);
        OnStart();
    }

    public virtual void OnStart()
    {
    }

    public void FixedUpdate()
    {
        Ship.Target = target;
        if (Time.time - time < Utils.StartTime)
        {
            Ship.LookAt(to);
            Ship.ExtraForward();
        }
        else
        {
            if (!Target)
            {
                Target = GameCore.Self.FindTargetShip(Ship.team, false, TargetFinding, transform);
                target.Fire = false;
                Ship.AutoBrake();
            }
            else
            {
                target.RB = Target.RB;
                OnFixedUpdate();
            }
        }
    }


    /// <summary>
    /// target is nut null
    /// </summary>
    public abstract void OnFixedUpdate();
} 