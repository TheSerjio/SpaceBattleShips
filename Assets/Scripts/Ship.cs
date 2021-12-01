using UnityEngine;

public class Ship : BaseEntity
{
    public const float MinEngine = 0.5f;
    public const float MaxEngine = 5;
    
    public readonly struct Warning
    {
        public readonly bool showText;
        public readonly float shakePower;

        public Warning(bool text, float power)
        {
            showText = text;
            shakePower = power;
        }
    }

    private ShipController __brain__;

    public ShipController Brain
    {
        get
        {
            if (!__brain__)
            {
                if (this)
                    foreach (var q in GetComponents<ShipController>())
                        if (q.isActiveAndEnabled)
                            __brain__ = q;
            }
            return __brain__;
        }
        set => __brain__ = value;
    }

    public bool UseCheats
    {
        get
        {
            if (Brain)
                return Brain is PlayerShip;
            else
                return false;
        }
    }

    public bool TakeEnergy(float value)
    {
        if (EnergyCD <= 0)
        {
            if (Energy > value)
            {
                Energy -= value;
                return true;
            }
            else
            {
                EnergyCD = NoEnergyCooldown;
                return false;
            }
        }
        else
            return false;
    }

    public Targeting Target = new Targeting();

    internal float ImmuneUntil;
    [Tooltip("Degrees per second")] [SerializeField]
    private float rotationSpeed;
    public float RotationSpeed => rotationSpeed;
    public float RelativeHealth => Health / MaxHealth;
    public float RelativeEnergy => Energy / MaxEnergy;
    public float speed;
  public float MaxHealth;
    private float Health { get; set; }
    [SerializeField] private ShipTrail[] trails;
    [SerializeField] private float EngineConsumption;
    public float EngineCons => EngineConsumption;
    [SerializeField] private float BrakeConsumption;
    [SerializeField] private float BrakePower;

    /// <summary>
    /// Mutable
    /// </summary>
    public float EnginePower
    {
        get => _enignePower;
        set => _enignePower = Mathf.Clamp(value, MinEngine, MaxEngine);
    }

    private float _enignePower;
    /// <summary>
    /// Serializable
    /// </summary>
    public float MaxEnergy;

    private float Energy { get; set; }

    [Tooltip("Per second")] public float EnergyRegeneration;
    private float EngineQ;
    [SerializeField] private MeshRenderer MeshForDamage;
    public TargetFrame frame;
    public Shield Shield { get; private set; }
    public float ExplosionPower;
    public float ExplosionSize;
    private bool _exploded;
    [SerializeField] private float NoEnergyCooldown;
    private float EnergyCD;
    public ShipWeapon mainWeapon;
    public bool PlayerMarked { get; set; }
    public Transform[] Formation;
    public Ship[] Teammates { get; private set; }

    public ShipData asset;

    private float RequredEngineQ;

    [Range(3f, 150f)] public float SniperCameraAngle;

    [ContextMenu("Magic")]
    public void FindTrails()
    {
        trails = GetComponentsInChildren<ShipTrail>();
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorUtility.SetDirty(this);
#endif
    }


    public void Warn(Vector3 moveTo, Warning how)
    {
        if (Brain)
            Brain.Warn(moveTo, how);
    }

    public void Update()
    {
        Energy = Mathf.MoveTowards(Energy, MaxEnergy, EnergyRegeneration * Time.deltaTime);
        EngineQ = Mathf.MoveTowards(EngineQ, RequredEngineQ, Time.deltaTime * 20);
        RequredEngineQ = Mathf.MoveTowards(RequredEngineQ, 0, Time.deltaTime * RequredEngineQ);
        foreach (var t in trails)
            t.SetTrailLent(EngineQ);
        if (EnergyCD > 0)
            EnergyCD -= Time.deltaTime;
    }

