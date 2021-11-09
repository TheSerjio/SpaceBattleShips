using UnityEngine;

public sealed class ProjectilePool : SINGLETON<ProjectilePool>
{
    public enum Projectile : byte { Flat, ParticleEffect }

    private System.Collections.Generic.Dictionary<Projectile, Transform> Types = new System.Collections.Generic.Dictionary<Projectile, Transform>();

    private Transform GetTransform(Projectile t)
    {
        if (Types.TryGetValue(t, out var q))
            return q;
        else
        {
            var obj = new GameObject(t.ToString()).transform;
            Types[t] = obj;
            return obj;
        }
    }

    public GameObject Get(Projectile type)
    {
        var container = GetTransform(type);
        var all = container.childCount;
        for (int i = 0; i < all; i++)
        {
            var q = container.GetChild(i).gameObject;
            if (!q.activeSelf)
            {
                q.SetActive(true);
                return q;
            }
        }
        return Create(true, type);
    }

    private GameObject Create(bool active, Projectile type)
    {
        GameObject prefab;
        switch (type)
        {
            case Projectile.Flat:
                prefab = DataBase.Get().FlatProjectile;
                break;
            case Projectile.ParticleEffect:
                prefab = DataBase.Get().EffectProjectile;
                break;
            default:
                prefab = null;
                Debug.LogError(type);
                break;
        }

        var obj = Instantiate(prefab);
        obj.transform.SetParent(GetTransform(type), false);
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
        foreach (var i in System.Enum.GetValues(typeof(Projectile)))
        {
            var type = (Projectile)i;
            var q = GetTransform(type);
            while (q.childCount < 256)//i love magic numbers
            {
                Create(false, type);
                yield return null;
            }
        }
    }
}