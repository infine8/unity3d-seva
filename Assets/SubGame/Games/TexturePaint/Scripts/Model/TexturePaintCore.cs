using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.TexturePaintGame.Model
{
    public sealed class TexturePaintCore : SubGameCoreBase<TexturePaintSettings, TexturePaintUserData, TexturePaintLevel, TexturePaintStage>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.TexturePaint; }
        }
    }
}
