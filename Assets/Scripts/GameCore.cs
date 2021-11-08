using UnityEngine;

[DisallowMultipleComponent]
public class GameCore : SINGLETON<GameCore>
{
    public COLLECTOR[] Collectors;
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
    private System.Collections.Generic.List<Explosion> Booms = new System.Collections.Generic.List<Explosion>();

    private struct Explosion
    {
        public Ship from;
        public Vector3 position;
        public float damage;
    }

    public Canvas canvas;

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
        for (int i = 0; i < me.Collectors.Length; i++)
            me.Collectors[i].Add(it);
    }

    public static Camera MainCamera { get; private set; }

    public void Update()
    {
        if (!MainCamera)
            MainCamera = Camera.main;
        if (Booms.Count > 0)
        {
            Shuffle();
            var b = Booms[0];
            Booms.RemoveAt(0);
            foreach (var q in All)
                q.OnDamaged(b.damage / Vector3.Distance(b.position, q.transform.position), null);
        }
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

    public Ship FindTargetShip(BaseEntity.Team team)
    {
        Shuffle();
        foreach (var ship in All)
            if (ship.team != team)
                if (ship.team != BaseEntity.Team.Derelict)
                    return ship;
        return null;
    }

    public void Explode(Vector3 where, float power, Ship from)
    {
        var q = new Explosion()
        {
            damage = power,
            position = where,
            from = from
        };
        Booms.Add(q);
    }
}