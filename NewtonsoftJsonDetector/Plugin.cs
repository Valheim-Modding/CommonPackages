using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;

namespace NewtonsoftJsonDetector;

[BepInPlugin("com.ValheimModding.NewtonsoftJsonDetector", "Newtonsoft.Json Detector", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    private const string AssemblyName = "Newtonsoft.Json";

    private void Awake()
    {
        if (IsAssemblyLoaded(out var assembly) && assembly != null)
        {
            Logger.LogInfo($"{AssemblyName} {assembly.GetName().Version} assembly already loaded from {RelativePath(assembly.Location)}");
            return;
        }

        string path = Path.Combine(Path.GetDirectoryName(Info.Location) ?? string.Empty, $"{AssemblyName}.dll");

        if (File.Exists(path))
        {
            Assembly.LoadFrom(path);
        }
        else
        {
            Assembly.Load(AssemblyName);
        }

        if (IsAssemblyLoaded(out assembly) && assembly != null)
        {
            Logger.LogInfo($"{AssemblyName} {assembly.GetName().Version} loaded from {RelativePath(assembly.Location)}");
        }
        else
        {
            Logger.LogError($"{AssemblyName} assembly not loaded");
        }
    }

    public static bool IsAssemblyLoaded(out Assembly? assembly)
    {
        assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == AssemblyName);
        return assembly != null;
    }

    public static string RelativePath(string path)
    {
        return path
            .Replace(Paths.BepInExRootPath, string.Empty)
            .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }
}
