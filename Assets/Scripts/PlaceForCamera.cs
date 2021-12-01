using UnityEngine;

[DisallowMultipleComponent]
public class PlaceForCamera : MonoBehaviour
{
    public enum Type
    {
        Parent,
        Default,
        Sniper
    }

    public Type type;
}