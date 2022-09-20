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
            cooldown = 1;//all.Count;
            var at = Random.onUnitSphere;
            var cam = GameCore.MainCamera.transform;
            if (Vector3.Dot(cam.forward, at) > 0)
                return;
            var a = Instantiate(Utils.Choice(DataBase.Get().asteroids), GameCore.MainCamera.transform.position + (at * Random.Range(50f, 100f)), Quaternion.identity);
            all.Add(a);
            var rb = a.GetComponent<Rigidbody>();
            rb.velocity = Random.insideUnitSphere - at;
            rb.angularVelocity = Random.onUnitSphere;

            var scale = Random.Range(1, 10);
            rb.mass *= scale * scale;
            a.transform.localScale = Vector3.one * scale;
            a.GetComponent<Asteroid>().HP *= scale * scale;
        }
        else
            cooldown -= Time.deltaTime;
    }
}
