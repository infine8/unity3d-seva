using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.MathMushroom
{
    public sealed class MathMushroomContext : MVCSContext
    {
        public MathMushroomContext()
        {

        }

        public MathMushroomContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {

        }

        protected override void mapBindings()
        {
        }
    }
}