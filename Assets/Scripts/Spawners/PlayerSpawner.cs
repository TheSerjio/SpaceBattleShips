using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public ShipData ship;

    public static void Spawn(ShipData what, Vector3 where)
    {
        var obj = Instantiate(what.Prefab);
        var s = obj.GetComponent<Ship>();
        s.team = Team.Player;
        Spawner.CreateFrame(s);
        obj.transform.position = where;
        obj.AddComponent<PlayerMark>().SwitchPlayer();
        Spectator.Self.gameObject.SetActive(false);
    }

    public void Start()
    {
        Spawn(ship, transform.position);
        Destroy(gameObject);
    }
}