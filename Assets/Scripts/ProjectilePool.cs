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
        return Create(true);
    }

    private GameObject Create(bool active)
    {
        var obj = Instantiate(prefab);
        obj.transform.SetParent(transform, false);
        obj.SetActive(active);
        return obj;
    }

    public void Add(GameObject what)
    {
        what.SetActive(false);
    }

    public void Start()
    {
        StartCoroutine(Coro());
    }

    private System.Collections.IEnumerator Coro()
    {
        while (transform.childCount < 256)//i love magic numbers
        {
            Create(false);
            yield return null;
        }
    }
}