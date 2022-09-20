using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public abstract class BaseEntity : Script
{
    /// <summary>
    /// from mothership
    /// </summary>
    internal float ImmuneUntil;
    public Team team;
    public Rigidbody RB { get; private set; }
    public float size;

    protected sealed override void OnAwake()
    {
        RB = GetComponent<Rigidbody>();
        GameCore.Add(this);
        OnEntityAwake();
    }

    protected abstract void OnEntityAwake();

    public abstract void OnDamaged(float dmg, BaseEntity from);
}