using System.IO;

public static class SettingsSaver
{
    private static readonly System.Collections.Generic.List<Property> All = new System.Collections.Generic.List<Property>();
    public abstract class Property
    {
        public readonly string name;
        public abstract void Save(TextWriter to);
        public abstract void Read(TextReader from);

        protected Property(string Name)
        {
            name = Name;
            All.Add(this);
        }
    }

    public abstract class Property<T> : Property where T : struct
    {
        public T Value { get; set; }

        protected Property(string Name,T val) : base(Name)
        {
            Value = val;
        }
    }

    public sealed class FloatSaver : Property<float>
    {
        public override void Read(TextReader from)
        {
            throw new System.NotImplementedException();
        }

        public override void Save(TextWriter to)
        {
            throw new System.NotImplementedException();
        }

        public FloatSaver(string Name, float val) : base(Name, val)
        {
        }
    }

    private static string Path => $"{Directory.GetCurrentDirectory()}\\.settings";

    public static void Save()
    {
        var file = new StreamWriter(Path);
        foreach (var q in All)
            q.Save(file);
    }

    public static void Read()
    {
        var file = new StreamReader(Path);
        foreach (var q in All)
            q.Read(file);
    }
}