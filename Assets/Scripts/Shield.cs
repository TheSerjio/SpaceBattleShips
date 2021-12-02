using UnityEngine;

[DisallowMultipleComponent]
public sealed class Shield : MonoBehaviour
{

    [SerializeField] private Renderer ShieldRender;
    [SerializeField] private Collider ShieldCollider;
    public float MaxShield;
    private float Current { get; set; }
    public float ShieldRegeneration;
    public bool HasShield { get; private set; }
    public float Relative => Current / MaxShield;
    private float Alpha;
    private const float defaultA = 0.25f;
    private Ship ship;

    public void TakeDamage(ref float dmg)
    {
        if (HasShield)
        {
            var q = dmg / (dmg + 1);
            if (Current >= dmg)
            {
                Current -= dmg;
                dmg = 0;
                if (ship.PlayerMarked)
                    AudioManager.PlaySound(DataBase.Get().OnShieldHit);
            }
            else
            {
                dmg -= Current;
                Current = 0;
                HasShield = false;
                if (ship.PlayerMarked)
                    AudioManager.PlaySound(DataBase.Get().OnZeroShields);
            }

            Alpha = (Alpha + q) / (q + 1);
        }
        else if (ship.PlayerMarked)
            AudioManager.PlaySound(DataBase.Get().OnTakeDamage);
    }

    public void Update()
    {
        Current = Mathf.MoveTowards(Current, MaxShield, ShieldRegeneration * Time.deltaTime);
        if (!HasShield)
            HasShield = Current > MaxShield / 2f;
        ShieldRender.enabled = HasShield && (Alpha != 0);
        ShieldCollider.enabled = HasShield;
        Alpha = Mathf.MoveTowards(Alpha, (Current >= MaxShield) ? 0 : defaultA, Time.deltaTime);
        ShieldRender.material.SetFloat(Utils.ShaderID(ShaderName.Alpha), Alpha);
    }

    public void Start()
    {
        HasShield = true;
        Current = MaxShield;
        ship = GetComponent<Ship>();
    }
}