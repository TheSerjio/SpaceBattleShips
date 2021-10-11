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
    public ulong cost;
    public GameObject prefab;
    public Type type;

    public override IEnumerable<Warning> Validate()
    {
        if (!prefab)
            yield return new Warning(Level.Error, "no prefab");
        if (cost == 0)
            yield return new Warning(Level.Warning, "zero cost");
        if (string.IsNullOrWhiteSpace(Name))
            yield return new Warning(Level.Warning, "no name");
    }
}