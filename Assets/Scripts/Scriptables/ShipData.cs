using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Ship data")]
public class ShipData : ValidableScriptableObject
{
    public enum Type
    {
        Free, Simple, Boss
    }

    public string Name;
    public int cost;
    public GameObject Prefab => prefab.gameObject;
    public Ship prefab;
    public Type type;
    public Sprite Preview;

    public override IEnumerable<Warning> Validate()
    {
        if (!Prefab)
            yield return Warn(Level.Error, "no prefab");
        if (cost == 0)
            yield return Warn(Level.Message, "zero cost");
        if (string.IsNullOrWhiteSpace(Name))
            yield return Warn(Level.Warning, "no name");
        if (!Preview)
            yield return Warn(Level.Warning, "no preview");
        if (!prefab.mainWeapon)
            yield return Warn(Level.Warning, "no weapon");
    }

    public int Index
    {
        get
        {
            for (int i = 0; i < DataBase.Get().Ships.Length; i++)
            {
                ShipData q = DataBase.Get().Ships[i];
                if (q == this)
                    return i;
            }
            return 0;
        }
    }
}