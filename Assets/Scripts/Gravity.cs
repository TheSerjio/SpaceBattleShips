using UnityEngine;

[DisallowMultipleComponent]
public class Gravity : MonoBehaviour
{
    private static Rigidbody[] all;

    public float Mass;

    public void Start()
    {
        all = new Rigidbody[0];
    }

    public static void UpdateObjects()
    {
        var shipz = FindObjectsOfType<Ship>();
        all = new Rigidbody[shipz.Length];
        for (int i = 0; i < shipz.Length; i++)
            all[i] = shipz[i].RB;
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
                sholdStart = true;

        if (sholdStart)
            UpdateObjects();
    }

    public void OnCollisionEnter(Collision collision)
    {
        UpdateObjects();
    }
}