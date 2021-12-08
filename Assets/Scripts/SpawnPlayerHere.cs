using UnityEngine;

[DisallowMultipleComponent]
public class SpawnPlayerHere : SINGLETON<SpawnPlayerHere>
{
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Ttransform.position, Ttransform.forward);
    }
}