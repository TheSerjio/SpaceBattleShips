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
    [SerializeField]private Vector3 to;

    public override void Warn(Vector3 moveTo, Ship.Warning how)
    {
        to = moveTo;
    }

    protected void LookAt(Vector3 where)
    {
        //yes, 8
        to = Vector3.MoveTowards(to, where, 8 * Time.deltaTime * Vector3.Distance(to, where));
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