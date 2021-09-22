using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Ship : MonoBehaviour
{
    public float rotationSpeed;
    public float speed;
    public abstract bool IsPlayerControlled { get; }
    public enum Team { Good, Bad }
    public Team team;
    public Rigidbody RB
    {
        get
        {
            if (rb == null)
                rb = GetComponent<Rigidbody>();
            return rb;
        }
    }
    private Rigidbody rb;
}