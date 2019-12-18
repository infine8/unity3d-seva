using System;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.impl;

namespace UniKid.Core.Controller
{
    public sealed class LoadSceneCommand : EventCommand
    {
        public static readonly float FADE_DURATION = 0.5f;

        private static bool _isSubGameScene;

        public override void Execute()
        {
            var sceneName = evt.data.ToString();
            var withFade = true;
            var loadAdditive = true;
            _isSubGameScene = false;

            if (evt.data is LoadSceneCommandArgument)
            {
                var arg = evt.data as LoadSceneCommandArgument;
                sceneName = arg.SceneName;
                withFade = arg.WithFade;
                loadAdditive = arg.LoadAdditive;
                _isSubGameScene = arg.IsSubGameScene;
            }

            if (string.IsNullOrEmpty(sceneName)) throw new Exception("Can't load a scene with a null or empty name");

            AudioController.StopAll(0.3f);

            if (withFade)
            {
                AutoFade.LoadLevel(sceneName, FADE_DURATION, FADE_DURATION, Color.black, loadAdditive);
            }
            else
            {
                if (loadAdditive) Application.LoadLevelAdditive(sceneName);
                else Application.LoadLevel(sceneName);
            }
        }

        public static void DestroyUnusingContexts()
        {
            foreach (ContextView contextView in GameObject.FindObjectsOfType(typeof(ContextView)))
            {
                if (contextView is CoreContextView) continue;

                if (_isSubGameScene && contextView is SubGame.UI.SubGameUIContextView) continue;

                GameObject.Destroy(contextView.gameObject);
            }
        }
    }

    public sealed class LoadSceneCommandArgument
    {
        public string SceneName { get; set; }

        public bool WithFade { get; set; }

        public bool LoadAdditive { get; set; }

        public bool IsSubGameScene { get; set; }
    }
}