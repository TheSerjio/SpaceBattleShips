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
            if (it != null)
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
        foreach (var q in FindAll<ValidableScriptableObject>())
            try
            {
                foreach (var qq in q.Validate())
                {
                    var qw = qq;
                    qw.parent = q;
                    all.Add(qq);
                }
            }
            catch
            {
                Debug.Log("kek");
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
#endif
}