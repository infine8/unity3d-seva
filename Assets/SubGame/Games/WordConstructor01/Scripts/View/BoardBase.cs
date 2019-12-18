using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.WordConstructor01.View
{
    public abstract class BoardBase : EventView
    {
        public int Index;

        public tk2dSprite CharSprite;

        public char Char { get; set; }
    }
}