using UnityEngine;

public sealed class AutoWeapon : Script
{
    public float rotationSpeed;
    public Transform body;
    private Ship parent;
    public ShipWeapon weapon;

    [Tooltip("less value -> more rotation")] [Range(-1, 1)]
    public float maxAngle;

    protected override void OnAwake()
    {
        parent = GetComponentInParent<Ship>();
    }

    public void Update()
    {
        parent.Target.OperateAutoWeapon(parent, this);
        while (Vector3.Dot(transform.forward, body.forward) < maxAngle)
        {
            body.RotateTowards(body.position + transform.forward, Time.deltaTime * rotationSpeed, false);
        }
    }
}