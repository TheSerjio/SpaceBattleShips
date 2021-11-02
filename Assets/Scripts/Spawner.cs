using UnityEngine;

[DisallowMultipleComponent]
public sealed class Spawner : MonoBehaviour
{
    public BaseEntity.Team team;
    public MotherShip.Data[] all;

    public void Start()
    {
        StartCoroutine(Summon());
    }

    public static void CreateFrame(Ship ship)
    {
        var f = Instantiate(DataBase.Get().TargetFramePrefab);
        f.transform.SetParent(GameCore.Self.canvas.transform);
        f.transform.localScale = Vector3.one;
        var ff = f.GetComponent<TargetFrame>();
        ff.target = ship;
        ff.number = (ushort)Random.Range(0, ushort.MaxValue);
        ship.frame = ff;
    }

    private System.Collections.IEnumerator Summon()
    {
        ulong i = 0;
        foreach (var g in all)
            for (ushort ii = 0; ii < g.count; ii++)
            {
                var q = Instantiate(g.ship.prefab);
                q.transform.position = transform.position + (4 * i++ * Random.onUnitSphere);
                var ship = q.GetComponent<Ship>();
                ship.team = team;
                CreateFrame(ship);
                yield return null;
            }
        Destroy(gameObject);
    }
}