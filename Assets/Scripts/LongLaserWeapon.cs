using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LongLaserWeapon : ShipWeapon
{
    private LineRenderer lr;

    private float q;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        q = lr.widthMultiplier;
    }

    protected override void DoFire()
    {
        lr.widthMultiplier = q;
        lr.positionCount = 2;
        if (Physics.Raycast(transform.position, transform.forward, out var hit))
        {
            lr.SetPositions(new Vector3[] { transform.position, hit.point });
        }
        else
        {
            lr.SetPositions(new Vector3[] { transform.position, transform.position + (transform.forward * ushort.MaxValue) });
        }
    }

    void Update()
    {
        lr.widthMultiplier = Mathf.MoveTowards(lr.widthMultiplier, 0, Time.deltaTime);
    }
}