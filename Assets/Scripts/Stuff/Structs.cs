using UnityEngine;

[System.Serializable]
public struct Locating
{
    public enum Sorting : byte { Min, Any, Max }

    public Sorting Distance;
    public Sorting Size;
    public bool InFrontOfMe;
    public bool SomeRandom;

    public float Get(Transform from, Ship tar)
    {
        //small = first
        float q = 1;

        var targetPos = tar.transform.position;
        switch (Distance)
        {
            case Sorting.Min:
                q *= Vector3.Distance(from.position, targetPos);
                break;
            case Sorting.Max:
                q /= Vector3.Distance(from.position, targetPos) + 1;
                break;
        }

        switch (Size)
        {
            case Sorting.Min:
                q *= tar.size;
                break;
            case Sorting.Max:
                q /= tar.size;
                break;
        }

        if (InFrontOfMe)
            q *= Vector3.Dot(from.forward, (from.position - targetPos).normalized) + 1;

        if (SomeRandom)
            q *= Random.value;

        return q;
    }
}