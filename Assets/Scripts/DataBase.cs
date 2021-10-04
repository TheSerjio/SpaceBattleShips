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
}