using UnityEngine;

public abstract class SINGLETON<T> : MonoBehaviour where T : SINGLETON<T>
{
    public static T Self { get; private set; }

    public void Awake()
    {
        Self = (T)this;
        OnAwake();
    }

    protected virtual void OnAwake() { }
}