using System.Collections;
using Holoville.HOTween;
using UniKid.Core.Model.Xml;
using UniKid.Core.Service;
using UnityEngine;
using strange.extensions.context.impl;

namespace UniKid.Core
{
    public sealed class CoreContextView : ContextView
    {
        [SerializeField]
        private bool _isDebugMode = false;
        [SerializeField]
        private GameObject _debugInfoObject;

        private void Awake()
        {
            CoreContext.IsDebugMode = _isDebugMode;


            CoreContext.IS_STANDALONE_BUILD = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer;

            context = new CoreContext(this, true, _debugInfoObject);
            context.Start();
            Initialize();

            HOTween.Init();

            DontDestroyOnLoad(this);
        }

        private void Initialize()
        {

#if UNITY_WEBPLAYER && !UNITY_EDITOR
        StartCoroutine(LoadWebSettings());
#else
            CoreContext.Settings = XmlSettings.Create();

            CoreContext.UserData = XmlUserData.Create();

            XmlUserData.UpdateSubGameBaseInfoList(CoreContext.Settings.SubGameBaseInfoList);

#endif
            CoreContext.AudioController = new Core.Model.Audio.AudioController();
            if (!CoreContext.AudioController.LoadSnapshot(CoreContext.UserData.Common.AudioControllerSnapshot))
                CoreContext.AudioController.LoadSnapshot(CoreContext.Settings.DefaultAudioControllerSnapshot);

            Application.LoadLevelAdditive(CoreContext.Settings.GameSceneArray.StartSceneName);
        }

        public IEnumerator LoadWebSettings()
        {
            var www = new WWW(Application.dataPath + @"/settings.xml");
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
            }
            else
            {
                CoreContext.Settings = XmlSettings.Create(www.text);
                CoreContext.UserData = XmlUserData.Create();
            }
        }

        private void OnApplicationQuit()
        {
            CoreContext.Save();
        }

#if (UNITY_IPHONE) && (!UNITY_EDITOR)
	    private void OnApplicationPause(bool pause)
	    {
		    if (pause) CoreContext.Save();
	    }
#endif
    }
}


