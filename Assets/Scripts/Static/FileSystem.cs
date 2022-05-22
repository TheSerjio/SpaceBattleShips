using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileSystem
{
    private static string LevelsPath => $"{Directory.GetCurrentDirectory()}/Levels.save";
    private static string CampaignPath => $"{Directory.GetCurrentDirectory()}/Campaign.save";

    public static void Set(Level lvl, StarType[] what)
    {
        var q = GetLevels();
        q[lvl.BuildingIndex] = what;
        Set(q);
    }

    public static Dictionary<int, StarType[]> GetLevels()
    {
        var result = new Dictionary<int, StarType[]>();
        if (!File.Exists(LevelsPath))
            return new Dictionary<int, StarType[]>();

        try
        {
            using var file = new BinaryReader(File.OpenRead(LevelsPath));
            var it = new List<uint>();
            while (file.PeekChar() != -1)
            {
                it.Clear();
                var text = "";
                while (true)
                {
                    var some = file.ReadChar();
                    if (some == ' ')
                    {
                        it.Add(uint.Parse(text));
                        text = "";
                    }
                    else if (some == '\n')
                    {
                        it.Add(uint.Parse(text));
                        break;
                    }
                    else if (file.PeekChar() == -1)
                    {
                        it.Add(uint.Parse(text + some));
                        break;
                    }
                    else
                        text += some;
                }

                var index = it[0];
                it.RemoveAt(0);

                var array = new StarType[it.Count];

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = (StarType)it[i];
                }

                result[(int)index] = array;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            File.Delete(LevelsPath);
            result.Clear();
        }

        return result;
    }

    public static Dictionary<int, (Dictionary<ShipData, int>, int)> GetCampaign()
    {
        var result = new Dictionary<int, (Dictionary<ShipData, int>, int)>();
        if (!File.Exists(CampaignPath))
            return new Dictionary<int, (Dictionary<ShipData, int>, int)>();

        try
        {
            using var file = new BinaryReader(File.OpenRead(CampaignPath));
            var it = new List<int>();
            while (file.PeekChar() != -1)
            {
                it.Clear();
                var text = "";
                while (true)
                {
                    var some = file.ReadChar();
                    if (some == ' ')
                    {
                        it.Add(int.Parse(text));
                        text = "";
                    }
                    else if (some == '\n')
                    {
                        it.Add(int.Parse(text));
                        break;
                    }
                    else if (file.PeekChar() == -1)
                    {
                        it.Add(int.Parse(text + some));
                        break;
                    }
                    else
                        text += some;
                }

                var index = it[0];
                it.RemoveAt(0);

                var array = new StarType[it.Count];

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = (StarType)it[i];
                }

                result[index] = default;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            File.Delete(CampaignPath);
            result.Clear();
        }

        return result;
    }

    public static void Set(Dictionary<int, StarType[]> data)
    {
        using var file = new StreamWriter(File.OpenWrite(LevelsPath));

        foreach (var pair in data)
        {
            file.Write(pair.Key.ToString());
            foreach (var star in pair.Value)
            {
                file.Write(" ");
                file.Write(((byte)star).ToString());
            }

            file.Write("\n");
        }
    }

    public static void Set(Dictionary<int, (Dictionary<ShipData, int>, int)> data)
    {
        using var file = new StreamWriter(File.OpenWrite(LevelsPath));

        foreach (var pair in data)
        {
            file.Write(pair.Key.ToString());
            file.Write(" ");
            file.Write(pair.Value.Item2);
            file.Write(" ");
            file.Write(pair.Value.Item1.Count);
            foreach (var line in pair.Value.Item1)
            {
                file.Write(" ");
                file.Write(line.Key.Index.ToString());
                file.Write(" ");
                file.Write(line.Value.ToString());
            }

            file.Write("\n");
        }
    }
}