using UnityEngine;

public abstract class Ship : BaseEntity
{
    [Tooltip("Degrees per second")] public float rotationSpeed;
    public float speed;
    public abstract bool IsPlayerControlled { get; }
    private Transform Eye;
    public float MaxHealth;
    public float Health { get; private set; }
    private ShipWeapon[] weapons;
    public ShipTrail[] trails;

#if UNITY_EDITOR
    [ContextMenu("Magic")]
    public void Magic()
    {
        weapons = GetComponentsInChildren<ShipWeapon>();
        trails = GetComponentsInChildren<ShipTrail>();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    public override void OnDamaged(float dmg)
    {
        Health -= dmg;
        if (Health <= 0)
            Destroy(gameObject);
    }

    protected void Brake()
    {
        RB.velocity = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * Time.deltaTime);
    }

    public void Fire()
    {
        if (weapons == null)
            weapons = GetComponentsInChildren<ShipWeapon>();
        foreach (var q in weapons)
            q.Fire();
    }

    public void Update()
    {
        float q = 2.125f;
        float L = Vector3.Dot(transform.forward, RB.velocity.normalized) / q + (1 - 1 / q);
        var mag = RB.velocity.magnitude;
        for (int i = 0; i < trails.Length; i++)
            trails[i].SetTrailLent(L, mag);

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