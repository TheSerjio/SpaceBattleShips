using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LongLaserWeapon : ShipWeapon
{
    private LineRenderer lr;

    private float q;

    public override float AntiSpeed => 0;

    public override float MaxFireDist => Accuracy;

    public float damagePerSecond;

    public float laserWidth;
    public float playerLaserWidth;

    public float EnergyPerSecond;

    public float Accuracy;

    private float NoShootUntil;

    private bool wasFiring;

    public void Start()
    {
        NoShootUntil = Time.time + Spawner.time;
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        q = lr.widthMultiplier;
    }

    public void Update()
    {
        bool b = true;
        if (Parent.Fire)
        {
            if (Time.time < NoShootUntil)
                goto end;
            if (Parent.Parent.TakeEnergy(Time.deltaTime * EnergyPerSecond))
            {
                lr.widthMultiplier = q;
                lr.positionCount = 2;
                Vector3 randy = transform.forward + (Random.insideUnitSphere / Accuracy);
                bool kek = true;
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
                            lr.SetPosition(1, transform.InverseTransformPoint(hit.point));
                            kek = false;
                            break;
                        }
                    }
                }
                if (kek)
                    lr.SetPosition(1, Vector3.forward * ushort.MaxValue);
                b = false;
            }
        }
        else if (wasFiring)
        {
            NoShootUntil = Time.time + 1;
        }

        end:
        wasFiring = Parent.Fire;
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

    public override float MaxDPS() => damagePerSecond;
}