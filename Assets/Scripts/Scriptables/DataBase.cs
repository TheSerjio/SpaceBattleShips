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

    public ValidableScriptableObject[] EveryThing;

    public AnimationCurve EngineSizeFromPower;

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
        foreach (var q in Ships)
        {
            float dps = 0;
            foreach (var weapon in q.Prefab.GetComponentsInChildren<ShipWeapon>())
                dps += weapon.MaxDPS();
            Debug.Log($"{q.Name}: DPS: {dps}; Range: {q.prefab.mainWeapon.MaxFireDist}");
        }
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