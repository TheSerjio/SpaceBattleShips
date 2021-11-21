using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SingleShotLaser : ShipWeaponWithCoolDown
{
    private LineRenderer lr;

    private float q;

    public override float AntiSpeed => 0;

    public float laserWidth;

    public float playerLaserWidth;

    public override void OnStart()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        q = lr.widthMultiplier;
    }

    public override void Shoot()
    {
        lr.widthMultiplier = q;
        lr.positionCount = 2;
        bool kek = true;
        lr.SetPosition(0, Vector3.zero);
        foreach (var hit in Physics.SphereCastAll(transform.position, Parent.Parent.UseCheats ? playerLaserWidth : laserWidth, transform.forward))
        {
            var obj = hit.collider.gameObject;
            var tar = obj.GetComponentInParent<BaseEntity>();
            if (tar)
            {
                if (tar.team != Parent.Parent.team)
                {
                    tar.OnDamaged(Damage, Parent.Parent);
                    lr.SetPosition(1, transform.InverseTransformPoint(hit.point));
                    kek = false;
                    break;
                }
            }
        }
        if (kek)
            lr.SetPosition(1, Vector3.forward * ushort.MaxValue);
    }

    public override void OnUpdate()
    {
        lr.widthMultiplier = Mathf.MoveTowards(lr.widthMultiplier, 0, Time.deltaTime);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, laserWidth);
        Gizmos.DrawWireSphere(transform.position, playerLaserWidth);
    }

    public override bool IsOutOfRange(float distance) => false;
}