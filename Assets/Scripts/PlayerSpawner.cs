using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public ShipData ship;

    public Team team;

    public void Start()
    {
        var obj = Instantiate(ship.prefab);
        obj.team = team;
        obj.transform.position = transform.position;
        DestroyImmediate(obj.GetComponent<ShipController>());
        obj.gameObject.AddComponent<PlayerShip>();
        Destroy(gameObject);
    }
}