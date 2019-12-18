using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.TexturePaintGame.Model;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.TexturePaintGame.Controller
{
    public sealed class ExitCommand : SubGameExitCommandBase<TexturePaintSettings, TexturePaintUserData, TexturePaintLevel, TexturePaintStage>
    {
        [Inject]
        public TexturePaintCore TexturePaintCore { get; set; }


        protected override SubGameCoreBase<TexturePaintSettings, TexturePaintUserData, TexturePaintLevel, TexturePaintStage> SubGameCore
        {
            get { return TexturePaintCore; }
        }
    }
}