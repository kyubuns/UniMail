using System.IO;
using UnityEngine;
using UnityEditor;

public static class PackageExporter
{
	[MenuItem("Package/Export Unity Package")]
	public static void ExportUnityPackage()
	{
		var libraryDirectories = new[]
		{
			"Assets/Plugins/UniMail"
		};

		var outputFilePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "UniMail.unitypackage");
		Debug.LogFormat("Export package to {0}", outputFilePath);
		AssetDatabase.ExportPackage(libraryDirectories, outputFilePath, ExportPackageOptions.Recurse);
		Debug.LogFormat("Success");
	}
}