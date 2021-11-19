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

    /// <summary>
    /// Does nothing by default, made mostly for players
    /// </summary>
    public virtual void Death() { }
}

public abstract class ShipAIController : ShipController
{
    private float time;

    private Vector3 to;

    public Ship target;

    public override void Warn(Vector3 moveTo, Ship.Warning how)
    {
        time = Time.time;
        to = moveTo;
    }

    public void Start()
    {
        time = Time.time - 60;
    }

    public void FixedUpdate()
    {
        if (Time.time - time < Spawner.time)
        {
            Ship.LookAt(to);
            Ship.ExtraForward();
            return;
        }
        else
        {
            if (!target)
            {
                target = GameCore.Self.FindTargetShip(Ship.team, transform.position);
                Ship.Fire = false;
                Ship.AutoBrake();
            }
            else
               OnFixedUpdate();
        }
    }


    /// <summary>
    /// target is nut null
    /// </summary>
    public abstract void OnFixedUpdate();
}