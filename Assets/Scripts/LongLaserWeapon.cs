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

    float noEnergyCoolDown;

    public float EnergyPerSecond;

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        q = lr.widthMultiplier;
    }

    public void FixedUpdate()
    {
        bool b = true;
        if (Parent.Fire)
        {
            if (Time.time > noEnergyCoolDown)
            {
                if (Parent.Parent.TakeEnergy(Time.deltaTime * EnergyPerSecond))
                {
                    lr.widthMultiplier = q;
                    lr.positionCount = 2;
                    lr.SetPositions(new Vector3[] { Vector3.zero, Vector3.forward * ushort.MaxValue });
                    foreach (var hit in Physics.SphereCastAll(transform.position, Parent.Parent.UseCheats ? playerLaserWidth : laserWidth, transform.forward))
                    {
                        var obj = hit.collider.gameObject;
                        var tar = obj.GetComponentInParent<BaseEntity>();
                        if (tar)
                        {
                            if (tar.team != Parent.Parent.team)
                            {
                                tar.OnDamaged(damagePerSecond * Time.deltaTime, Parent.Parent);
                                lr.SetPositions(new Vector3[] { Vector3.zero, Vector3.forward * hit.distance });
                                break;
                            }
                        }
                    }
                    b = false;
                }
                else
                {
                    noEnergyCoolDown = Time.time + 2;
                }
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
    }
}