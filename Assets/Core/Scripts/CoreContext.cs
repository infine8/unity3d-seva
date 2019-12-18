using System;
using System.Collections;
using UniKid.Core.Controller;
using UniKid.Core.Model;
using UniKid.Core.Model.Xml;
using UniKid.Core.Service;
using UniKid.SubGame.Controller;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;

namespace UniKid.Core
{
    public sealed class CoreContext : MVCSContext
    {
        public const bool USE_ENCRYPTION = false;
        public static bool IS_STANDALONE_BUILD;

        private static CoreContext _instance;

        private SettingsBase _settings;
        private UserDataBase _userData;

        private static GameObject DebugInfoObject { get; set; }

        public static bool IsDebugMode { get; set; }

        public static SettingsBase Settings { get { return _instance._settings; } set { _instance._settings = value; } }
        public static UserDataBase UserData { get { return _instance._userData; } set { _instance._userData = value; } }
        public static MonoBehaviour ContextView { get; private set; }
        public static NetworkingCore NetworkingCore { get; private set; }

        public static Core.Model.Audio.AudioController AudioController { get; set; }
        public static SubGameName LoadedSubGameName { get; set; }
        public static bool PlayParticularGame { get; set; }

        public CoreContext () {}

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return ContextView.StartCoroutine(routine);
        }

        public CoreContext(MonoBehaviour view, bool autoStartup, GameObject debugInfoObject) : base(view, autoStartup)
        {
            _instance = this;

            ContextView = view;

            if (IsDebugMode)
            {
                DebugInfoObject = (GameObject)GameObject.Instantiate(debugInfoObject);
                DebugInfoObject.transform.parent = view.transform;
                DebugInfoObject.SetActive(false);
            }

            NetworkingCore = new NetworkingCore();
        }

        protected override void mapBindings()
        {
            commandBinder.Bind(ContextEvent.START).To<CoreStartCommand>().Once();
            commandBinder.Bind(CoreEventType.LoadMainMenu).To<LoadMainMenuCommand>();
            commandBinder.Bind(CoreEventType.LoadScene).To<LoadSceneCommand>();

            commandBinder.Bind(CoreEventType.DbUpdateUserData).To<DbUpdateUserDataCommand>();

            injectionBinder.Bind<IDbService>().To<ParseDbService>().CrossContext().ToSingleton();

        }

        public static void Save()
        {
            UserData.Common.AudioControllerSnapshot = AudioController.CreateSnapshot();

#if !UNITY_WEBPLAYER || UNITY_EDITOR
            UserData.Save();
#endif
        }

        public static void SetDebugInfoActive(bool isActive)
        {
            if (DebugInfoObject == null) return;

            DebugInfoObject.SetActive(isActive);
        }

    }
}
