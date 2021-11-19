using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public ShipData ship;

    public Team team;

    public void Start()
    {
        var obj = Instantiate(ship.Prefab);
        obj.GetComponent<Ship>().team = team;
        obj.transform.position = transform.position;
        obj.AddComponent<PlayerMark>();
        Spectator.Self.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}