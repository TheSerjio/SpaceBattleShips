using System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public ShipData ship;

    public static void Spawn(ShipData what, Vector3 where, Quaternion rotation)
    {
        var obj = Instantiate(what.Prefab);
        var s = obj.GetComponent<Ship>();
        s.team = Team.Player;
        Spawner.CreateFrame(s);
        obj.transform.position = where;
        obj.transform.rotation = rotation;
        obj.AddComponent<PlayerMark>().SwitchPlayer();
        Spectator.Self.gameObject.SetActive(false);
    }

    public void Start()
    {
        Spawn(ship, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}