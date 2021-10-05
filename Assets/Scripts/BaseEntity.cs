using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public abstract class BaseEntity : MonoBehaviour
{
    public enum Team : byte
    { Good, Bad }
    public Team team;
    public Rigidbody RB { get; private set; }

    public void Awake()
    {
        RB = GetComponent<Rigidbody>();
        foreach (var q in FindObjectsOfType<COLLECTOR>())
            q.Add(this);
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public abstract void OnDamaged(float dmg, BaseEntity from);
}