using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.Sandbox01.Model;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.Sandbox01.Controller
{
    public sealed class StartCommand : SubGameStartCommandBase<Sandbox01Settings, Sandbox01UserData, Sandbox01Level, Sandbox01Stage>
    {
        [Inject]
        public Sandbox01Core Sandbox01Core { get; set; }

        protected override SubGameCoreBase<Sandbox01Settings, Sandbox01UserData, Sandbox01Level, Sandbox01Stage> SubGameCore
        {
            get { return Sandbox01Core; }
        }
    }
}