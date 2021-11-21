using UnityEngine;

public sealed class DataBase : ScriptableObject
{
    private static DataBase self;

    public static DataBase Get()
    {
        if (!self)
            self = Resources.Load<DataBase>("System");
        return self;
    }

    public GameObject TargetFramePrefab;
    public GameObject CameraPrefab;
    public GameObject DustPrefab;
    public GameObject ShipExplosion;
    public GameObject SmallExplosion;
    public GameObject FlatProjectile;
    public GameObject EffectProjectile;

    public Color PlayerColor;
    public Color Defenders;
    public Color Attackers;
    public Color Pirates;

    public ShipData[] Ships;

    public ValidableScriptableObject[] EveryThing;

#if UNITY_EDITOR
    [ContextMenu("Find all")]
    public void Execute()
    {
        Ships = FindAll<ShipData>();
        EveryThing = FindAll<ValidableScriptableObject>();
    }

    private static T[] FindAll<T>() where T : ValidableScriptableObject
    {
        var ids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
        var all = new System.Collections.Generic.List<T>();

        static string Q(string q) => q.ToLower().Replace(" ", "");

        foreach (var id in ids)
        {
            var it = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(id));
            if (it)
                all.Add(it);
        }
        var r = all.ToArray();

        foreach (var i1 in r)
            foreach (var i2 in r)
                if (i1 != i2)
                    if (Q(i1.name) == Q(i2.name))
                        Debug.LogError($"Duplication {i1.GetType().Name}~{i2.GetType().Name}: {i1.name}");

        return r;
    }

    [ContextMenu("Validate")]
    public void Check()
    {
        Debug.ClearDeveloperConsole();
        var all = new System.Collections.Generic.List<ValidableScriptableObject.Warning>();
        var objs = FindAll<ValidableScriptableObject>();
        foreach (var q in objs)
        {
            foreach (var qq in q.Validate())
            {
                var qw = qq;
                qw.parent = q;
                all.Add(qq);
            }
        }

        foreach (var q in all)
        {
            string txt = $"{q.parent.name}:{q.text}";
            switch (q.level)
            {
                case ValidableScriptableObject.Level.Message:
                    Debug.Log(txt);
                    break;
                case ValidableScriptableObject.Level.Warning:
                    Debug.LogWarning(txt);
                    break;
                case ValidableScriptableObject.Level.Error:
                    Debug.LogError(txt);
                    break;
            }
        }
    }

    [ContextMenu("Ship stats")]
    public void CountShipStats()
    {
        Temp<ShipData>[] all = new Temp<ShipData>[Ships.Length];
        for (int i = 0; i < Ships.Length; i++)
        {
            var obj = Ships[i].Prefab;
            float dps = 0;
            foreach (var weapon in obj.GetComponentsInChildren<ShipWeapon>())
                dps += weapon.MaxDPS();
            var q = new Temp<ShipData>()
            {
                value = dps,
                obj = Ships[i]
            };
            all[i] = q;
        }
        System.Array.Sort(all);
        foreach (var q in all)
            Debug.Log($"{q.obj.Name}:{q.value}");
    }

    struct Temp<T> : System.IComparable<Temp<T>> where T : Object
    {
        public float value;
        public T obj;

        public int CompareTo(Temp<T> other) => value.CompareTo(other.value);
    }
#endif

    public Color TeamColor(Team team)
    {
        switch (team)
        {
            case Team.Defenders: return Defenders;
            case Team.Attackers: return Attackers;
            case Team.Pirates: return Pirates;
            case Team.Player: return PlayerColor;
            default: return Random.ColorHSV();
        }
    }
}