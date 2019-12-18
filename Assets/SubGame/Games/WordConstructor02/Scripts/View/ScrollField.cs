using System.Collections.Generic;
using UniKid.SubGame.Games.WordConstructor02.Model;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace UniKid.SubGame.Games.WordConstructor02.View
{
    public sealed class ScrollField : PoolObject
    {
        [Inject]
        public WordConstructor02Core WordConstructor02Core { get; set; }

        public List<Scroll> ScrollList;

        public void Init(List<tk2dTextMesh> textMeshList)
        {
            var possibleSyllableList = WordConstructor02Core.CurrentStage.PossibleSyllableSequence.SplitSequence(true);
            var syllableList = WordConstructor02Core.CurrentStage.SyllableSequence.SplitSequence(true);

            foreach (var scroll in ScrollList)
            {
                scroll.Show();

                possibleSyllableList.Shuffle();

                for (var i = 0; i < scroll.ItemList.Count; i++)
                {
                    var possibleSyllable = possibleSyllableList[i % possibleSyllableList.Count];

                    if (Scroll.CURRENT_ITEM_INDEX == i && possibleSyllable.ToUpper().Equals(syllableList[scroll.Index]))
                    {
                        possibleSyllable = scroll.ItemList[0].Syllable;
                        scroll.ItemList[0].Init(possibleSyllable, textMeshList);
                    }

                    scroll.ItemList[i].Init(possibleSyllable, textMeshList);
                }
            }
        }

        public void Hide()
        {
            ScrollList.ForEach(x => x.Hide());
        }
    }
}