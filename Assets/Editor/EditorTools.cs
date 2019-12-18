using System;
using System.Diagnostics;
using System.Linq;
using UniKid.SubGame.Controller;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class EditorTools : MonoBehaviour
{
    private static readonly string BUILD_FILE_NAME = "Seva";

    [MenuItem("Build/Windows Build")]
    public static void BuildWin()
    {
        BuildGame(BuildTarget.StandaloneWindows, RuntimePlatform.WindowsPlayer, true);
    }

    [MenuItem("Build/OSX Build")]
    public static void BuildOSX()
    {
        BuildGame(BuildTarget.StandaloneOSXUniversal, RuntimePlatform.OSXPlayer, true);
    }

    [MenuItem("Build/Android Build")]
    public static void BuildAndroid()
    {
        BuildGame(BuildTarget.Android, RuntimePlatform.Android, true);
    }

    [MenuItem("Build/IOS Build")]
    public static void BuildIOS()
    {
        BuildGame(BuildTarget.iPhone, RuntimePlatform.IPhonePlayer, true);
    }

    private static void BuildGame(BuildTarget buildTarget, RuntimePlatform platform, bool isStandalone)
    {
        log4net.LogConfigXml.SetBuildAppender(platform);

        var path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

        var ext = string.Empty;

        if (buildTarget == BuildTarget.StandaloneWindows) ext = "exe";
        if (buildTarget == BuildTarget.StandaloneOSXUniversal) ext = "app";

        BuildPipeline.BuildPlayer(GetLevelsToBuild(), string.Format("{0}/{1}.{2}", path, BUILD_FILE_NAME, ext), buildTarget, BuildOptions.None);

        if (isStandalone) StandalonePostprocess(path, buildTarget);
    }


    private static void StandalonePostprocess(string path, BuildTarget buildTarget)
    {
        var appDataFolder = string.Empty;

        if (buildTarget == BuildTarget.StandaloneWindows) appDataFolder = string.Format("{0}/{1}_Data", path, BUILD_FILE_NAME);
        if (buildTarget == BuildTarget.StandaloneOSXUniversal) appDataFolder = string.Format("{0}/{1}.app/Contents/Data", path, BUILD_FILE_NAME);

        FileUtil.CopyFileOrDirectory(Application.dataPath + "/Common/Resources/settings.xml", string.Format("{0}/settings.xml", appDataFolder));


        foreach (var name in Enum.GetValues(typeof(SubGameName)))
        {
            var subGameFolder = System.IO.Directory.CreateDirectory(appDataFolder + "/" + name);

            FileUtil.CopyFileOrDirectory(string.Format("{0}/SubGame/Games/{1}/Resources/{1}/settings.xml", Application.dataPath, name),
                string.Format("{0}/settings.xml", subGameFolder.FullName.Replace(@"\", "/")));
        }

        //var proc = new Process();
        //proc.StartInfo.FileName = path + "/" + BUILD_FILE_NAME + ".exe";
        //proc.Start(); 
    }

    private static string[] GetLevelsToBuild()
    {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
    }
}
