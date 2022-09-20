using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    List<GameObject> all;
    float cooldown;

    public void Start()
    {
        all = new List<GameObject>();
        cooldown = 1;
    }

    public void Update()
    {
        Utils.RemoveNull(all);
        if (cooldown < 0)
        {
            cooldown = all.Count;
            var at = Random.onUnitSphere;
            if (Vector3.Dot(GameCore.MainCamera.transform.forward, at) > 0)
                return;
            all.Add(Instantiate(Utils.Choice(DataBase.Get().asteroids), GameCore.MainCamera.transform.position + (at * Random.Range(50f, 100f)), Quaternion.identity));
        }
        else
            cooldown -= Time.deltaTime;
    }
}
