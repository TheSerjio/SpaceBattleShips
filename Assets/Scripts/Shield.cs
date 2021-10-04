using UnityEngine;

[RequireComponent(typeof(Ship))]
[DisallowMultipleComponent]
public sealed class Shield : MonoBehaviour
{

    [SerializeField] Renderer ShieldRender;
    [SerializeField] Collider ShieldCollider;
    [SerializeField] float MaxShield;
    public float Current { get; private set; }
    [SerializeField] float ShieldRegeneration;
    public bool HasShield { get; private set; }
    public float Relative => Current / MaxShield;
    private float Alpha;

    private const float defaultA = 0.25f;
    private const float HighA = 0.5f;

    public void TakeDamage(ref float dmg)
    {
        if (HasShield)
        {
            if (Current >= dmg)
            {
                Current -= dmg;
                dmg = 0;
            }
            else
            {
                dmg -= Current;
                HasShield = false;
            }
            Alpha = HighA;
        }
    }

    public void FixedUpdate()
    {
        Current = Mathf.MoveTowards(Current, MaxShield, ShieldRegeneration * Time.deltaTime);
        if (!HasShield)
            HasShield = Current > MaxShield / 2f;
        ShieldRender.enabled = HasShield && (Current != MaxShield);
        ShieldCollider.enabled = HasShield;
        Alpha = Mathf.MoveTowards(Alpha, defaultA, Time.deltaTime);
        ShieldRender.material.SetFloat("Alpha", Alpha);
    }

    public void Start()
    {
        HasShield = true;
        //Current = MaxShield;
    }
}