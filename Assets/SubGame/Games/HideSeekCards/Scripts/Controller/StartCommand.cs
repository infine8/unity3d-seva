using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.HideSeekCards.Model;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.HideSeekCards.Controller
{
    public class StartCommand : SubGameStartCommandBase<HideSeekCardsSettings, HideSeekCardsUserData, HideSeekCardsLevel, HideSeekCardsStage>
    {
        [Inject]
        public HideSeekCardsCore HideSeekCardsCore { get; set; }

        protected override SubGameCoreBase<HideSeekCardsSettings, HideSeekCardsUserData, HideSeekCardsLevel, HideSeekCardsStage> SubGameCore
        {
            get { return HideSeekCardsCore; }
        }
    }
}