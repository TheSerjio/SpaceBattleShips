using UnityEngine;

public sealed class MotherShip : Ship
{
    [System.Serializable]
    public class Data
    {
        public ushort count;
        public ShipData ship;
    }

    [SerializeField] private Transform[] spawners;

    [SerializeField] private Data[] all;

    [SerializeField] private int MaxAlive;

    private Ship[] alive;

    public float TotalCost;

    public void OnValidate()
    {
        TotalCost = Utils.GetCost(all);
    }

    public bool SpawnRandomShips;

    public float CoolDown;

    public void FixedUpdate()
    {
        alive ??= new Ship[MaxAlive];
        if (all != null)
        {
            foreach (var spawner in spawners)
            {
                for (var i = 0; i < MaxAlive; i++)
                    if (!alive[i])
                        foreach (var q in all)
                            if (q.count != 0)
                                if (q.ship)
                                {
                                    q.count--;
                                    var ship = Instantiate(q.ship.Prefab, spawner.position, spawner.rotation).GetComponent<Ship>();
                                    ship.team = team;
                                    var point = spawner.position + (spawner.forward * 100);
                                    ship.Warn(point, new Warning(false, 0));
                                    ship.transform.LookAt(point);
                                    alive[i] = ship;
                                    Spawner.CreateFrame(ship);
                                    ship.ImmuneUntil = Time.time + Utils.StartTime;
                                    goto End;//because i cant use "return" or "break"
                                }
                End:;
            }
        }
    }

    protected override void DrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (var q in spawners)
        {
            Gizmos.DrawWireSphere(q.position, 1);
            Gizmos.DrawLine(q.position, q.forward + q.position);
        }

    }
}