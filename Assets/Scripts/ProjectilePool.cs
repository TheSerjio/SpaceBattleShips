using UnityEngine;

public sealed class ProjectilePool : SINGLETON<ProjectilePool>
{
    public GameObject prefab;

    public GameObject Get()
    {
        var all = transform.childCount;
        for (int i = 0; i < all; i++)
        {
            var q = transform.GetChild(i).gameObject;
            if (!q.activeSelf)
            {
                q.SetActive(true);
                return q;
            }
        }
        var obj = Instantiate(prefab);
        obj.transform.SetParent(transform, false);
        return obj;
    }

    public void Add(GameObject what)
    {
        what.SetActive(false);
    }
}