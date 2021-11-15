using UnityEngine;

public class Ship : BaseEntity,IFireControl
{
    public struct Warning
    {
        public bool showText;
        public float shakePower;

        public Warning(bool text, float power)
        {
            showText = text;
            shakePower = power;
        }
    }

    public enum Type
    {
        Projectile,
        Laser
    }

    bool IFireControl.Fire { get => Fire; }
    Ship IFireControl.Parent { get => this; }

    private ShipController __brain__;
    private ShipController Brain
    {
        get
        {
            if (!__brain__)
                if(this)
                    foreach (var q in GetComponents<ShipController>())
                        if (q.isActiveAndEnabled)
                            __brain__ = q;
            return __brain__;
        }
    }

    public bool UseCheats
    {
        get
        {
            if (Brain)
                return Brain is PlayerShip;
            else
                return Random.value < 0.5f;
        }
    }

    public bool TakeEnergy(float value)
    {
        if (Energy > value)
        {
            Energy -= value;
            return true;
        }
        else
            return false;
    }

    internal float ImmuneUntil;
    [Tooltip("Degrees per second")] [SerializeField] float rotationSpeed;
    public float RotationSpeed => rotationSpeed;
    public float RelativeHealth => Health / MaxHealth;
    public float RelativeEnergy => Energy / MaxEnergy;
    [SerializeField] float speed;
    [SerializeField] float MaxHealth;
    public float Health { get; private set; }
    [SerializeField] ShipTrail[] trails;
    [SerializeField] float EngineConsumption;
    [SerializeField] float BrakeConsumption;
    public float EnginePower { get; set; }
    public float BrakePower { get; set; }
    /// <summary>
    /// Legacy
    /// </summary>
    public float ShieldPower { get; set; }
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
    public bool Fire { get; set; }
    public float size;
    public float ExplosionPower;
    public float ExplosionSize;
    private bool _exploded = false;

#if UNITY_EDITOR
    [ContextMenu("Magic")]
    public void Magic()
    {
        trails = GetComponentsInChildren<ShipTrail>();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    public void Warn(Vector3 moveTo, Warning how)
    {
        if (Brain)
            Brain.Warn(moveTo, how);
    }

    public void Update()
    {
        Energy = Mathf.MoveTowards(Energy, MaxEnergy, EnergyRegeneration * Time.deltaTime);
        EngineQ = Mathf.MoveTowards(EngineQ, 0, EngineQ * Time.deltaTime);
        for (int i = 0; i < trails.Length; i++)
            trails[i].SetTrailLent(EngineQ);
    }

    public sealed override void OnDamaged(float dmg, BaseEntity from)
    {
        if (_exploded)
            return;
        void Do(Vector3 world)
        {
            var boom = Instantiate(DataBase.Get().ShipExplosion, world, Random.rotation);
            boom.transform.localScale = Vector3.one * ExplosionSize;
            Destroy(boom, 10);
            GameCore.Self.Explode(transform.position, ExplosionPower, team);
        }

        if (!Shield)
            Shield = GetComponent<Shield>();
        if (Shield)
            Shield.TakeDamage(ref dmg);


        if (Time.time < ImmuneUntil)
            return;

        Health -= dmg;
        MeshForDamage.material.SetFloat("Damage", 1 - (Health / MaxHealth));
        if (Health <= 0)
        {
            _exploded = true;
            if (Brain)
                Brain.Death();
            foreach (var q in GetComponentsInChildren<ShipAdditionalExplosion>())
                Do(q.transform.position);
            Do(transform.position);
            Destroy(gameObject);
        }
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
            EngineQ = EnginePower;
        }
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

    /*[System.Obsolete]
    Transform CreateEye(string q)
    {
        var obj = new GameObject(q).transform;
        obj.parent = transform;
        obj.localPosition = Vector3.zero;
        obj.localEulerAngles = Vector3.zero;
        obj.localScale = Vector3.one;
        return obj;
    }*/

    protected sealed override void OnAwake()
    {
        EnginePower = 1;
        BrakePower = 1;
        ShieldPower = 1;
        Energy = MaxEnergy;
        Health = MaxHealth;
        ImmuneUntil = Time.time + Spawner.time;
        Shield = GetComponent<Shield>();
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

    /// <returns>from -1 to 1</returns>
    public float LookAt(Vector3 worldPoint)
    {
        transform.RotateTowards(worldPoint, rotationSpeed * Time.deltaTime);
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

    public void OnTriggerEnter(Collider other)
    {
        var p = other.gameObject.GetComponent<Projectile>();
        if (p)
            if (p.Team != team)
            {
                OnDamaged(p.Damage, p.Parent);
                p.Die();
            }
    }

    public void ExtraForward()
    {
        float prev = EnginePower;
        EnginePower = 5;//5
        Forward();
        EnginePower = prev;
    }

    public override void DeathDamage()
    {
        OnDamaged(MaxHealth * Time.deltaTime, null);
    }
}