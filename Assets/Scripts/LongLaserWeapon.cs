using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LongLaserWeapon : ShipWeapon
{
    private LineRenderer lr;

    private float q;

    public override float AntiSpeed => 0;

    public float damagePerSecond;

    public float laserWidth;
    public float playerLaserWidth;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        q = lr.widthMultiplier;
    }

    public void FixedUpdate()
    {
        if (Parent.Fire)
        {
            lr.widthMultiplier = q;
            lr.positionCount = 2;
            lr.SetPositions(new Vector3[] { Vector3.zero, Vector3.forward * ushort.MaxValue });
            foreach (var hit in Physics.SphereCastAll(transform.position, laserWidth, transform.forward))
            {
                var obj = hit.collider.gameObject;
                var tar = obj.GetComponentInParent<BaseEntity>();
                if (tar)
                {
                    if (tar.team != Parent.team)
                    {
                        tar.OnDamaged(damagePerSecond * Time.deltaTime, Parent);
                        lr.SetPositions(new Vector3[] { Vector3.zero, Vector3.forward * hit.distance });
                        break;
                    }
                }
            }
        }
        else
            lr.widthMultiplier = Mathf.MoveTowards(lr.widthMultiplier, 0, Time.deltaTime);
    }
}