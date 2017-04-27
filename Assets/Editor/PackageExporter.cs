using System.IO;
using UnityEngine;
using UnityEditor;

public static class PackageExporter
{
	[MenuItem("Package/Export Unity Packages")]
	public static void ExportUnityPackage()
	{
		Export("UniMail.unitypackage", new[] { "Assets/Plugins/UniMail" });
		Export("UniBugReporter.unitypackage", new[] { "Assets/Plugins/UniBugReporter" });
		Debug.LogFormat("Success");
	}

	private static void Export(string packageName, string[] libraryDirectories)
	{
		var outputFilePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), packageName);
		Debug.LogFormat("Export package to {0}", outputFilePath);
		AssetDatabase.ExportPackage(libraryDirectories, outputFilePath, ExportPackageOptions.Recurse);
	}
}
