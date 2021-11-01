using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShipLineTrail : ShipTrail
{
    private LineRenderer lr;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public override void SetTrailLent(float speed)
    {
        lr.SetPosition(0, speed * size * Vector3.back);
    }
}