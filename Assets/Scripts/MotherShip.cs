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

    public override void UseExtraAbility()
    {
        if (all != null)
        {
            foreach(var spw in spawners)
            foreach (var q in all)
                if (q.count != 0)
                {
                    q.count--;
                        if (q.ship)
                            Instantiate(q.ship.prefab, spw.position, spw.rotation).GetComponent<Ship>().team = team;
                    break;
                }
        }
    }
}