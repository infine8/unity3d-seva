using System;
using System.Collections.Generic;
using TouchScript.Gestures;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public sealed class PaintChar : EventView
    {
        public GameObject Waypoints;
        public Waypoint FirstWaypoint;

        public PaintCharPart CurrentPaintCharPart { get; private set; }

        private readonly List<PaintCharPart> _paintCharPartList = new List<PaintCharPart>();

        private int _currentCharPartIndex = 0;

        public void Init()
        {
            _currentCharPartIndex = 0;
            _paintCharPartList.AddRange(GetComponentsInChildren<PaintCharPart>());
            _paintCharPartList.ForEach(x => x.Init());
        }

        public void AddComponentToParts(Type componentyType)
        {
            _paintCharPartList.ForEach(x => x.gameObject.AddComponent(componentyType));
        }

        public void AddTapGestureComponent(EventHandler<TouchScript.Events.GestureStateChangeEventArgs> stateChageAction)
        {
            _paintCharPartList.ForEach(x =>
            {
                x.gameObject.AddComponent<TouchScript.Gestures.TapGesture>().StateChanged += stateChageAction;                                  
            });
        }


        public void MoveNextCharPart()
        {
            _currentCharPartIndex++;

            CurrentPaintCharPart = _paintCharPartList.Find(x => x.CompareTag(GetCurrentCharPartTag()));

            var currentUntouchable = CurrentPaintCharPart.gameObject.GetComponent<TouchScript.Hit.Untouchable.Untouchable>();
            if (currentUntouchable != null) Destroy(currentUntouchable);

            _paintCharPartList.ForEach(x =>
            {
                if (x.GetInstanceID() != CurrentPaintCharPart.GetInstanceID() && x.GetComponent<TouchScript.Hit.Untouchable.Untouchable>() == null) 
                    x.gameObject.AddComponent<TouchScript.Hit.Untouchable.Untouchable>();
            });

        }

        private string GetCurrentCharPartTag()
        {
            return string.Format(Const.TAG_TexturePaint_PaintChar_Part_Teplate, _currentCharPartIndex);
        }

    }
}