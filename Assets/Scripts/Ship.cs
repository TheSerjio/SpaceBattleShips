using UnityEngine;

public abstract class Ship : BaseEntity
{
    [Tooltip("Degrees per second")] public float rotationSpeed;
    public float speed;
    public abstract bool IsPlayerControlled { get; }
    private Transform Eye;
    public TrailRenderer[] trails;
    public float lentghOfTrails;
    public float MaxHealth;
    public float Health { get; private set; }

    [ContextMenu("Add trails")]
    public void AddTrails()
    {
        trails = GetComponentsInChildren<TrailRenderer>();
    }

    public override void OnDamaged(float dmg)
    {
        Health -= dmg;
        if (Health <= 0)
            Destroy(gameObject);
    }

    public void ConfigureTrails(bool emit)
    {

        for (int i = 0; i < trails.Length; i++)
        {
            var it = trails[i];
            if (it)
                it.emitting = emit;
            else
            {
                var list = new System.Collections.Generic.List<TrailRenderer>();
                foreach (var q in trails)
                    if (q)
                        list.Add(q);
                trails = list.ToArray();
                break;
            }
        }
    }

    public void Update()
    {
        OnUpdate();
    }

    public abstract void OnUpdate();

    public void Start()
    {
        OnStart();
    }

    public abstract void OnStart();

#if DEBUG
    public void OnDrawGizmos()
    {
        if (RB)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(RB.position, RB.position + RB.velocity);
        }
    }
#endif

    protected void LookAt(Vector3 worldPoint)
    {
        if (Eye == null)
        {
            Eye = new GameObject("systemEye").transform;
            Eye.parent = transform;
            Eye.localEulerAngles = Vector3.zero;
            Eye.localPosition= Vector3.zero;
            Eye.localScale = Vector3.one;
        }
        Eye.LookAt(worldPoint);
        var z = transform.eulerAngles.z;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Eye.rotation, rotationSpeed * Time.deltaTime);
        var e = transform.eulerAngles;
        e.z = z;
        transform.eulerAngles = e;
    }

}