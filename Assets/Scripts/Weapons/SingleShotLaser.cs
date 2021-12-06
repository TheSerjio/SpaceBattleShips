using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SingleShotLaser : ShipWeaponWithCoolDown
{
    private LineRenderer lr;

    private float q;

    public override float AntiSpeed => 0;

    public override float S_EnergyConsumption => EnergyPerShot / ReloadTime;

    public float laserWidth;

    public float playerLaserWidth;
    public override float FrameDistance => 10000;
    public override float S_Bullets => 1f / ReloadTime;

    protected override void OnStart()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        q = lr.widthMultiplier;
    }

    protected override void Shoot()
    {
        lr.widthMultiplier = q;
        lr.positionCount = 2;
        bool kek = true;
        lr.SetPosition(0, transform.position);
        foreach (var hit in Physics.SphereCastAll(transform.position, Parent.UseCheats ? playerLaserWidth : laserWidth, transform.forward))
        {
            var obj = hit.collider.gameObject;
            var tar = obj.GetComponentInParent<BaseEntity>();
            if (tar)
            {
                if (tar.team != Parent.team)
                {
                    tar.OnDamaged(Damage, Parent);
                    lr.SetPosition(1, hit.point);
                    kek = false;
                    break;
                }
            }
        }
        if (kek)
            lr.SetPosition(1, transform.position + (transform.forward * ushort.MaxValue));
    }

    protected override void OnUpdate()
    {
        lr.widthMultiplier = Mathf.MoveTowards(lr.widthMultiplier, 0, Time.deltaTime / 4f);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Ttransform.position, laserWidth);
        Gizmos.DrawWireSphere(Ttransform.position, playerLaserWidth);
    }

    public override bool IsOutOfRange(float distance) => false;
}