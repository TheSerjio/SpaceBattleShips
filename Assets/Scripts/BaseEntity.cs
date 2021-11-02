using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public abstract class BaseEntity : MonoBehaviour
{
    public enum Team : byte
    { Defenders, Attackers, Derelict, Pie, Pirates }
    public Team team;
    public Rigidbody RB { get; private set; }

    public void Awake()
    {
        RB = GetComponent<Rigidbody>();
        GameCore.Add(this);
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public abstract void OnDamaged(float dmg, BaseEntity from);

    public abstract void DeathDamage();
}