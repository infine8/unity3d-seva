using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.TexturePaintGame.View
{
    public sealed class PaintCharPart : EventView
    {
        [System.NonSerialized]
        public PaintChar PaintChar;

        public enum PaintCharPartEventType
        {
            OnBrushTriggerExit,
            OnBrushTriggerEnter
        }

        public void Init()
        {
            PaintChar = transform.parent.gameObject.GetComponent<PaintChar>();

            var r = gameObject.AddComponent<Rigidbody>();
            r.isKinematic = true;
            r.useGravity = false;

            gameObject.GetComponent<MeshCollider>().isTrigger = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!gameObject.CompareTag(PaintChar.CurrentPaintCharPart.tag)) return;

            dispatcher.Dispatch(PaintCharPartEventType.OnBrushTriggerExit);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (!gameObject.CompareTag(PaintChar.CurrentPaintCharPart.tag)) return;

            dispatcher.Dispatch(PaintCharPartEventType.OnBrushTriggerEnter);
        }
    }
}