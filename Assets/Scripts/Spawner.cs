using UnityEngine;

[DisallowMultipleComponent]
public sealed class Spawner : MonoBehaviour
{
    public Team team;
    public MotherShip.Data[] all;

    public const float time = 5;

    public void Start()
    {
        StartCoroutine(Summon());
    }

    public static void CreateFrame(Ship ship, ShipData asset)
    {
        var f = Instantiate(DataBase.Get().TargetFramePrefab);
        f.transform.SetParent(GameUI.Self.WorlsCanvas.transform);
        f.transform.localScale = Vector3.one;
        var ff = f.GetComponent<TargetFrame>();
        ff.target = ship;
        ff.Name = $"{asset.Name}";//-{(ushort)Random.Range(0, ushort.MaxValue)}";
        ship.frame = ff;
    }

    private System.Collections.IEnumerator Summon()
    {
        ulong i = 0;
        foreach (var g in all)
            for (ushort ii = 0; ii < g.count; ii++)
            {
                var q = Instantiate(g.ship.Prefab);
                q.transform.position = transform.position + (4 * i++ * Random.onUnitSphere);
                var ship = q.GetComponent<Ship>();
                ship.team = team;
                CreateFrame(ship, g.ship);
                yield return null;
            }
        Destroy(gameObject);
    }
}