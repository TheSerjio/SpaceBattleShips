using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public abstract class Ship : MonoBehaviour
{
    public float rotationSpeed;
    public float speed;
    public abstract bool IsPlayerControlled { get; }
    public enum Team { Good, Bad }
    public Team team;
    public Rigidbody RB { get; private set; }
   /* public TrailRenderer[] trails;
    public float lentghOfTrails;

    [ContextMenu("Add trails")]
    public void AddTrails()
    {
        trails = GetComponentsInChildren<TrailRenderer>();
    }*/

    public void Update()
    {
       /* for(int i=0;i<trails.Length;i++)
        {
            var it = trails[i];
            if (it)
            {
                it.time = RB.velocity.magnitude * lentghOfTrails;
            }
            else
            {
                var list = new System.Collections.Generic.List<TrailRenderer>();
                foreach (var q in trails)
                    if (q)
                        list.Add(q);
                trails = list.ToArray();
                break;
            }
        }*/
        OnUpdate();
    }

    public abstract void OnUpdate();

    public void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        OnStart();
    }

    public abstract void OnStart();

#if DEBUG
    public void OnDrawGizmos()
    {
        if (RB)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(RB.position, RB.position + RB.velocity);
        }
    }
#endif

}