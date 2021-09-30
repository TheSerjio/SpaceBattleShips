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

    public void TakeDamage(ref float dmg)
    {
        if (Current >= dmg)
        {
            Current -= dmg;
            dmg = 0;
        }
        else
        {
            dmg -= Current;

        }
    }

    public void FixedUpdate()
    {
        Current = Mathf.MoveTowards(Current, MaxShield, ShieldRegeneration * Time.deltaTime);
        if (!HasShield)
            HasShield = Current > MaxShield / 2f;
        ShieldRender.enabled = HasShield && (Current != 1);
        ShieldCollider.enabled = HasShield;
    }
}