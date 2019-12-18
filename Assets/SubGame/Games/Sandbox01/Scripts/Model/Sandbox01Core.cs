using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.Sandbox01.Model
{
    public sealed class Sandbox01Core : SubGameCoreBase<Sandbox01Settings, Sandbox01UserData, Sandbox01Level, Sandbox01Stage>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.Sandbox01; }
        }
    }
}