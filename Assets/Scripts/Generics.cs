using UnityEngine;

[DisallowMultipleComponent]
public abstract class SINGLETON<T> : MonoBehaviour where T : SINGLETON<T>
{
    public static T Self { get; private set; }

    public void Awake()
    {
        if (Self)
            Debug.LogError($"Duplicated singleton {typeof(T).Name}! [{name}] and [{Self.name}]");
        Self = (T)this;
        OnAwake();
    }

    protected virtual void OnAwake() { }
}

public abstract class COLLECTOR : MonoBehaviour
{
    public abstract void Add(BaseEntity it);
}

public abstract class COLLECTOR<T> : COLLECTOR where T : BaseEntity
{
    private System.Collections.Generic.List<T> all;

    private System.Collections.Generic.List<T> All
    {
        get
        {
            if (all == null)
                all = new System.Collections.Generic.List<T>();
            return all;
        }
    }

    public override sealed void Add(BaseEntity it)
    {
        if (it is T t)
            All.Add(t);
    }

    private void RemoveNull()
    {
        var real = new System.Collections.Generic.List<T>();
        foreach (var q in All)
            if (q)
                real.Add(q);
        all = real;
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < All.Count; i++)
        {
            var q = all[i];
            if (q)
            {
                ForEach(q);
            }
            else
            {
                RemoveNull();
                break;
            }
        }
    }

    /// <param name="with">never null</param>
    public abstract void ForEach(T with);
}

[System.Flags]
public enum DistanceWarn
{
    None = 0,
    CameraShake = 1,
    Text = 2,
    All = 3
}