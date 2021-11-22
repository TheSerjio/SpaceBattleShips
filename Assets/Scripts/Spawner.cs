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
        ff.frameImage.color = DataBase.Get().TeamColor(ship.team);
        ff.target = ship;
        ff.Name = $"{asset.Name}";//-{(ushort)Random.Range(0, ushort.MaxValue)}";
        ship.frame = ff;
        ff.Update();
    }

    private System.Collections.IEnumerator Summon()
    {
        float D = 0;
        foreach (var g in all)
            for (ushort ii = 0; ii < g.count; ii++)
            {
                var q = Instantiate(g.ship.Prefab);
                q.transform.position = transform.position + (D * Random.onUnitSphere);
                q.transform.rotation = Random.rotationUniform;
                var ship = q.GetComponent<Ship>();
                D += ship.size;
                ship.team = team;
                CreateFrame(ship, g.ship);
                yield return null;
            }
        Destroy(gameObject);
    }
}