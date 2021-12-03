using UnityEngine;

public abstract class ShipWeapon : Script
{
    protected sealed override void OnAwake()
    {

    }

    public abstract float S_Bullets { get; }

    public abstract float AntiSpeed { get; }

    public abstract bool IsOutOfRange(float distance);

    public abstract float S_MaxDPS();

    public  abstract float S_EnergyConsumption { get; }

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

    public SoundClip onShot;

    public float PlayerExplosionSize;

    public void Update()
    {
        if (CoolDown > 0)
            CoolDown -= Time.deltaTime;
        else if (Parent.Target.Fire)
        {
            if (Parent.TakeEnergy(EnergyPerShot))
            {
                CoolDown += ReloadTime;
                if (Parent.PlayerMarked)
                {
                        AudioManager.PlaySound(onShot, false);
                        if (PlayerExplosionSize != 0)
                        {
                            var obj = Instantiate(DataBase.Get().PlayerShotSmallExplosion, transform);
                            obj.transform.localPosition = Vector3.zero;
                            obj.transform.localScale = Vector3.one * PlayerExplosionSize;
                            Destroy(obj, 1);
                        }
                }

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

    public sealed override float S_MaxDPS() => Damage / ReloadTime;
}