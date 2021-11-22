using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShipLineTrail : ShipTrail
{
    public float size;

    public LineRenderer Line { get; private set; }

    public void Awake()
    {
        Line = GetComponent<LineRenderer>();
    }

    public override void SetTrailLent(float speed)
    {
        Line.SetPosition(0, speed * Line.startWidth * 8 * Vector3.back);
    }
}