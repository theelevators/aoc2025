using System.Reflection;

namespace Utils;

public static class Utilities
{

    extension(string path)
    {
        private string[] ReadAsLines() => File.Exists(path) is true
                                        ? File.ReadAllLines(path)
                                        : [];

    }

    public static string[] GetInput()
    {
        string fullExePath = Assembly.GetCallingAssembly().Location;
        var name = "inputs/" + string.Join('\0', Path.GetFileName(fullExePath).Split('.').SkipLast(1)) + ".txt";
        return name.ReadAsLines();

    }



}
