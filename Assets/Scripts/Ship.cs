using UnityEngine;

public sealed class Ship : BaseEntity
{
   // private ShipController brain;
    [Tooltip("Degrees per second")] [SerializeField] float rotationSpeed;
    public float RotationSpeed => rotationSpeed;
    public float RelativeHealth => Health / MaxHealth;
    public float RelativeEnergy => Energy / MaxEnergy;
    [SerializeField] float speed;
    public Transform EyeA { get;private set; }
    public Transform EyeB { get;private set; }
    [SerializeField] float MaxHealth;
    public float Health { get; private set; }
    [SerializeField] ShipWeapon[] weapons;
    [SerializeField] ShipTrail[] trails;
    [SerializeField] float EngineConsumption;
    [SerializeField] float BrakeConsumption;
    public float EnginePower { get;  set; }
    public float BrakePower { get;  set; }
    /// <summary>
    /// Legacy
    /// </summary>
    public float ShieldPower { get;  set; }
    /// <summary>
    /// Serializable
    /// </summary>
    [SerializeField] float MaxEnergy;
    public float Energy { get; private set; }
    [Tooltip("Per second")] [SerializeField] float EnergyRegeneration;
    private float EngineQ;
    [SerializeField] MeshRenderer MeshForDamage;
    public TargetFrame frame;
    public Shield Shield { get; private set; }

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
        EngineQ = Mathf.MoveTowards(EngineQ, 0, Time.deltaTime * 2);
        float q = 2.125f;
        float L = Vector3.Dot(transform.forward, RB.velocity.normalized) / q + (1 - 1 / q);
        float mag = RB.velocity.magnitude;
        for (int i = 0; i < trails.Length; i++)
            trails[i].SetTrailLent(L, mag * EngineQ);
    }

    public void FixedUpdate()
    {
        Energy = Mathf.MoveTowards(Energy, MaxEnergy, EnergyRegeneration * Time.deltaTime);
    }

    public override void OnDamaged(float dmg, BaseEntity from)
    {
        if (!Shield)
            Shield = GetComponent<Shield>();
        if (Shield)
            Shield.TakeDamage(ref dmg);
        Health -= dmg;
        MeshForDamage.material.SetFloat("Damage", 1 - (Health / MaxHealth));
        if (Health <= 0)
            Destroy(gameObject);
        if (frame)
            frame.OnHit(from);
    }

    public void Forward()
    {
#if DEBUG
        if (EnginePower < 0)
        {
            Debug.Log($"{name} has negative engine power - {EnginePower}");
            return;
        }
#endif
        float e = EngineConsumption * Utils.EnergyConsumption(EnginePower) * Time.deltaTime;
        if (Energy >= e)
        {
            RB.velocity += speed * EnginePower * Time.deltaTime * transform.forward;
            Energy -= e;
            EngineQ = 1;
        }
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
#if DEBUG
        if (BrakePower < 0)
        {
            Debug.Log($"{name} has negative brake power - {EnginePower}");
            return;
        }
#endif
        Vector3 next = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * BrakePower * Time.deltaTime);
        if (next == Vector3.zero)
        {
            RB.velocity = next;
        }
        else
        {
            float e = BrakeConsumption * Utils.EnergyConsumption(BrakePower) * Time.deltaTime;
            if (Energy >= e)
            {
                RB.velocity = next;
                Energy -= e;
            }
        }
    }

    public void AutoBrake()
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
        Energy = MaxEnergy;
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
}