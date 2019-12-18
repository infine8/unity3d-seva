using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.WordConstructor02.Model;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.WordConstructor02.Controller
{
    public sealed class ExitCommand : SubGameExitCommandBase<WordConstructor02Settings, WordConstructor02UserData, WordConstructor02Level, WordConstructor02Stage>
    {
        [Inject]
        public WordConstructor02Core WordConstructor02Core { get; set; }


        protected override SubGameCoreBase<WordConstructor02Settings, WordConstructor02UserData, WordConstructor02Level, WordConstructor02Stage> SubGameCore
        {
            get { return WordConstructor02Core; }
        }
    }
}