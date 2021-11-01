using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class ShipParticleTrail : ShipTrail
{
    private VisualEffect lr;

    public void Start()
    {
        lr = GetComponent<VisualEffect>();
    }

    public override void SetTrailLent(float speed)
    {
        // :)
        lr.SetVector2("speed", Vector2.up * size * speed);
    }
}