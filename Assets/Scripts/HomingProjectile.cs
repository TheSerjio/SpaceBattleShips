using UnityEngine;

public sealed class HomingProjectile : Projectile
{
    [Range(1f, 10f)] public float acceleration;

    [Range(45f, 360f)] public float rotationSpeed;

    public void FixedUpdate()
    {
        if (Target)
            transform.RotateTowards(Target.position + Target.velocity * Vector3.Distance(Target.position, transform.position) / acceleration, rotationSpeed);
        RB.velocity += acceleration * Time.deltaTime * transform.forward;
    }
}