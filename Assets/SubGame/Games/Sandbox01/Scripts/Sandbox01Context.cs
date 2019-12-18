using UniKid.SubGame.Games.Sandbox01.Controller;
using UniKid.SubGame.Games.Sandbox01.Model;
using UniKid.SubGame.Games.Sandbox01.View;
using UnityEngine;
using System.Collections;

namespace UniKid.SubGame.Games.Sandbox01
{
    public sealed class Sandbox01Context : SubGameContextBase<Sandbox01Core, MainView, MainViewMediator, StartCommand, ExitCommand>
    {

        public Sandbox01Context()
        {

        }

        public Sandbox01Context(MonoBehaviour view, bool autoStartup)
            : base(view, autoStartup)
        {

        }
    }
}
