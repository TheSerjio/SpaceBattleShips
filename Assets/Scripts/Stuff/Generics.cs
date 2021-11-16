using UnityEngine;

[DisallowMultipleComponent]
public abstract class SINGLETON<T> : MonoBehaviour where T : SINGLETON<T>
{
    public static T Self {get
        {
            if (!RealSelf)
                RealSelf = FindObjectOfType<T>();
            return RealSelf;
        }
    }

    private static T RealSelf { get; set; }

    public void Awake()
    {
        if (Self)
            if (Self != this)
                Debug.LogError($"Duplicated singleton {typeof(T).Name}! [{name}] and [{Self.name}]");
        RealSelf = (T)this;
        OnAwake();
    }

    protected virtual void OnAwake() { }
}

public abstract class COLLECTOR : MonoBehaviour
{
    public abstract void Add(BaseEntity it);

    public void Awake()
    {
        GameCore.Self.Add(this);
    }
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

public abstract class ValidableScriptableObject : ScriptableObject
{
    public enum Level
    {
        Message, Warning, Error
    }

    public struct Warning
    {
        public Level level;
        public string text;
        public ValidableScriptableObject parent;

        public Warning(Level lvl, string txt)
        {
            level = lvl;
            text = txt;
            parent = null;
        }
    }

    public abstract System.Collections.Generic.IEnumerable<Warning> Validate();
}

public abstract class PoolableComponent : MonoBehaviour
{
    public abstract void ReInit();
}