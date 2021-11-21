using UnityEngine;

public abstract class ShipWeapon : MonoBehaviour
{
    public IFireControl Parent
    {
        get
        {
            if (ship == null)
                ship = GetComponentInParent<Ship>();
            return ship;
        }
        set
        {
            ship = value;
        }
    }

    private IFireControl ship;

    public abstract float AntiSpeed { get; }

    public abstract bool IsOutOfRange(float distance);

    public abstract float MaxDPS();
}

public abstract class ShipWeaponWithCoolDown : ShipWeapon
{
    public void Start()
    {
        CoolDown = ReloadTime * (Random.value + 1);
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
        else if (Parent.Fire)
        {
            if (Parent.Parent.TakeEnergy(EnergyPerShot))
            {
                CoolDown += ReloadTime;
                Shoot();
            }
        }
        else
            CoolDown = 0;
        OnUpdate();
    }

    public abstract void Shoot();

    public virtual void OnUpdate() { }
    public virtual void OnStart() { }

    public sealed override float MaxDPS() => Damage / ReloadTime;
}