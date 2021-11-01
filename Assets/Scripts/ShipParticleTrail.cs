using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class ShipParticleTrail : ShipTrail
{
    public float MaxLentgh;
    public float VelocityAffect;

    private VisualEffect lr;

    public void Start()
    {
        lr = GetComponent<VisualEffect>();
    }

    public override void SetTrailLent(float dir, float speed)
    {
        // :)
        lr.SetVector2("speed", Vector2.up * MaxLentgh * speed / (speed + VelocityAffect));
    }
}