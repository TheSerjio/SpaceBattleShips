using UnityEngine;

public class Ship : BaseEntity
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

    private ShipController __brain__;
    private ShipController Brain
    {
        get
        {
            if (!__brain__)
                foreach (var q in GetComponents<ShipController>())
                    if (q.isActiveAndEnabled)
                        __brain__ = q;
            return __brain__;
        }
    }
    [Tooltip("Degrees per second")] [SerializeField] float rotationSpeed;
    public float RotationSpeed => rotationSpeed;
    public float RelativeHealth => Health / MaxHealth;
    public float RelativeEnergy => Energy / MaxEnergy;
    [SerializeField] float speed;
    [SerializeField] float MaxHealth;
    public float Health { get; private set; }
    [SerializeField] ShipWeapon[] weapons;
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

#if UNITY_EDITOR
    [ContextMenu("Magic")]
    public void Magic()
    {
        weapons = GetComponentsInChildren<ShipWeapon>();
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
        EngineQ = Mathf.MoveTowards(EngineQ, 0, EngineQ * Time.deltaTime);
        for (int i = 0; i < trails.Length; i++)
            trails[i].SetTrailLent(EngineQ);
    }

    public virtual void UseExtraAbility() { }

    public void FixedUpdate()
    {
        Energy = Mathf.MoveTowards(Energy, MaxEnergy, EnergyRegeneration * Time.deltaTime);
    }

    public sealed override void OnDamaged(float dmg, BaseEntity from)
    {
        void Do(Vector3 world)
        {
            var boom = Instantiate(DataBase.Get().ShipExplosion, world, Quaternion.identity);
            boom.transform.localScale = Vector3.one * size;
            Destroy(boom, 30);
        }

        if (!Shield)
            Shield = GetComponent<Shield>();
        if (Shield)
            Shield.TakeDamage(ref dmg);
        Health -= dmg;
        MeshForDamage.material.SetFloat("Damage", 1 - (Health / MaxHealth));
        if (Health <= 0)
        {
            if (Brain)
                Brain.Death();
            foreach (var q in GetComponentsInChildren<ShipAdditionalExplosion>())
                Do(q.transform.position);
            Do(transform.position);
            GameCore.Self.Explode(transform.position, MaxHealth, this);
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

    public float LookAt(Vector3 worldPoint)
    {
        transform.RotateTowards(worldPoint, rotationSpeed * Time.deltaTime);
        return Vector3.Dot(transform.forward, (worldPoint - transform.position).normalized);
    }

    public void OnCollisionEnter(Collision collision)
    {
        var q = collision.collider.gameObject.GetComponentInParent<Ship>();
        if (q)
        {
            float dmg = collision.relativeVelocity.magnitude;
            q.OnDamaged(dmg, this);
            OnDamaged(dmg, q);
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