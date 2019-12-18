using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.WordConstructor01.Model;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.WordConstructor01.Controller
{
    public sealed class StartCommand : SubGameStartCommandBase<WordConstructor01Settings, WordConstructor01UserData, WordConstructor01Level, WordConstructor01Stage>
    {
        [Inject]
        public WordConstructor01Core WordConstructor01Core { get; set; }


        protected override SubGameCoreBase<WordConstructor01Settings, WordConstructor01UserData, WordConstructor01Level, WordConstructor01Stage> SubGameCore
        {
            get { return WordConstructor01Core; }
        }
    }
}