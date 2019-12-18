using System.Collections.Generic;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.SeaGuessing.Model
{
    public sealed class SeaGuessingCore : SubGameCoreBase<SeaGuessingSettings, SeaGuessingUserData, SeaGuessinggLevel, SeaGuessingStage>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.SeaGuessing; }
        }

        public List<string> PossibleCharList
        {
            get
            {
                return CurrentStage.PossibleCharSequence.SplitSequence();
            }
        }
    }
}