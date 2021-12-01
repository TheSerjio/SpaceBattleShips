using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public ShipData ship;

    public Team team;

    public void Start()
    {
        var obj = Instantiate(ship.Prefab);
        var s = obj.GetComponent<Ship>();
        s.team = team;
        Spawner.CreateFrame(s);
        obj.transform.position = transform.position;
        obj.AddComponent<PlayerMark>().SwitchPlayer();
        Spectator.Self.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}