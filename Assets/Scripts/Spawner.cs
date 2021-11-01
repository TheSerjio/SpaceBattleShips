using UnityEngine;

[DisallowMultipleComponent]
public sealed class Spawner : MonoBehaviour
{
    public BaseEntity.Team team;
    public Group[] all;
    public Canvas canva;
    [System.Serializable]
    public struct Group
    {
        public ShipData prefab;
        public byte number;
    }

    public void Start()
    {
        StartCoroutine(Summon());
    }

    private System.Collections.IEnumerator Summon()
    {
        ulong i = 0;
        foreach (var g in all)
            for (ushort ii = 0; ii < g.number; ii++)
            {
                var q = Instantiate(g.prefab.prefab);
                q.transform.position = transform.position + (Random.onUnitSphere * i++);
                var ship = q.GetComponent<Ship>();
                ship.team = team;
                var f = Instantiate(DataBase.Get().TargetFramePrefab);
                f.transform.SetParent(canva.transform);
                f.transform.localScale = Vector3.one;
                var ff = f.GetComponent<TargetFrame>();
                ff.target = ship;
                ff.number = ii;
                ship.frame = ff;
                yield return null;
            }
        Destroy(gameObject);
    }
}