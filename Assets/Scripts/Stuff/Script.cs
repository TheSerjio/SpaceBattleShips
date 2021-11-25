using UnityEngine;

public abstract class Script : MonoBehaviour
{
    public new Transform transform { get; private set; }

    public Transform Ttransform => base.transform;
    public void Awake()
    {
        transform = GetComponent<Transform>();
        OnAwake();
    }

    protected abstract void OnAwake();
}