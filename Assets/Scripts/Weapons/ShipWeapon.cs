using UnityEngine;

public abstract class ShipWeapon : MonoBehaviour
{
    public abstract float AntiSpeed { get; }

    public abstract bool IsOutOfRange(float distance);

    public abstract float MaxDPS();

    public abstract float MaxFireDist { get; }

    public Ship Parent
    {
        get
        {
            if (!_parent_)
                _parent_ = GetComponentInParent<Ship>();
            return _parent_;
        }
    }

    private Ship _parent_;
}

public abstract class ShipWeaponWithCoolDown : ShipWeapon
{
    public void Start()
    {
        CoolDown = ReloadTime;
        OnStart();
    }

    public float Damage;

    public float ReloadTime;

    private float CoolDown;

    public float EnergyPerShot;

    public void Update()
    {
        if (CoolDown > 0)
            CoolDown -= Time.deltaTime;
        else if (Parent.Target.Fire)
        {
            if (Parent.TakeEnergy(EnergyPerShot))
            {
                CoolDown += ReloadTime;
                Shoot();
            }
        }
        else
            CoolDown = 0;
        OnUpdate();
    }

    protected abstract void Shoot();

    protected virtual void OnUpdate() { }
    protected virtual void OnStart() { }

    public sealed override float MaxDPS() => Damage / ReloadTime;
}