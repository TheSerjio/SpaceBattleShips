using UnityEngine;

[DisallowMultipleComponent]
public class SpawnPlayerHere : SINGLETON<SpawnPlayerHere>
{
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Ttransform.position, Ttransform.position + (Ttransform.forward * 100));
    }
}