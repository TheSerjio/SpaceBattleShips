using UnityEngine;

[DisallowMultipleComponent]
public class GameCore : SINGLETON<GameCore>
{//Someone can say that singletons are bad, and this class is too large, but...

    private readonly System.Collections.Generic.Dictionary<Poolable, Transform> Types = new System.Collections.Generic.Dictionary<Poolable, Transform>();
    private readonly System.Collections.Generic.List<COLLECTOR> collectors = new System.Collections.Generic.List<COLLECTOR>(8);
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

    private System.Collections.Generic.Dictionary<Team, ulong> Counts;

    public Camera EditorCamera;
    
    private void RemoveNull()
    {
        var yes = true;
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

    protected override void OnAwake()
    {
        Counts = new System.Collections.Generic.Dictionary<Team, ulong>();
        foreach (var q in System.Enum.GetValues(typeof(Team)))
            Counts[(Team)q] = 0;
        //StartCoroutine(Initialize());
    }

    /*private System.Collections.IEnumerator Initialize()
    {
        Time.timeScale = 0;
        foreach (var q in System.Enum.GetValues(typeof(Poolable)))
        {
            var p = (Poolable)q;
            var t = GetTransform(p);
            while(t.childCount<100)
            {
                Create(false, p);
                yield return null;
            }
        }
        Debug.LogWarning("Finished!");
        Time.timeScale = 1;
    }*/

    public static void Add(BaseEntity it)
    {
        var me = Self;
        if (!me)
            me = FindObjectOfType<GameCore>();
        if (!me)
            return;
        me.RemoveNull();
        if (it is Ship s)
            me.All.Add(s);
        for (var i = 0; i < me.collectors.Count; i++)
            me.collectors[i].Add(it);
    }

    public static Camera MainCamera
    {
        get
        {
            if (!_cam)
            {
                _cam = FindObjectOfType<Camera>();
                if (!_cam)
                {
                    _cam = FindObjectOfType<Camera>(true);
                    _cam.enabled = true;
                    _cam.gameObject.SetActive(true);
                }

            }
            return _cam;
        }
        set
        {
            if (value)
                _cam = value;
        }
    }

    private static Camera _cam;

    public void FixedUpdate()
    {
        EditorCamera = MainCamera;
        Time.timeScale = Mathf.Clamp01(1f / Time.deltaTime);
        RemoveNull();
        foreach(var q in System.Enum.GetValues(typeof(Team)))
            Counts[(Team)q] = 0;
        foreach(var q in All)
            Counts[q.team]++;
        string s = "";
        foreach(var q in Counts)
        {
            if (q.Value != 0)
                s += $"{q.Key}:{q.Value}\n";
        }
        GameUI.Self.ShipCount.text = s;
    }

    public Ship FindTargetShip(Team team, bool sameTeam, Locating mode, Transform from)
    {
        RemoveNull();
        foreach (var q in All)
            q.GameCoreCachedValue = mode.Get(from, q);
        All.Sort();//TODO remove it
        foreach (var ship in All)
            if ((ship.team == team) == sameTeam)
                if (ship.team != Team.Derelict)
                    return ship;
        return null;
    }

    public void Explode(Vector3 where, float power, Team team)
    {
        RemoveNull();
        foreach (var q in All)
            if (q.team != team)
                q.OnDamaged(q.size * power / ((where - q.transform.position).sqrMagnitude + 1), null);
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
        var t = GetTransform(type);
//        Debug.Log($"Created {type} - {t.childCount}");
        var obj = Instantiate(prefab, t);
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