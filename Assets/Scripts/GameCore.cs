using UnityEngine;

[DisallowMultipleComponent]
public class GameCore : SINGLETON<GameCore>
{//Someone can say that singletons are bad, and this class is too large, but...

    private System.Collections.Generic.Dictionary<Poolable, Transform> Types = new System.Collections.Generic.Dictionary<Poolable, Transform>();
    private System.Collections.Generic.List<COLLECTOR> collectors = new System.Collections.Generic.List<COLLECTOR>(8);
    private System.Collections.Generic.List<Ship> _all_;
    private System.Collections.Generic.List<Ship> All
    {
        get
        {
            if (_all_ == null)
                _all_ = new System.Collections.Generic.List<Ship>();
            return _all_;
        }
    }

    private void RemoveNull()
    {
        bool yes = true;
        while (yes)
        {
            yes = false;

            for (int i = 0; i < All.Count; i++)
            {
                if (!All[i])
                {
                    All.RemoveAt(i);
                    yes = true;
                    break;
                }
            }
        }
    }

    public static void Add(BaseEntity it)
    {
        GameCore me = Self;
        if (!me)
            me = FindObjectOfType<GameCore>();
        if (!me)
            return;
        me.RemoveNull();
        if (it is Ship s)
            me.All.Add(s);
        for (int i = 0; i < me.collectors.Count; i++)
            me.collectors[i].Add(it);
    }

    public static Camera MainCamera { get; private set; }

    public void Update()
    {
        if (!MainCamera)
            MainCamera = Camera.main;
    }

    private void Shuffle()
    {
        RemoveNull();
        if (All.Count > 1)
        {
            var temp = All[0];
            var i = Random.Range(1, All.Count);
            All[0] = All[i];
            All[i] = temp;
        }
    }

    public Ship FindTargetShip(Team team)
    {
        RemoveNull();
        Shuffle();
        foreach (var ship in All)
            if (ship.team != team)
                if (ship.team != Team.Derelict)
                    return ship;
        return null;
    }

    public void Explode(Vector3 where, float power, Team team)
    {
        RemoveNull();
        foreach (var q in All)
            if (q.team != team)
                q.OnDamaged(power / ((where - q.transform.position).sqrMagnitude + 1), null);
    }

    public void Add(COLLECTOR what)
    {
        if (!collectors.Contains(what))
            collectors.Add(what);
    }

    private Transform GetTransform(Poolable t)
    {
        if (Types.TryGetValue(t, out var q))
            return q;
        else
        {
            var obj = new GameObject(t.ToString()).transform;
            obj.SetParent(transform);
            Types[t] = obj;
            return obj;
        }
    }

    public GameObject GetFromPool(Poolable type)
    {
        var container = GetTransform(type);
        var all = container.childCount;
        for (int i = 0; i < all; i++)
        {
            var q = container.GetChild(i).gameObject;
            if (!q.activeSelf)
            {
                q.SetActive(true);
                q.GetComponent<PoolableComponent>().ReInit();
                return q;
            }
        }
        return Create(true, type);
    }

    private GameObject Create(bool active, Poolable type)
    {
        GameObject prefab;
        switch (type)
        {
            case Poolable.FlatProjectile:
                prefab = DataBase.Get().FlatProjectile;
                break;
            case Poolable.ParticleEffectProjectile:
                prefab = DataBase.Get().EffectProjectile;
                break;
            case Poolable.SmallExplosion:
                prefab = DataBase.Get().SmallExplosion;
                break;
            case Poolable.ShipExplosion:
                prefab = DataBase.Get().ShipExplosion;
                break;
            default:
                prefab = null;
                Debug.LogError(type);
                break;
        }

        var obj = Instantiate(prefab, GetTransform(type));
        obj.GetComponent<PoolableComponent>().ReInit();
        obj.SetActive(active);
        return obj;
    }

    public void MakeBoom(Vector3 pos, Poolable type, float size)
    {
        var q = GetFromPool(type).transform;
        q.position = pos;
        q.localScale = Vector3.one * size;
    }
}