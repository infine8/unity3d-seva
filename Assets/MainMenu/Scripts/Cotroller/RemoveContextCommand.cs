using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;

namespace UniKid.MainMenu.Controller
{
    public class RemoveContextCommand : EventCommand
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        public override void Execute()
        {
            GameObject.Destroy(contextView);
        }

    }
}