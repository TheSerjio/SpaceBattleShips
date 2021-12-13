using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileSystem
{
    private static string FilePath => $"{Directory.GetCurrentDirectory()}/Save.save";

    public static StarType[] Get(Level lvl)
    {
        return Get().TryGetValue(lvl.BuildingIndex, out var some) ? some : System.Array.Empty<StarType>();
    }

    public static void Set(Level lvl, StarType[] what)
    {
        var q = Get();
        q[lvl.BuildingIndex] = what;
        Set(q);
    }

    private static void CheckFile()
    {
        if (!File.Exists(FilePath))
            File.Create(FilePath).Dispose();
    }

    private static Dictionary<int, StarType[]> Get()
    {
        CheckFile();
        var result = new Dictionary<int, StarType[]>();

        try
        {
            using var file = new BinaryReader(File.OpenRead(FilePath));
            var it = new List<byte>();
            while (file.PeekChar() != -1)
            {
                it.Clear();
                var text = "";
                while (true)
                {
                    var some = file.ReadChar();
                    if (some == ' ')
                    {
                        it.Add(byte.Parse(text));
                        text = "";
                    }
                    else if (some == '\n')
                    {
                        it.Add(byte.Parse(text));
                        break;
                    }
                    else if (file.PeekChar()==-1)
                    {
                        it.Add(byte.Parse(text + some));
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
                    array[i] = (StarType) it[i];
                }

                result[index] = array;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{e.GetType().FullName}\n\n\n{e.Message}\n\n\n{e.StackTrace}");
            //if (File.Exists(FilePath))
              //  File.Delete(FilePath);
            result.Clear();
        }

        return result;
    }

    private static void Set(Dictionary<int, StarType[]> data)
    {
        CheckFile();
        Debug.LogError("...");
    }
}