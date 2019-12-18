using UniKid.Intro.View;
using UnityEngine;
using strange.extensions.context.impl;

namespace UniKid.Intro
{
    public sealed class IntroContext : MVCSContext
    {
        public IntroContext() : base()
        {

        }

        public IntroContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
            mediationBinder.Bind<MainView>().To<MainViewMediator>();
        }
    }
}