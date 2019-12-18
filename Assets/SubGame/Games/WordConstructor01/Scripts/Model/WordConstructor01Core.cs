using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.WordConstructor01.Model
{
    public sealed class WordConstructor01Core : SubGameCoreBase<WordConstructor01Settings, WordConstructor01UserData, WordConstructor01Level, WordConstructor01Stage>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.WordConstructor01; }
        }
    }
}