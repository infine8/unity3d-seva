using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;


namespace UniKid.Core.Controller
{
    public class CoreStartCommand : EventCommand
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }
        public override void Execute()
        {
        }
    }
}


