using UnityEngine;

[DisallowMultipleComponent]
public class GameCore : SINGLETON<GameCore>
{//Someone can say that singletons are bad, and this class is too large, but...

    private readonly System.Collections.Generic.Dictionary<Poolable, Transform> Types = new System.Collections.Generic.Dictionary<Poolable, Transform>();
    private readonly System.Collections.Generic.List<COLLECTOR> collectors = new System.Collections.Generic.List<COLLECTOR>(8);
    private System.Collections.Generic.List<Ship> _all_;
    private System.Collections.Generic.List<Ship> All => _all_ ??= new System.Collections.Generic.List<Ship>();

    private System.Collections.Generic.Dictionary<Team, ulong> Counts;

    private Team[] allTeams = Utils.EnumValues<Team>();

    public SunContainer sun;

    public MotherShip PlayerMotherShip, EnemyMotherShip;

    private void RemoveNull()
    {
        Utils.RemoveNull(All);
    }

    private State state = State.Nothing;

    private enum State : byte
    {
        Nothing,
        PlayerWin,
        PlayerFail
    }

    protected override void OnSingletonAwake()
    {
        Counts = new System.Collections.Generic.Dictionary<Team, ulong>();
        foreach (var q in allTeams)
            Counts[q] = 0;
    }

    public void Start()
    {
        var level = LevelManager.currentLevel;
        if (level)
        {
            print(level.Name);
            if (level.IsCampaignLevel)
            {
                EnemyMotherShip.all = level.enemyes;
                PlayerMotherShip.all = MainMenu.PlayerShips_C.ToArray();
            }
            else
            {
                var rotation = Quaternion.identity;
                var spawnAt = Vector3.zero;
                {
                    var point = SpawnPlayerHere.Self;
                    if (point)
                    {
                        spawnAt = point.transform.position;
                        rotation = point.transform.rotation;
                    }
                }
                PlayerSpawner.Spawn(LevelManager.startedWith[0], spawnAt, rotation);
            }

            if (level.sun)
                sun.Do(level.sun);
        }

        Cursor.SetCursor(DataBase.Get().GameCursor, Vector2.one * 15, CursorMode.Auto);

        StartCoroutine(CoreCoroutine());
    }


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
        foreach (var t in me.collectors)
            t.Add(it);
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

    private System.Collections.IEnumerator CoreCoroutine()
    {
        while (this)
        {
            foreach (var q in allTeams)
                Counts[q] = 0;

            var anyPlayers = false;
            var anyEnemyes = false;
            
            foreach (var q in All)
                if (q)
                {
                    Counts[q.team]++;
                    if (q.team == Team.Player)
                        anyPlayers = true;
                    else
                        anyEnemyes = true;
                }

            if (state == State.Nothing)
            {
                if (!anyEnemyes)
                {
                    if (!FindObjectOfType<Spawner>())
                        if (!FindObjectOfType<ChainSpawner>())
                        {
                            state = State.PlayerWin;
                            GameUI.Self.Operate(GameUI.Self.WinText);

                            StarType star;

                            var player = PlayerMark.Self;
                            if (player)
                                if (player.Ship.RelativeHealth > 0.99f)
                                    star = StarType.Best;
                                else
                                    star = StarType.Good;
                            else
                                star = StarType.Bad;

                            LevelManager.Complete(star);
                        }
                }
                else if (!anyPlayers)
                {
                    state = State.PlayerFail;
                    GameUI.Self.Operate(GameUI.Self.FailText);
                }
            }

            var s = "";
            foreach (var q in Counts)
            {
                if (q.Value != 0)
                    s += $"{q.Key}:{q.Value}\n";
                yield return null;
            }

            GameUI.Self.ShipCount.text = s;

            yield return null;
        }
    }

    public Ship FindTargetShip(Team team, bool sameTeam, Locating mode, Transform from)
    {
        RemoveNull();
        Ship q = null;
        var min = float.MaxValue;
        
        foreach (var ship in All)
            if (ship.team == team == sameTeam)
                if (ship.team != Team.Derelict)
                {
                    var f = mode.Get(from, ship);
                    if (min > f)
                    {
                        min = f;
                        q = ship;
                    }

                }

        return q;
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

    public T GetFromPool<T>(Poolable type) where T:PoolableComponent
    {
        var container = GetTransform(type);
        var all = container.childCount;
        for (int i = 0; i < all; i++)
        {
            var q = container.GetChild(i).gameObject;
            if (!q.activeSelf)
            {
                q.SetActive(true);
                var pc = q.GetComponent<T>();
                pc.ReInit();
                return pc;
            }
        }

        return Create<T>(true, type);
    }

    private T Create<T>(bool active, Poolable type) where T : PoolableComponent
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
        var pc = obj.GetComponent<T>();
        pc.ReInit();
        obj.SetActive(active);
        return pc;
    }

    public void MakeBoom(Vector3 pos, Poolable type, float size)
    {
        var q = GetFromPool<Explosion>(type).transform;
        q.position = pos;
        q.localScale = Vector3.one * size;
    }
}