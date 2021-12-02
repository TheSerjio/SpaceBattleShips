using UnityEngine;

public static class LevelManager
{
    public enum Type
    {
        Level,
        Campaign
    }

    public static Type type;
    public static Level currentLevel;
    public static MotherShip.Data[] startedWith;
}