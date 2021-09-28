using UnityEngine;

public sealed class Ship : BaseEntity
{
   // private ShipController brain;
    [Tooltip("Degrees per second")] public float rotationSpeed;
    public float speed;
    public Transform EyeA { get;private set; }
    public Transform EyeB { get;private set; }
    public float MaxHealth;
    public float Health { get; private set; }
    [SerializeField] private ShipWeapon[] weapons;
    [SerializeField] private ShipTrail[] trails;
    public float EnginePower { get;  set; }
    public float BrakePower { get;  set; }
    /// <summary>
    /// Legacy
    /// </summary>
    public float ShieldPower { get;  set; }
    /// <summary>
    /// Serializable
    /// </summary>
    public float MaxEnergy;
    public float Energy { get; private set; }
    [Tooltip("Per second")] [SerializeField] float EnergyRegeneration;
    /// <summary>
    /// Serializable
    /// </summary>
    public float MaxShield;
    public float Shield { get; private set; }
    [Tooltip("Per second")] [SerializeField] float ShieldRegeneration;
    private float EngineQ;
    [SerializeField] Renderer ShieldRender;
    [SerializeField] Collider ShieldCollider;
    private bool HasShield;

#if UNITY_EDITOR
    [ContextMenu("Magic")]
    public void Magic()
    {
        weapons = GetComponentsInChildren<ShipWeapon>();
        trails = GetComponentsInChildren<ShipTrail>();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    public void Update()
    {
        EngineQ = Mathf.MoveTowards(EngineQ, 0, Time.deltaTime);
        float q = 2.125f;
        float L = Vector3.Dot(transform.forward, RB.velocity.normalized) / q + (1 - 1 / q);
        float mag = RB.velocity.magnitude;
        for (int i = 0; i < trails.Length; i++)
            trails[i].SetTrailLent(L, mag * EngineQ);
    }

    public void FixedUpdate()
    {
        Shield = Mathf.MoveTowards(Shield, MaxShield, ShieldRegeneration * Time.deltaTime);
        if (!HasShield)
            HasShield = Shield > MaxShield / 2f;
        ShieldRender.enabled = HasShield && (Shield != 1);
        ShieldCollider.enabled = HasShield;
    }

    public override void OnDamaged(float dmg)
    {
        if (HasShield)
        {
            Shield -= dmg;
            if (Shield <= 0)
            {
                Shield = 0;
                HasShield = false;
            }
        }
        else
        {
            Health -= dmg;
            if (Health <= 0)
                Destroy(gameObject);
        }
    }

    public void Forward()
    {
        EnginePower = Mathf.Clamp01(EnginePower);
        RB.velocity += speed * EnginePower * Time.deltaTime * transform.forward;
        EngineQ = EnginePower;
    }

    public T GetWeapon<T>() where T : ShipWeapon
    {
        foreach (var q in weapons)
            if (q is T t)
                return t;
        return null;
    }

    public void Brake()
    {
        BrakePower = Mathf.Clamp01(BrakePower);
        RB.velocity = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * BrakePower * Time.deltaTime);
    }

    public void Fire()
    {
        if (weapons == null)
            weapons = GetComponentsInChildren<ShipWeapon>();
        foreach (var q in weapons)
            q.Fire();
    }

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
        Shield = 1;
        HasShield = true;
        EyeA = CreateEye("eye a");
        EyeB = CreateEye("eye b");
        Health = MaxHealth;
     //   brain = GetComponent<ShipController>();
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

    public void LookAt(Vector3 worldPoint)
    {
        transform.RotateTowards(worldPoint, rotationSpeed * Time.deltaTime);
    }

    public override void WhenDestroy()
    {
        //TODO
    }
}