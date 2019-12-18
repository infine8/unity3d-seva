using UnityEngine;

namespace UniKid.SubGame.Games.BubbleGuessing.View
{
    public sealed class FindingChar : PoolObject
    {
        public LibraryCharOffset[] LibraryCharOffsets;

        [System.Serializable]
        public class LibraryCharOffset
        {
            public string LibraryName;
            public CharOffset[] CharOffsets;

            [System.Serializable]
            public class CharOffset
            {
                [SerializeField]
                public string Char;
                [SerializeField]
                public float YOffset;
                [SerializeField]
                public float XSize;
            }
        }
    }
}