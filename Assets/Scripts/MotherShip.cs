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
                                    ship.Warn(ship.transform.position * 2 - transform.position, new Warning(false, 0));
                                    alive[i] = ship;
                                    Spawner.CreateFrame(ship);
                                    Destroy(ship.gameObject.AddComponent<JustSpawned>(), JustSpawned.Duration);
                                    goto End;//because i cant use "return" or "break"
                                }
                End:;
            }
        }
    }
}