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
}