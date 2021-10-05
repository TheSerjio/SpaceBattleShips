using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShipTrail : MonoBehaviour
{
    public float MaxLentgh;
    public float VelocityAffect;

    private LineRenderer lr;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void SetTrailLent(float dir, float speed)
    {
        var q = speed / (speed + VelocityAffect);
        lr.SetPosition(0, dir * MaxLentgh * q * Vector3.back);
    }
}