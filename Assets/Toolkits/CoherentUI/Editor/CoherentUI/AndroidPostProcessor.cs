using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Xml;

public static class AndroidPostProcessor
{	
	class KeyStoreInfo
	{
		public string KeyStoreFile;
		public string StorePass;
		public string KeyPass;
		public string Alias;
		
		public KeyStoreInfo()
		{
			// Construct the default Android debug keystore info
			// http://developer.android.com/tools/publishing/app-signing.html
			KeyStoreFile = Path.Combine(AndroidPostProcessor.GetUserDir(), ".android/debug.keystore");
			StorePass = "android";
			KeyPass = "android";
			Alias = "androiddebugkey";
		}
	}
	
	private static string APKToolDir = null;
	
	private static string GetAPKToolDir()
	{
		if (!string.IsNullOrEmpty(APKToolDir))
		{
			return APKToolDir;
		}
		
		string path = Path.Combine(Application.dataPath, "CoherentUI/Editor/apktool1.5.2");
		if (!Directory.Exists(path))
		{
			path = Path.Combine(Application.dataPath, "Editor/apktool1.5.2");
			if (!Directory.Exists(path))
			{
				UnityEngine.Debug.LogError("Unable to locate APKTool path!");
				return null;
			}
		}
		
		APKToolDir = path;
		return APKToolDir;
	}
	
	private static bool IsWindowsHost()
	{
		return
			Environment.OSVersion.Platform == PlatformID.Win32NT ||
			Environment.OSVersion.Platform == PlatformID.Win32S ||
			Environment.OSVersion.Platform == PlatformID.Win32Windows ||
			Environment.OSVersion.Platform == PlatformID.WinCE;
	}
	
	private static string GetUserDir()
	{
		string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
		if (IsWindowsHost() &&
			Environment.OSVersion.Version.Major >= 6)
		{
			path = Directory.GetParent(path).FullName;
		}
		return path;
	}
	
	private static KeyStoreInfo GetKeyStoreInfo()
	{
		KeyStoreInfo ksInfo = new KeyStoreInfo();
		
		if (!string.IsNullOrEmpty(PlayerSettings.Android.keystoreName))
		{
			ksInfo.KeyStoreFile = PlayerSettings.Android.keystoreName;
			ksInfo.StorePass = PlayerSettings.Android.keystorePass;
			ksInfo.KeyPass = PlayerSettings.Android.keyaliasPass;
			ksInfo.Alias = PlayerSettings.Android.keyaliasName;
		}
		
		return ksInfo;
	}
	
	private static void StartProcess(string processName, string args)
	{
		Process process = new Process();
		process.StartInfo.FileName = processName;
		process.StartInfo.Arguments = args;
		process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
		string pathEnv = process.StartInfo.EnvironmentVariables["PATH"];
		char pathSeparator = IsWindowsHost() ? ';' : ':';
		pathEnv += pathSeparator + GetAPKToolDir();
		process.StartInfo.EnvironmentVariables["PATH"] = pathEnv;
		
		//UnityEngine.Debug.Log (processName + " " + args);
		
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;

		StringBuilder output = new StringBuilder();
		StringBuilder error = new StringBuilder();
		int timeout = 30000;
		
		using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
		{
			using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
			{
				process.OutputDataReceived += (sender, e) => {
					if (e.Data == null)
					{
						outputWaitHandle.Set();
					}
					else
					{
						output.AppendLine(e.Data);
					}
				};
				process.ErrorDataReceived += (sender, e) =>
				{
					if (e.Data == null)
					{
						errorWaitHandle.Set();
					}
					else
					{
						error.AppendLine(e.Data);
					}
				};
		
				process.Start();
		
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
		
				if (process.WaitForExit(timeout) &&
					outputWaitHandle.WaitOne(timeout) &&
					errorWaitHandle.WaitOne(timeout))
				{
					// Process completed
					if (output.Length > 0)
					{
						UnityEngine.Debug.Log(processName + ": " + output.ToString());
					}
					if (error.Length > 0)
					{
						UnityEngine.Debug.LogWarning(processName + ": " + error.ToString());
					}
				}
				else
				{
					// Timed out
					UnityEngine.Debug.LogError(processName + " timed out!");
				}
			}
		}
	}
	
