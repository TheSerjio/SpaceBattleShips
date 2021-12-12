using UnityEngine;

public sealed class Spawner : MonoBehaviour
{
    public Team team;
    public MotherShip.Data[] all;
    public float waitSeconds;


    public float TotalCost;

    public void OnValidate()
    {
        TotalCost = Utils.GetCost(all);
    }

    public void Start()
    {
        StartCoroutine(Summon());
    }

    public static void CreateFrame(Ship ship)
    {
        var asset = ship.asset;
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
        yield return new WaitForSeconds(waitSeconds);
        float D = 0;
        foreach (var g in all)
            for (ushort ii = 0; ii < g.count; ii++)
            {
                var q = Instantiate(g.ship.Prefab);
                q.transform.SetPositionAndRotation(transform.position + (D * Random.onUnitSphere), Random.rotationUniform);
                var ship = q.GetComponent<Ship>();
                D += ship.size;
                ship.team = team;
                CreateFrame(ship);
                yield return null;
            }

        Destroy(this);
    }
}