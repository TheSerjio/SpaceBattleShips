using UnityEngine;

public sealed class MotherShip : Ship
{
    [System.Serializable]
    public class Data
    {
        public ushort count;
        public ShipData ship;
    }

    [SerializeField] Transform[] spawners;

    [SerializeField] Data[] all;

    [SerializeField] int MaxAlive;

    Ship[] alive;

    public void FixedUpdate()
    {
        if (alive == null)
            alive = new Ship[MaxAlive];
        if (all != null)
        {
            foreach (var spawner in spawners)
            {
                for (int i = 0; i < MaxAlive; i++)
                    if (!alive[i])
                        foreach (var q in all)
                            if (q.count != 0)
                                if (q.ship)
                                {
                                    q.count--;
                                    var ship = Instantiate(q.ship.prefab, spawner.position, spawner.rotation).GetComponent<Ship>();
                                    ship.team = team;
                                    var point = ship.transform.position * 2 - transform.position + (Random.onUnitSphere * Vector3.Distance(ship.transform.position, transform.position) / 2);
                                    ship.Warn(point, new Warning(false, 0));
                                    ship.transform.LookAt(point);
                                    alive[i] = ship;
                                    Spawner.CreateFrame(ship);
                                    ship.ImmuneUntil = Time.time + Spawner.time;
                                    goto End;//because i cant use "return" or "break"
                                }
                End:;
            }
        }
    }

    public override void DrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (var q in spawners)
        {
            Gizmos.DrawWireSphere(q.position, 1);
            Gizmos.DrawLine(q.position, q.forward + q.position);
        }

    }
}