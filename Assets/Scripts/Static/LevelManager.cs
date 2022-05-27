public static class LevelManager
{
    public static Level currentLevel;
    public static ShipData[] startedWith;

    public static bool CanExitFromShip => currentLevel == null || currentLevel.IsCampaignLevel;

    public static void Complete(StarType star)
    {
        if (!currentLevel)
            return;

        var a = FileSystem.GetLevels().TryGetValue(currentLevel.BuildingIndex, out var some) ? some : System.Array.Empty<StarType>();
        if (a.Length != currentLevel.ships.Length)
            a = new StarType[currentLevel.ships.Length];
        for (var i = 0; i < a.Length; i++)
            if (currentLevel.ships[i] == startedWith[0])
                a[i] = star;

        FileSystem.Set(currentLevel, a);
    }
}