	public static void ModifyManifestFile(string pathToManifest, bool apiLevel11OrGreater)
	{
		XmlDocument doc = new XmlDocument();
		doc.Load(pathToManifest);
		XmlElement manifest = doc.DocumentElement;
		if (manifest.Name != "manifest")
		{
			throw new ApplicationException("Invalid Android.xml file: manifest element is not root!");
		}

		XmlAttribute androidNamespace = manifest.Attributes["xmlns:android"];
		if (androidNamespace == null)
		{
			throw new ApplicationException("Invalid Android.xml file: unable to determine android namespace!");
		}

		XmlElement application = manifest["application"];
		if (application == null)
		{
			throw new ApplicationException("Invalid Android.xml file: unable to locate application element!");
		}

		if (apiLevel11OrGreater)
		{
			// Enable hardware acceleration
			XmlAttribute attr = doc.CreateAttribute("android", "hardwareAccelerated", androidNamespace.Value);
			attr.Value = "true";
			application.Attributes.Append(attr);
		}

		// Enable forwarding native events to VM for native activities
		XmlNode nativeActivity = null;
		foreach (XmlNode child in application.ChildNodes)
		{
			if (child.Name == "activity")
			{
				XmlAttribute name = child.Attributes["android:name"];
				if (name != null && name.Value.EndsWith("NativeActivity"))
				{
					nativeActivity = child;
					break;
				}
			}
		}

		if (nativeActivity != null)
		{
			XmlNode metaData = null;
			if (nativeActivity != null)
			{
				foreach (XmlNode child in nativeActivity.ChildNodes)
				{
					if (child.Name == "meta-data")
					{
						XmlAttribute name = child.Attributes["android:name"];
						if (name != null && name.Value == "unityplayer.ForwardNativeEventsToDalvik")
						{
							metaData = child;
						}
					}
				}
			}

			if (metaData == null)
			{
				// Create and append element
				metaData = doc.CreateElement("meta-data");

				XmlAttribute attr = doc.CreateAttribute("android", "name", androidNamespace.Value);
				attr.Value = "unityplayer.ForwardNativeEventsToDalvik";
				metaData.Attributes.Append(attr);

				attr = doc.CreateAttribute("android", "value", androidNamespace.Value);
				attr.Value = "true";
				metaData.Attributes.Append(attr);

				nativeActivity.AppendChild(metaData);
			}
			else
			{
				// Make sure the value is true
				XmlAttribute val = metaData.Attributes["android:value"];
				if (val == null)
				{
					XmlAttribute attr = doc.CreateAttribute("android", "value", androidNamespace.Value);
					attr.Value = "true";
					metaData.Attributes.Append(attr);
				}
				else
				{
					val.Value = "true";
				}
			}
		}

		doc.Save(pathToManifest);
	}
	
	public static void UnpackAPK(string pathToApk, string dirToUnpack)
	{
		StartProcess("java", string.Format("-jar \"{0}\" d -f \"{1}\" \"{2}\"", Path.Combine(GetAPKToolDir(), "apktool.jar"), pathToApk, dirToUnpack));
	}
	
	public static void RepackAPK(string pathToApk, string unpackedDir)
	{
		bool apiLevel11OrGreater = (PlayerSettings.Android.minSdkVersion >= AndroidSdkVersions.AndroidApiLevel11);
		ModifyManifestFile(Path.Combine(unpackedDir, "AndroidManifest.xml"), apiLevel11OrGreater);
		
		CleanStreamingAssets(Path.Combine(unpackedDir, "assets"));
		
		// Pack the unpacked apk
		StartProcess("java", string.Format("-jar \"{0}\" b \"{1}\"", Path.Combine(GetAPKToolDir(), "apktool.jar"), unpackedDir));
		
		// Sign the apk
		string apkName = Path.GetFileName(pathToApk);
		string pathToRepackedApk = Path.Combine(Path.Combine(unpackedDir, "dist"), apkName);
		KeyStoreInfo ksInfo = GetKeyStoreInfo();
		StartProcess("jarsigner",
			string.Format("-sigalg MD5withRSA -digestalg SHA1 -keystore \"{0}\" -storepass {1} -keypass {2} \"{3}\" {4}",
				ksInfo.KeyStoreFile,
				ksInfo.StorePass,
				ksInfo.KeyPass,
				pathToRepackedApk,
				ksInfo.Alias
			)
		);
		
		// Align the apk
		
		string pathToZipAlign = Path.Combine(GetAndroidSDKDir(), "tools/zipalign");
		StartProcess(pathToZipAlign, string.Format("-f 4 \"{0}\" \"{1}\"", pathToRepackedApk, pathToApk));
		
		// Delete the temp dir
		Directory.Delete(unpackedDir, true);
	}
	
	private static string GetAndroidSDKDir()
	{
		const string AndroidSdkRootKey = "AndroidSdkRoot";
		
		if (EditorPrefs.HasKey(AndroidSdkRootKey))
		{
			return EditorPrefs.GetString(AndroidSdkRootKey);
		}
		else
		{
			string sdkRoot = EditorUtility.OpenFolderPanel("Please select Android SDK directory", "", "");
			return sdkRoot;
		}
	}
	
	private static void CleanStreamingAssets(string dataFolder)
	{		
		string[] dlls = {
			Path.Combine(dataFolder, "CoherentUINet.dll64"),
			Path.Combine(dataFolder, "CoherentUI64_Native.dll"),
		};
		
		foreach (var file in dlls)
		{
			if (File.Exists(file))
			{
				File.Delete(file);
			}
		}
		
		string hostDir = Path.Combine(dataFolder, "CoherentUI_Host");
		if(Directory.Exists(hostDir))
		{
			Directory.Delete(hostDir, true);
		}
	}
	
	public static void CleanUpForAndroid(string pluginsFolder)
	{
		DirectoryInfo dirInfo = new DirectoryInfo(pluginsFolder);
		if (dirInfo == null || !dirInfo.Exists)
		{
			return;
		}
		
		// Delete every directory != "Android"
		foreach (var item in dirInfo.GetDirectories())
		{
			if (item.Name != "Android")
			{
				Directory.Delete(item.FullName, true);
			}
		}
		
		// Delete every file that doesn't match CoherentUIMobile*
		foreach (var item in dirInfo.GetFiles())
		{
			if (!item.Name.StartsWith("CoherentUIMobile"))
			{
				File.Delete(item.FullName);
			}
		}
	}
}

