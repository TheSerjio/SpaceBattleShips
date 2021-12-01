using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public GameObject BetterTrailPrefab;

    public Color PlayerColor;
    public Color Defenders;
    public Color Attackers;
    public Color Pirates;

    public ShipData[] Ships;
    public Level[] Levels;

    public ValidableScriptableObject[] EveryThing;

    public AnimationCurve EngineSizeFromPower;

#if UNITY_EDITOR

    [SerializeField] private TextAsset file;
    
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
                all.Add(qw);
            }
        }

        foreach (var q in all)
        {
            var txt = $"{q.parent.name}:{q.text}";
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [ContextMenu("ShipStats")]
    public void ShipStats()
    {
        var stats = new[]
        {
            "Name", "Cost", "HP", "S", "+S+", "Full_S", "E", "+E+", "Full_E", "DPS", "WEC", "Bullets", "Fire time",
            "Shift",
        };
        var data = new string[stats.Length, Ships.Length];
        var sizes = new int[stats.Length];
        for (var x = 0; x < stats.Length; x++)
            sizes[x] = stats[x].Length + 2;
        for (var i = 0; i < Ships.Length; i++)
        {
            var x = 0;

            void Do(string q)
            {
                sizes[x] = Mathf.Max(sizes[x], q.Length + 2);
                data[x, i] = q;
                x++;
            }

            void DoN(float q) => Do(Mathf.RoundToInt(q).ToString());
            
            var ship = Ships[i].prefab;

            Do(Ships[i].Name);
            Do(Ships[i].cost.ToString());

            DoN(ship.MaxHealth);
            var s = ship.GetComponent<Shield>();
            if (s)
            {
                DoN(s.MaxShield);
                DoN(s.ShieldRegeneration);
                DoN(s.MaxShield / s.ShieldRegeneration);
            }
            else
            {
                Do("X");
                Do("X");
                Do("X");
            }

            DoN(ship.MaxEnergy);
            DoN(ship.EnergyRegeneration);
            DoN(ship.MaxEnergy/ ship.EnergyRegeneration);

            float dps = 0;
            float wc = 0;
            float bullets = 0;
            var plus = false;
            foreach (var weapon in ship.GetComponentsInChildren<ShipWeapon>())
            {
                dps += weapon.S_MaxDPS();
                wc += weapon.S_EnergyConsumption;
                bullets += weapon.S_Bullets;
                if(weapon is SimpleWeapon sw)
                    if (sw.ProjectileExplosionPower != 0)
                        plus = true;
            }

            if (plus)
                Do(Mathf.RoundToInt(dps) + "+");
            else
                DoN(dps);

            DoN(wc);

            DoN(bullets);

            DoN(ship.MaxEnergy / (wc - ship.EnergyRegeneration));

            DoN(ship.MaxEnergy / (Utils.EnergyConsumption(Ship.MaxEngine) * ship.EngineCons - ship.EnergyRegeneration));
        }

        var path = $"{System.IO.Directory.GetCurrentDirectory()}\\{UnityEditor.AssetDatabase.GetAssetPath(file)}";
        path = path.Replace('/', '\\');
        
        
        
        var F = new System.IO.StreamWriter(path, false);

        for(var x=0;x<stats.Length;x++)
        {
            F.Write(stats[x]);
            F.Write(new string(' ', sizes[x] - stats[x].Length));
        }
        F.Write('\n');

        for (var y = 0; y < Ships.Length; y++)
        {
            for (var x = 0; x < stats.Length; x++)
            {
                var q = data[x, y];
                q += new string(' ', sizes[x] - q.Length);
                F.Write(q);
            }

            F.Write('\n');
        }
        
        F.WriteLine("S = Shield = Щит");
        F.WriteLine("E = Energy = Энергия");
        F.WriteLine("WEC = Weapon Energy Consumption = Сколько энегрии оружия тратят в секудну");
        F.WriteLine("Fire time = За сколько секунд оружия потратят всю энергию корабля который стоит на месте. Если меньше нуля, то может стрелять бесконечно");
        F.WriteLine("Shift = Сколько секунд можно лететь на шифте");

        F.Flush();
        F.Close();
        F.Dispose();

    }
#endif

    public Color TeamColor(Team team)
    {
        return team switch
        {
            Team.Defenders => Defenders,
            Team.Attackers => Attackers,
            Team.Pirates => Pirates,
            Team.Player => PlayerColor,
            _ => Random.ColorHSV(),
        };
    }
}