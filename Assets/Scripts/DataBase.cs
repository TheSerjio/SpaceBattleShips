using UnityEngine;

public sealed class DataBase : ScriptableObject
{
    private static DataBase self;

    public DataBase Get()
    {
        if (!self)
            self = Resources.Load<DataBase>("System");
        return self;
    }

    public GameObject SimpleProjectile;
}