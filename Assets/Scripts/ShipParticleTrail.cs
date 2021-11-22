using UnityEngine;
using UnityEngine.VFX;

public class ShipParticleTrail : ShipTrail
{
    private VisualEffect ve;

    public void Awake()
    {
        ve = GetComponentInChildren<VisualEffect>();
    }

    public override void SetTrailLent(float speed)
    {
        ve.SetVector2("speed", 4 * speed * Vector2.up);
    }
}