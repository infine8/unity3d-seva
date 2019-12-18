using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.HideSeekCards.Model
{
    public sealed class HideSeekCardsCore : SubGameCoreBase<HideSeekCardsSettings, HideSeekCardsUserData, HideSeekCardsLevel, HideSeekCardsStage>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.HideSeekCards; }
        }
    }
}