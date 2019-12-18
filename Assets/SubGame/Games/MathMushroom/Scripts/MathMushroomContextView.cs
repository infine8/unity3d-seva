using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

namespace UniKid.SubGame.Games.MathMushroom
{
    public sealed class MathMushroomContextView : ContextView
    {
        void Awake()
        {
            context = new MathMushroomContext(this, true);
            context.Start();
        }
    }
}