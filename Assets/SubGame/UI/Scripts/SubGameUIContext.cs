using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

namespace UniKid.SubGame.UI
{
    public sealed class SubGameUIContext : MVCSContext
    {
        public SubGameUIContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {
        }

        protected override void mapBindings()
        {
        }
    }
}