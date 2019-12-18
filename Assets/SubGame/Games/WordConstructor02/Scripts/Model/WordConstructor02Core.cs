using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.WordConstructor02.Model
{
    public sealed class WordConstructor02Core : SubGameCoreBase<WordConstructor02Settings, WordConstructor02UserData, WordConstructor02Level, WordConstructor02Stage>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.WordConstructor02; }
        }
    }
}