using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public ShipData ship;

    public Team team;

    public void Start()
    {
        var obj = Instantiate(ship.prefab);
        obj.GetComponent<Ship>().team = team;
        obj.transform.position = transform.position;
        DestroyImmediate(obj.GetComponent<ShipController>());
        obj.AddComponent<PlayerShip>();
        Destroy(gameObject);
    }
}