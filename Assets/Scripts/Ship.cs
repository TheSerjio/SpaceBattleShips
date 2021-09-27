using UnityEngine;

public abstract class Ship : BaseEntity
{
    [Tooltip("Degrees per second")] public float rotationSpeed;
    public float speed;
    protected Transform EyeA { get; private set; }
    protected Transform EyeB { get; private set; }
    public float MaxHealth;
    public float Health { get; private set; }
    [SerializeField] private ShipWeapon[] weapons;
    [SerializeField] private ShipTrail[] trails;
    public float EnginePower { get; protected set; }
    public float BrakePower { get; protected set; }
    public float ShieldPower { get; protected set; }

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
        else
            Debug.Log($"{name}: {Health} health left!");
    }

    protected void Forward()
    {
        RB.velocity += speed * EnginePower * Time.deltaTime * transform.forward;
    }

    protected T GetWeapon<T>() where T : ShipWeapon
    {
        foreach (var q in weapons)
            if (q is T t)
                return t;
        return null;
    }

    protected void Brake()
    {
        RB.velocity = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * BrakePower * Time.deltaTime);
    }

    public void Fire()
    {
        if (weapons == null)
            weapons = GetComponentsInChildren<ShipWeapon>();
        foreach (var q in weapons)
            q.Fire();
    }

    protected void ConfigTrails(float power)
    {
        float q = 2.125f;
        float L = Vector3.Dot(transform.forward, RB.velocity.normalized) / q + (1 - 1 / q);
        float mag = RB.velocity.magnitude;
        for (int i = 0; i < trails.Length; i++)
            trails[i].SetTrailLent(L, mag * power);
    }

    public void Update()
    {
            OnUpdate();
    }

    public abstract void OnUpdate();

    public abstract void OnStart();

    public abstract void OnFixedUpdate();

    private Transform CreateEye(string q)
    {
        var obj = new GameObject(q).transform;
        obj.parent = transform;
        obj.localPosition = Vector3.zero;
        obj.localEulerAngles = Vector3.zero;
        obj.localScale = Vector3.one;
        return obj;
    }

    public void Start()
    {
        EnginePower = 1;
        BrakePower = 1;
        ShieldPower = 1;
        EyeA = CreateEye("eye a");
        EyeB = CreateEye("eye b");
        OnStart();
        Health = MaxHealth;
    }

    public void FixedUpdate()
    {
            OnFixedUpdate();
    }

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
        transform.RotateTowards(worldPoint, rotationSpeed * Time.deltaTime);
    }
}