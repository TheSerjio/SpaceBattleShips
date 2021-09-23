using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LongLaserWeapon : ShipWeapon
{
    private LineRenderer lr;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
    }

    protected override void DoFire()
    {
        lr.widthMultiplier = 1;
        lr.positionCount = 2;
        if (Physics.Raycast(transform.position, transform.forward, out var hit))
        {
            lr.SetPositions(new Vector3[] { transform.position, hit.point });
            Debug.Log("yep");
        }
        else
        {
            lr.SetPositions(new Vector3[] { transform.position, transform.position + (transform.forward * ushort.MaxValue) });
            Debug.Log("fail");
        }
    }

    void Update()
    {
        lr.widthMultiplier = Mathf.MoveTowards(lr.widthMultiplier, 0, Time.deltaTime);
    }
}