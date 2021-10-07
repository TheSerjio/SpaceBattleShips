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

    public abstract void Warn(Vector3 moveTo);
}

public abstract class ShipAIController : ShipController
{
    private Vector3 target;

    public sealed override void Warn(Vector3 moveTo)
    {
        target = moveTo;
    }

    protected void LookAt(Vector3 where)
    {
        target = Vector3.MoveTowards(target, where, Time.deltaTime * Vector3.Distance(target, where));
        Ship.LookAt(target);
    }
}