    public sealed override void OnDamaged(float dmg, BaseEntity from)
    {
        if (_exploded)
            return;
        if (Time.time < ImmuneUntil)
            return;

        void Do(Vector3 world)
        {
            GameCore.Self.Explode(world, ExplosionPower, team);
            GameCore.Self.MakeBoom(world, Poolable.ShipExplosion, ExplosionSize);
        }

        if (!Shield)
            Shield = GetComponent<Shield>();
        if (Shield)
            Shield.TakeDamage(ref dmg);

        Health -= dmg;
        MeshForDamage.material.SetFloat(Utils.ShaderID(ShaderName.Damage), 1 - (Health / MaxHealth));
        if (Health <= 0)
        {
            if (TryGetComponent<PlayerMark>(out var mark))
                mark.IfDie();
            _exploded = true;
            foreach (var q in GetComponentsInChildren<ShipAdditionalExplosion>())
                Do(q.transform.position);
            Do(transform.position);
            Destroy(gameObject);
        }

        if (frame)
            if (from is Ship {PlayerMarked: true})
                frame.OnHit();
    }

    public void Forward()
    {
#if UNITY_EDITOR
        if (EnginePower < 0)
        {
            Debug.Log($"{name} has negative engine power - {EnginePower}");
            return;
        }
#endif
        float e = EngineConsumption * Utils.EnergyConsumption(EnginePower) * Time.deltaTime;
        if (TakeEnergy(e))
        {
            RB.velocity += speed * EnginePower * Time.deltaTime * transform.forward;
            RequredEngineQ = EnginePower;
        }
    }

    public void Brake(bool backWard)
    {
        var target = backWard ? -transform.forward : Vector3.zero;
        var next = Vector3.MoveTowards(RB.velocity, target, speed * BrakePower * Time.deltaTime);
        if (next == target)
        {
            RB.velocity = next;
        }
        else
        {
            var e = BrakeConsumption * Time.deltaTime;
            if (TakeEnergy(e))
            {
                RB.velocity = next;
            }
        }
    }

    public void AutoBrake()
    {
        RB.velocity = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * Time.deltaTime);
    }

    protected sealed override void OnEntityAwake()
    {
        EnginePower = 1;
        Energy = MaxEnergy;
        Health = MaxHealth;
        ImmuneUntil = Time.time + Utils.StartTime;
        Shield = GetComponent<Shield>();
        MeshForDamage.material.SetFloat(Utils.ShaderID(ShaderName.E_Skin), Random.value);
        Teammates = new Ship[Formation.Length];
    }

    public void OnDrawGizmos()
    {
        if (RB)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(RB.position, RB.position + RB.velocity);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, Random.value, Random.value);
        Gizmos.DrawWireSphere(Ttransform.position, size / 2f);
        Gizmos.color = Color.white;
        if (Formation != null)
            foreach (var q in Formation)
                Gizmos.DrawSphere(q.position, 1);
        DrawGizmosSelected();
    }

    protected virtual void DrawGizmosSelected() { }

    /// <returns>from -1 to 1</returns>
    public float LookAt(Vector3 worldPoint)
    {
        transform.RotateTowards(worldPoint, rotationSpeed * Time.deltaTime, false);
        return Vector3.Dot(transform.forward, (worldPoint - transform.position).normalized);
    }

    public void OnCollisionEnter(Collision collision)
    {
        var q = collision.collider.gameObject.GetComponentInParent<Ship>();
        if (q)
            if (Time.time > ImmuneUntil)
                if (Time.time > q.ImmuneUntil)
                {
                    float dmg = collision.relativeVelocity.magnitude / 5f;
                    q.OnDamaged(dmg, this);
                    OnDamaged(dmg, q);
                }
    }

    public void ExtraForward()
    {
        var prev = EnginePower;
        EnginePower = MaxEngine;
        Forward();
        EnginePower = prev;
    }

    public override void DeathDamage()
    {
        OnDamaged(MaxHealth * Time.deltaTime, null);
    }
}