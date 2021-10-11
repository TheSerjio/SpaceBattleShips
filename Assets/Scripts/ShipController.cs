using UnityEngine;

[DisallowMultipleComponent]
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
    /// Does nothing by default
    /// </summary>
    public virtual void Death() { }
}

public abstract class ShipAIController : ShipController
{
    private float time;

    private Vector3 to;

    public override void Warn(Vector3 moveTo, Ship.Warning how)
    {
        time = Time.time;
        to = moveTo;
    }

    protected void LookAt(Vector3 where)
    {
        if (Time.time - time > 1)
            Ship.LookAt(where);
        else
            Ship.LookAt(to);
    }
}

public abstract class ShipSimpleAIController : ShipAIController
{
    public Ship target;

    public void FixedUpdate()
    {
        if (!target)
        {
            var obj = Utils.Choice(System.Array.FindAll(FindObjectsOfType<Ship>(), (Ship s) => s.team != Ship.team));
            if (obj)
                target = obj;
            Ship.Fire = false;
        }
        OnFixedUpdate();
    }

    public abstract void OnFixedUpdate();
}