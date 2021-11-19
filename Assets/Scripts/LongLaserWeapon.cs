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

    public float EnergyPerSecond;

    public float Accuracy;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        q = lr.widthMultiplier;
    }

    public void Update()
    {
        bool b = true;
        if (Parent.Fire)
        {
            if (Parent.Parent.TakeEnergy(Time.deltaTime * EnergyPerSecond))
            {
                lr.widthMultiplier = q;
                lr.positionCount = 2;
                Vector3 randy = transform.forward + (Random.insideUnitSphere / Accuracy);
                var point = transform.position + (randy * ushort.MaxValue);
                lr.SetPosition(0,Vector3.zero);
                foreach (var hit in Physics.SphereCastAll(transform.position, Parent.Parent.UseCheats ? playerLaserWidth : laserWidth, randy))
                {
                    var obj = hit.collider.gameObject;
                    var tar = obj.GetComponentInParent<BaseEntity>();
                    if (tar)
                    {
                        if (tar.team != Parent.Parent.team)
                        {
                            tar.OnDamaged(damagePerSecond * Time.deltaTime, Parent.Parent);
                            point = hit.point;
                            break;
                        }
                    }
                }
                lr.SetPosition(1, transform.InverseTransformPoint(point));
                b = false;
            }
        }
        if (b)
            lr.widthMultiplier = Mathf.MoveTowards(lr.widthMultiplier, 0, Time.deltaTime);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, laserWidth);
        Gizmos.DrawWireSphere(transform.position, playerLaserWidth);

        Gizmos.color = Color.cyan;
        float D = 100;
        foreach (var vec in new Vector3[] { transform.up, transform.right, -transform.up, -transform.right })
            Gizmos.DrawLine(transform.position, (transform.forward + vec / Accuracy) * D);
    }

    public override bool IsOutOfRange(float distance) => false;
}