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
    public GameObject Prefab => prefab.gameObject;
    [SerializeField] Ship prefab;
    public Type type;
    public Sprite Preview;

    public override IEnumerable<Warning> Validate()
    {
        if (!Prefab)
            yield return Warn(Level.Error, "no prefab");
        if (cost == 0)
            yield return Warn(Level.Warning, "zero cost");
        if (string.IsNullOrWhiteSpace(Name))
            yield return Warn(Level.Warning, "no name");
        if (!Preview)
            yield return Warn(Level.Warning, "no preview");
    }
}