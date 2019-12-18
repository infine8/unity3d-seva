using UnityEngine;
using System.Collections;
#if UNITY_IPHONE && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

/** Attach TestFlight.cs to a GameObject and set the appropriate app tokens for iOS and/or Android.
 * call TestFlight.passCheckpoint(...) and TestFlight.log(...); to log messages and CheckPoints to the TestFlight backend.
 * Disable Log Scene Load if you do not wan't scene loading to be automaticly logged.
 * Thats it!
 **/
public class TestFlight : MonoBehaviour 
{	
	/// log when levels are started and finished
	public bool logSceneLoad = true;
	public string appTokenAndroid = "";
	public string appTokenIOS = "";
	private static TestFlight instance = null;
	/** When a tester does something you care about in your app you can pass a checkpoint. 
	 * For example completing a level, adding a todo item, etc. 
	 * The checkpoint progress is used to provide insight into how your testers are testing your apps. 
	 * The passed checkpoints are also attached to crashes, which can help when creating steps to replicate.
	 * Use passCheckpoint() to track when a user performs certain tasks in your application. 
	 * This can be useful for making sure testers are hitting all parts of your application, 
	 * as well as tracking which testers are being thorough.
	 **/
	public static void passCheckpoint(string checkPointName)
	{
		if(hasTakenOff)
		{
			LogInternal("Unity.TestFlight.passCheckpoint('"+checkPointName+"');");
#if UNITY_ANDROID && !UNITY_EDITOR
			testFlightClass.CallStatic("passCheckpoint", checkPointName);
#elif UNITY_IPHONE && !UNITY_EDITOR
			_iOS_TestFlight_passCheckpoint(checkPointName);
#endif
		}
		else
		{
			Debug.Log("TestFlight has not taken off. Wrong App Token or missing TestFlight GameObject?");
		}
	}
	
	/** To perform remote logging you can use the TestFlight.log() method.
	 **/ 
	public static void log(string message)
	{
		if(hasTakenOff)
		{
			LogInternal("Unity.TestFlight.log('"+message+"');");
#if UNITY_ANDROID && !UNITY_EDITOR
			testFlightClass.CallStatic("log", message);
#elif UNITY_IPHONE && !UNITY_EDITOR
			_iOS_TestFlight_log(message);
#endif	
		}
		else
		{
			Debug.Log("TestFlight has not taken off. Wrong App Token or missing TestFlight GameObject?");
		}	
	}
	
	/** open view to submit feedback (!!! iOS only !!!)
	 **/ 
	public static void openFeedbackView()
	{
		if(hasTakenOff)
		{
			LogInternal("Unity.TestFlight.openFeedbackView();");
#if UNITY_ANDROID && !UNITY_EDITOR
			
#elif UNITY_IPHONE && !UNITY_EDITOR
			_iOS_TestFlight_openFeedbackView();
#endif	
		}
		else
		{
			Debug.Log("TestFlight has not taken off. Wrong App Token or missing TestFlight GameObject?");
		}	
	}
	/** Crash the app. For testing purposes. :o)
	 **/ 
	public static void forceCrash()
	{
		if(hasTakenOff)
		{
			LogInternal("Unity.TestFlight.forceCrash();");
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.FatalError("TestFlight.forceCrash");
#elif UNITY_IPHONE && !UNITY_EDITOR
			_iOS_TestFlight_forceCrash();
#endif	
		}
		else
		{
			Debug.Log("TestFlight has not taken off. Wrong App Token or missing TestFlight GameObject?");
		}	
	}
	/* --------------- Internal -------------------------------------------- */	
	private static string _appTokenIOS = "";
	private static string _appTokenAndroid = "";
	private static bool hasTakenOff = false;
	private string oldSceneName = "";
	
#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaClass testFlightClass = null;
#elif UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern void _iOS_TestFlight_takeOff(string appToken);	
	[DllImport ("__Internal")]
	private static extern void _iOS_TestFlight_log(string message);
	[DllImport ("__Internal")]
	private static extern void _iOS_TestFlight_passCheckpoint(string checkpoint);
	[DllImport ("__Internal")]
	private static extern void _iOS_TestFlight_submitFeedback(string feedback);
	[DllImport ("__Internal")]
	private static extern void _iOS_TestFlight_openFeedbackView();
	[DllImport ("__Internal")]
	private static extern void _iOS_TestFlight_forceCrash();
#endif
	void Awake() 
	{
		if(TestFlight.instance == null)
		{
			TestFlight.instance = this;
			DontDestroyOnLoad(this.gameObject);
			_appTokenIOS = this.appTokenIOS;
			_appTokenAndroid = this.appTokenAndroid;
			TestFlight.takeOff();
		}
		else
		{
			this.gameObject.SetActive(false);
			this.logSceneLoad = false;
			GameObject.Destroy(this.gameObject);
		}
	}
	
	public void Update ()
	{
		if(this.logSceneLoad)
		{
			string newSceneName = Application.loadedLevelName;
			if(newSceneName != oldSceneName)
			{
				if(this.oldSceneName != null && this.oldSceneName != string.Empty)
				{
					TestFlight.passCheckpoint("FinishedLevel__"+this.oldSceneName);
					//TestFlight.log("FinishedLevel__"+this.oldSceneName);
				}
				if(newSceneName != null && newSceneName != string.Empty)
				{
					TestFlight.passCheckpoint("LoadedLevel__"+newSceneName);
					//TestFlight.log("LoadedLevel__"+newSceneName);
				}
				this.oldSceneName = newSceneName;
			}	
		}
	}
	
	private static void takeOff()
	{
		LogInternal("Unity.TestFlight.takeOff('"+_appTokenIOS+"', '"+_appTokenAndroid+"');");
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityPlayerJava = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activityJava = unityPlayerJava.GetStatic<AndroidJavaObject>("currentActivity");
		activityJava.Call("runOnUiThread", new AndroidJavaRunnable(takeOffOnUiThread));
#elif UNITY_IPHONE && !UNITY_EDITOR
		_iOS_TestFlight_takeOff(_appTokenIOS);
		hasTakenOff = true;
#else
		hasTakenOff = true;
#endif
	}
	
	private static void takeOffOnUiThread()
	{
		LogInternal("Unity.TestFlight.takeOffOnUiThread('"+_appTokenIOS+"', '"+_appTokenAndroid+"');");
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityPlayerJava = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activityJava = unityPlayerJava.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject applicationJava = activityJava.Call<AndroidJavaObject>("getApplication");
		testFlightClass = new AndroidJavaClass("com.testflightapp.lib.TestFlight"); 
		testFlightClass.CallStatic("takeOff", applicationJava, _appTokenAndroid);
#endif
		hasTakenOff = true;
	}
	
	private static void LogInternal(string message)
	{
		//Debug.Log(message);
	}
	
	/*
	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width/2-50,Screen.height/2-25,100,50),"Feedback"))
		{
			TestFlight.openFeedbackView();
		}
	}
	*/
}


