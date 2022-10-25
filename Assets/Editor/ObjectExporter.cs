using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Based off version by RedBrumbler
// https://discord.com/channels/441805394323439646/583108561396170837/1028991853707673610
public class ObjectExporter : EditorWindow
{
    [MenuItem("Tools/Object Exporter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectExporter), false, "Object Exporter");
    }

    private void OnGUI()
    {
        string relativeInFolderPath = "Assets/In/";
        string absoluteInFolderPath = Path.GetFullPath(relativeInFolderPath);

        GUILayout.Label($"Assets to export (from {relativeInFolderPath}):");

        var assetsToBundle = new List<string>();
        if (!Directory.Exists(relativeInFolderPath))
        {
            GUILayout.Label($"\t!!! Missing {relativeInFolderPath} folder !!!");
            return;
        }

        foreach (var relativeFilePath in Directory.EnumerateFiles(relativeInFolderPath, "*", SearchOption.AllDirectories))
        {
            if (Path.GetExtension(relativeFilePath) == ".meta")
            {
                continue;
            }

            var name = relativeFilePath.Substring(relativeInFolderPath.Length);

            assetsToBundle.Add(name);
            GUILayout.Label($"\t- {name}");
        }

        if (assetsToBundle.Count <= 0)
        {
            GUILayout.Label($"\t(no files)");
        }

        if (GUILayout.Button("Export"))
        {
            string outFolderPath = "Assets/Out/";
            if (!Directory.Exists(outFolderPath)) Directory.CreateDirectory(outFolderPath);

            string workingDir = Application.temporaryCachePath + "/Export";
            if (!Directory.Exists(workingDir)) Directory.CreateDirectory(workingDir);

            AssetBundleBuild assetBundleBuild = default;
            assetBundleBuild.addressableNames = assetsToBundle.ToArray();
            assetBundleBuild.assetNames = assetsToBundle.ConvertAll((name) => relativeInFolderPath + name).ToArray();

            foreach (BuildTarget buildTarget in new BuildTarget[] { BuildTarget.StandaloneWindows, BuildTarget.Android }) {
                string targetOutFolderPath = outFolderPath + $"{buildTarget}/";
                if (!Directory.Exists(targetOutFolderPath)) Directory.CreateDirectory(targetOutFolderPath);

                string fileName = "Bundle.bundle";
                string fullpath = targetOutFolderPath + fileName;

                assetBundleBuild.assetBundleName = fileName;

                if (File.Exists(fullpath)) File.Delete(fullpath);

                BuildTargetGroup selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
                BuildTarget activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;

                //Build Bundle
                BuildPipeline.BuildAssetBundles(workingDir, new AssetBundleBuild[] { assetBundleBuild }, 0, buildTarget);
                EditorPrefs.SetString("currentBuildingAssetBundlePath", targetOutFolderPath);
                EditorUserBuildSettings.SwitchActiveBuildTarget(selectedBuildTargetGroup, activeBuildTarget);

                File.Move(workingDir + "/" + fileName, fullpath);
                Debug.Log($"Made bundle for Build Target {buildTarget} at {fullpath}");
            }

            clearTemp(workingDir);

            EditorUtility.DisplayDialog("Exportation Successful!", "Exportation Successful!", "OK");
            //EditorUtility.RevealInFinder(baseFolderPath);
        }

        void clearTemp(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            Directory.Delete(path);
        }
    }

    struct AssetToBundle
    {
        internal string name;
        internal UnityEngine.Object asset;
    }
}
