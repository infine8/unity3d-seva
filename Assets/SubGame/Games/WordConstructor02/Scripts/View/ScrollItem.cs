using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.WordConstructor02.View
{
    public sealed class ScrollItem : EventView
    {
        public int Index;

        public string Syllable { get; private set; }

        private tk2dTextMesh _textMesh;

        public void Init(string syllable, List<tk2dTextMesh> textMeshList)
        {
            Syllable = syllable;

            var textMeshOrig = textMeshList.Find(x => x.name.EndsWith(syllable.Length.ToString()));

            if (_textMesh != null) PoolManager.Despawn(_textMesh.gameObject);

            _textMesh = PoolManager.Spawn(textMeshOrig.gameObject, transform).GetComponent<tk2dTextMesh>();

            _textMesh.transform.localPosition = textMeshOrig.transform.localPosition;

            _textMesh.text = syllable;

            _textMesh.Commit();
        }
    }
}