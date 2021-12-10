using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LongLaserWeapon : ShipWeapon
{
    private LineRenderer lr;

    private float q;

    public override float FrameDistance => Accuracy * 2;
    public override float S_Bullets => 1;
    public override float AntiSpeed => 0;

    public float damagePerSecond;

    public float laserWidth;
    public float playerLaserWidth;

    public float EnergyPerSecond;

    public float Accuracy;

    private float NoShootUntil;

    private bool wasFiring;

    public SoundClip sound;

    private float seed;
    
    public void Start()
    {
        NoShootUntil = Time.time + Utils.StartTime;
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        q = lr.widthMultiplier;
        seed = Random.value * 9 * 11 * 13;
    }

    public void Update()
    {
        var b = true;
        if (Parent.Target.Fire)
        {
            if (Time.time < NoShootUntil)
                goto end;
            if (Parent.TakeEnergy(Time.deltaTime * EnergyPerSecond))
            {
                lr.widthMultiplier = q;
                lr.positionCount = 2;
                var randy = transform.forward + (Utils.TimeRandomSphere(seed) / Accuracy);
                var hitpoint = transform.position + randy * ushort.MaxValue;
                lr.SetPosition(0,Vector3.zero);
                
                foreach (var hit in Physics.SphereCastAll(transform.position, Parent.UseCheats ? playerLaserWidth : laserWidth, randy))
                {
                    var obj = hit.collider.gameObject;
                    var tar = obj.GetComponentInParent<BaseEntity>();
                    if (tar && tar.team != Parent.team)
                    {
                        tar.OnDamaged(damagePerSecond * Time.deltaTime, Parent);
                        hitpoint = hit.point;
                        break;
                    }
                }

                lr.SetPosition(1, transform.InverseTransformPoint(hitpoint));
                b = false;
            }
        }
        else if (wasFiring)
        {
            NoShootUntil = Time.time + 1;
        }

        end:
        wasFiring = Parent.Target.Fire;
        if (b)
            lr.widthMultiplier = Mathf.MoveTowards(lr.widthMultiplier, 0, Time.deltaTime);
        AudioManager.Self.SetLaserSound(sound.clip, lr.widthMultiplier * sound.volume);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Ttransform.position, laserWidth);
        Gizmos.DrawWireSphere(Ttransform.position, playerLaserWidth);

        Gizmos.color = Color.cyan;
        const float D = 100;
        foreach (var vec in new[] {Ttransform.up, Ttransform.right, -Ttransform.up, -Ttransform.right})
            Gizmos.DrawLine(Ttransform.position, (Ttransform.forward + vec / Accuracy) * D);
    }

    public override bool IsOutOfRange(float distance) => false;

    public override float S_MaxDPS() => damagePerSecond;
    public override float S_EnergyConsumption => EnergyPerSecond;
}