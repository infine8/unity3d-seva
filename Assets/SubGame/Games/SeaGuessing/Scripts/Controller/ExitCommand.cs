﻿using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.SeaGuessing.Model;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.SeaGuessing.Controller
{
    public class ExitCommand : SubGameExitCommandBase<SeaGuessingSettings, SeaGuessingUserData, SeaGuessinggLevel, SeaGuessingStage>
    {
        [Inject]
        public SeaGuessingCore SeaGuessingCore { get; set; }

        protected override SubGameCoreBase<SeaGuessingSettings, SeaGuessingUserData, SeaGuessinggLevel, SeaGuessingStage> SubGameCore
        {
            get { return SeaGuessingCore; }
        }

        public override void Execute()
        {
            base.Execute();
        }
    }
}