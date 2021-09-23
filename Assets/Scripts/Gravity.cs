using UnityEngine;

[DisallowMultipleComponent]
public class Gravity : MonoBehaviour
{
    private static Rigidbody[] all;

    public float Mass;


    public void Start()
    {
        all = FindObjectsOfType<Rigidbody>();
    }

    public void FixedUpdate()
    {
        bool sholdStart = false;
        var pos = transform.position;
        foreach (var rb in all)
            if (rb)
            {
                var dir = pos - rb.position;
                rb.velocity += Time.deltaTime * Mass * dir.normalized / dir.sqrMagnitude;
            }
            else
            {
                sholdStart = true;
                break;
            }

        if (sholdStart)
            all = FindObjectsOfType<Rigidbody>();
    }
}