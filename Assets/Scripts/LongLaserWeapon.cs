using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LongLaserWeapon : ShipWeapon
{
    private LineRenderer lr;

    private float q;

    public override float AntiSpeed => 0;

    public float damagePerSecond;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        q = lr.widthMultiplier;
    }

    public void FixedUpdate()
    {
        if (Parent.Fire)
        {
            lr.widthMultiplier = q;
            lr.positionCount = 2;
            lr.SetPositions(new Vector3[] { transform.position, transform.position + (transform.forward * ushort.MaxValue) });
            foreach (var hit in Physics.RaycastAll(transform.position, transform.forward))
            {
                var obj = hit.collider.gameObject;
                var tar = obj.GetComponentInParent<BaseEntity>();
                if (tar)
                {
                    if (tar.team != Parent.team)
                    {
                        tar.OnDamaged(damagePerSecond * Time.deltaTime, Parent);
                        lr.SetPositions(new Vector3[] { transform.position, hit.point });
                        break;
                    }
                }
            }
        }
        else
            lr.widthMultiplier = Mathf.MoveTowards(lr.widthMultiplier, 0, Time.deltaTime);
    }
}