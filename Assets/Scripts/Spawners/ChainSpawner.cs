using UnityEngine;

public class ChainSpawner : MonoBehaviour
{
    public float TimeBeforeFirstGroup;
    public float TimeBetweenGroups;
    public Team team;
    public MotherShip.Data[] all;

    public float TotalCost;

    public void OnValidate()
    {
        TotalCost = Utils.GetCost(all);
    }

    public void Start()
    {
        StartCoroutine(Summon());
    }
    private System.Collections.IEnumerator Summon()
    {
        float D = 0;
        var prev = new System.Collections.Generic.List<Ship>();
        yield return new WaitForSeconds(TimeBeforeFirstGroup);
        foreach (var g in all)
        {
            prev.Clear();
            for (ushort ii = 0; ii < g.count; ii++)
            {
                var q = Instantiate(g.ship.Prefab);
                q.transform.SetPositionAndRotation(transform.position + (D * Random.onUnitSphere),
                    Random.rotationUniform);
                var ship = q.GetComponent<Ship>();
                prev.Add(ship);
                D += ship.size;
                ship.team = team;
                Spawner.CreateFrame(ship);
                yield return null;
            }

            while (prev.Count > 0)
            {
                Utils.RemoveNull(prev);
                yield return null;
            }

            yield return new WaitForSeconds(TimeBetweenGroups);
        }

        Destroy(gameObject);
    }
}