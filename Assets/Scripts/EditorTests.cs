using UnityEngine;

[ExecuteAlways]
public class EditorTests : MonoBehaviour
{
    public EditorTests Target;

    public Vector3 velocity;

    public float ASpeed;

    public byte Q;

    public void OnDrawGizmos()
    {
        var pos = transform.position;
        Gizmos.color = Color.white;
        Gizmos.DrawLine(pos, pos + velocity);

        if (Target)
        {
            for (byte i = 1; i < Q; i++)
            {
                var q = Utils.ShootTo(pos, Vector3.zero, Target.transform.position, Target.velocity, ASpeed, i);
                Gizmos.color = Color.HSVToRGB((float) i / Q, 1, 1);
                Gizmos.DrawLine(pos, q);
            }
        }
    }
}