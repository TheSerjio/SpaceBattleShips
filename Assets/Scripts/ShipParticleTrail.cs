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
        ve.SetVector2("speed", DataBase.Get().EngineSizeFromPower.Evaluate(speed / 5f) * 20 * Vector2.up);
    }
}