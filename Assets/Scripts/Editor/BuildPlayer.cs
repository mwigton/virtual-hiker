using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class BuildPlayer : MonoBehaviour
{
    static string adbLocation;
    static string bundleIdent = PlayerSettings.bundleIdentifier;

    [MenuItem("Build/PushRun To Android %i" )]
    public static void PushToAndroid()
    {
        string apkLocation = PlayerPrefs.GetString("APK location");
        if (string.IsNullOrEmpty(apkLocation) || !File.Exists(apkLocation))
            apkLocation = EditorUtility.OpenFilePanel("Find APK", Environment.CurrentDirectory, "apk");

        if (string.IsNullOrEmpty(apkLocation) || !File.Exists(apkLocation))
        {
            Debug.LogError("Cannot find .apk file.");
            return;
        }
        PlayerPrefs.SetString("APK location", apkLocation);

        adbLocation = PlayerPrefs.GetString("Android debug bridge location");
        if (string.IsNullOrEmpty(apkLocation) || !File.Exists(adbLocation))
            adbLocation = EditorUtility.OpenFilePanel("Android debug bridge", Environment.CurrentDirectory, "exe");
        if (string.IsNullOrEmpty(apkLocation) || !File.Exists(adbLocation))
        {
            Debug.LogError("Cannot find adb.exe.");
            return;
        }
        PlayerPrefs.SetString("Android debug bridge location", adbLocation);

        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = adbLocation,
            Arguments = string.Format("install -r \"{0}\"", apkLocation),
            WorkingDirectory = Path.GetDirectoryName(adbLocation),
        };
        Process adbPushProcess = Process.Start(info);
        if (adbPushProcess != null)
        {
            adbPushProcess.EnableRaisingEvents = true;
            adbPushProcess.Exited += RunApp; 
        }
        else
        {
            Debug.LogError("Error starting adb");
        }
    }

    public static void RunApp(object o, EventArgs args)
    {
        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = adbLocation,
            Arguments = string.Format("shell am start -n " + bundleIdent + "/com.unity3d.player.UnityPlayerNativeActivity"),
            WorkingDirectory = Path.GetDirectoryName(adbLocation),
        };

        Process.Start(info);
    }

    [MenuItem("Build/Change APK to push")]
    public static void ChangeAPK()
    {
        PlayerPrefs.SetString("APK location", EditorUtility.OpenFilePanel("Find APK", Environment.CurrentDirectory, "apk"));
    }
}