using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

namespace UniKid.MainMenu
{

    public class MainMenuContextView : ContextView
    {

        void Awake()
        {
            context = new MainMenuContext(this, true);
            context.Start();
        }
    }

}
