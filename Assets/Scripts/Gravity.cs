using UnityEngine;

[DisallowMultipleComponent]
public class Gravity : COLLECTOR<Ship>
{
    public float Mass;

    protected override void ForEach(Ship with)
    {
        var dir = transform.position - with.transform.position;
        with.RB.velocity += Time.deltaTime * Mass * dir.normalized / dir.sqrMagnitude;
    }
}